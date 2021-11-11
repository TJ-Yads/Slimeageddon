using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameManage gameManager;
    private SurvivialManager Survivialmanager;
    private StatsManager statManager;
    public Rigidbody2D rb;
    public float speed;
    private PlayerController playercontroller;
    public float Size, Power, Health;
    public float JumpHeight;
    public bool Jump;
    private float lifetime;
    private GameObject PlayerControllerObject;
    private int roll;
    public Transform FXAbsorb;
    public GameObject FXAbosrbOBJ, DeathFX, AttackFX;
    private int Hits;
    // Start is called before the first frame update
    void Start()//set component data and use the Size methods based on the gamemode, set speed and lifetime values to a random values
    {
        GameObject GameMangerOBJ = GameObject.FindWithTag("GameManage");
        gameManager = GameMangerOBJ.GetComponent<GameManage>();
        statManager = GameMangerOBJ.GetComponent<StatsManager>();
        rb = GetComponent<Rigidbody2D>();
        PlayerControllerObject = GameObject.FindWithTag("Player");
        playercontroller = PlayerControllerObject.GetComponent<PlayerController>();
        if (gameManager.Survivial == 1)
        {
            if (playercontroller == null)
            {
                SetSize(1);
            }
            else
                SetSize(playercontroller.Size);
        }
        if (gameManager.Survivial == 0)
        {
            if (playercontroller == null)
            {
                SetSize(1);
            }
            else
                SurvivialSize();
        }
        UpdateSize();
        speed = Random.Range(3, 7);
        lifetime = Random.Range(3, 7);
        StartCoroutine(Movement());
    }

    // Update is called once per frame
    void Update()
    {
        if (Jump)//cause the enemy to leap in a direction 
        {
            AttackFX.SetActive(false);
            Jump = false;
            rb.AddForce(Vector2.up * rb.mass * JumpHeight, ForceMode2D.Impulse);
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }
    public IEnumerator Movement()//primary loop for combat or jumping ability, waits based on lifetime and then sets its jump direction based on player location and random chance to move left or right
    {
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(lifetime * .75f);
                AttackFX.SetActive(true);
                yield return new WaitForSeconds(lifetime * .25f);
                    Jump = true;
                    if (PlayerControllerObject.transform.position.x < transform.position.x)
                    {
                        roll = Random.Range(1, 5);
                        if (roll == 4)
                        {
                            speed = Mathf.Abs(speed);
                        }
                        else
                        {
                            speed = -speed;
                        }
                    }
                    if (PlayerControllerObject.transform.position.x > transform.position.x)
                    {
                        roll = Random.Range(1, 5);
                        if (roll == 4)
                        {
                            speed = -speed;
                        }
                        else
                        {
                            speed = Mathf.Abs(speed);
                        }
                    }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")//touching enemies will either kill the enemy or itself based on the size of the two, start a UpdateSize method if it lives
        {
            GameObject EnemyHit = other.gameObject;
            EnemyAI Encounter = EnemyHit.GetComponent<EnemyAI>();
            if (Encounter.Power < Power)
            {
                Destroy(other.gameObject);
                Size = Size * 1.05f;
                UpdateSize();
                Instantiate(FXAbosrbOBJ, FXAbsorb);
            }
        }
        if (other.tag == "Player")//touching the player can kill the player or itself based on size, starts a UpdateSize function if it lives
        {
            if (playercontroller.Power * playercontroller.Crush < Power)
            {
                Instantiate(FXAbosrbOBJ, FXAbsorb);
                playercontroller.Dead = true;
                Size += playercontroller.Size / 2;
                UpdateSize();
            }
            else
            {
                Hits += 1;
                Dead();
            }
        }
        if (other.tag == "Killzone")
        {
            Destroy(this.gameObject);
        }
    }
    public void SetSize(float Player)//set size for the normal and attrition game modes, size is based on player size and difficulty
    {
        if (Player > 10)
        {
            Player = 9;
        }
        Size = Random.Range(Player / (2.2f + gameManager.MinUp), Player * (1.25f + gameManager.MaxUp));
    }
    public void SurvivialSize()//set size for surivial gamemode, size is based off of the gameControllers size value and the difficulty
    {
        GameObject SuriviveManageOBJ = GameObject.FindWithTag("GameController");
        Survivialmanager = SuriviveManageOBJ.GetComponent<SurvivialManager>();
        Size = Random.Range(Survivialmanager.Size / (2.2f + gameManager.MinUp), Survivialmanager.Size * (1.4f + gameManager.MaxUp));
    }
    public void UpdateSize()//upon "eating" something the size goes up by a small value
    {
        float Radius;
        Radius = Size / 2;
        Power = Mathf.PI * Mathf.Pow(Radius, 2) / 2;
        transform.localScale = new Vector2(Size, Size);
    }
    public void Dead()//if it was "eaten" by the player then it plays a death animation and makes the player larger
    {
        if (Hits == 1)
        {
            GameObject Death = Instantiate(DeathFX, FXAbsorb.position, FXAbsorb.rotation);
            Death.transform.localScale = new Vector2(Size, Size);
            playercontroller.SizedUp = true;
            playercontroller.UpdateSize(Health);
            playercontroller.SpawnAbsorbFX();
            playercontroller.Kills += 1;
            statManager.Kills += 1;
            if (playercontroller.Crush > 1)
            {
                statManager.SlamKills += 1;
            }
            Destroy(this.gameObject);
        }
    }
}
