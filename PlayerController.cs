using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Script acessesers
    private DeathMenu deathmenu;
    private GameManage gameManager;
    private TrophyPowers TPower;
    private StatsManager statManager;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D JumpCol, WalkCol, Bottom;
    public float speed = 9.5f, movement;
    public float JumpHeight, Crush;
    public bool Jump, Jumped, Landed, Falling, CanMove;
    public int JumpLimit, Jumps, Kills, DesObjs;
    public GameObject AltPlayer;

    public float Size, Power, TimeAlive, SizeUpValue = 1;
    public bool Dead, SizedUp;
    public GameObject AbsorbFXOBJ, CrushFX, LandFX, JumpFX, SpawnFX, DeathFX;
    public Transform AbosrbFXSpawn, LandFXSpawn;

    // Atrittion Values
    private float ATRWait = 10, ATRTimer = 0;
    public bool Full;


    // Start is called before the first frame update
    void Start()
    {
        GameObject GameMangerOBJ = GameObject.FindWithTag("GameManage");
        gameManager = GameMangerOBJ.GetComponent<GameManage>();
        TPower = GameMangerOBJ.GetComponent<TrophyPowers>();
        statManager = GameMangerOBJ.GetComponent<StatsManager>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GameObject DeathMenuObject = GameObject.FindWithTag("GameController");
        deathmenu = DeathMenuObject.GetComponent<DeathMenu>();
        SizeUpValue = SizeUpValue + TPower.Size;
        UpdateSize(0);
        JumpLimit = JumpLimit + TPower.Jumps;
        if (gameManager.Atr == 1)
        {
            Full = true;
            StartCoroutine(Decay());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanMove == true)
        {
            movement = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(movement * speed, rb.velocity.y);
        }

        if (Jump)
        {
            Jump = false;
            //rb.AddForce(Vector2.up * rb.mass * JumpHeight, ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
            Instantiate(JumpFX, AbosrbFXSpawn);
        }
        if (Dead)
        {
            Dead = false;
            YouDied();
        }
        //on in survivial
        if (gameManager.Survivial == 0 && Dead == false)
        {
            TimeAlive += Time.deltaTime;
        }
        //on in atrition
        if (gameManager.Atr == 1)
        {
            TimeAlive += Time.deltaTime;
            ATRTimer += Time.deltaTime;
            if (gameManager.Atr == 1 && ATRWait <= ATRTimer)
            {
                Full = false;
            }
        }
    }
    private void Update()
    {
        if (CanMove == true)
        {
            if (Input.GetButtonDown("Jump")&& Jumps > 0)
            {
                Jumped = true;
                Jump = true;
                Landed = false;
                Jumps -= 1;
                JumpCol.enabled = true;
                WalkCol.enabled = false;
                CrushFX.SetActive(false);
                Crush = 1;
            }
            if (Input.GetButtonDown("Down") && Landed == false)
            {
                Crush = 1.33f + TPower.Crush;
                CrushFX.SetActive(true);
                rb.velocity = new Vector2(rb.velocity.x, -JumpHeight * 2);
            }
        }
    }
    public void UpdateSize(float Sustenance)
    {
        SizeUpValue += (.02f + Sustenance) * gameManager.Survivial;
        Size = Mathf.Sqrt(SizeUpValue);
        float Radius;
        Radius = Size / 2;
        Power = Mathf.PI * Mathf.Pow(Radius, 2) / 2;
        transform.localScale = new Vector2(Size, Size);
        JumpHeight += .01f;
        speed += .01f;
        deathmenu.Kills = Kills;
        deathmenu.Size = Size;
        if (gameManager.Atr == 1)
        {
            ATRWait = 8 - Mathf.Pow(Size, 1.5f);
            ATRTimer = 0;
            Full = true;
        }
    }
    public void SpawnAbsorbFX()
    {
        Instantiate(AbsorbFXOBJ, AbosrbFXSpawn);
    }
    public void SpawnSpawnFX()
    {
        Instantiate(SpawnFX, AbosrbFXSpawn);
    }
    public void YouDied()
    {
        if (gameManager.Survivial == 0)
        {
            deathmenu.Timed = true;
            deathmenu.TimeAlive = TimeAlive;
            statManager.CurrentTimeAlive = TimeAlive;
        }
        if (gameManager.Atr == 1)
        {
            deathmenu.Timed = true;
            deathmenu.TimeAlive = TimeAlive;
            statManager.CurrentARTTime = TimeAlive;
        }
        GameObject Death =  Instantiate(DeathFX, AbosrbFXSpawn.position, AbosrbFXSpawn.rotation);
        Death.transform.localScale = new Vector2(Size, Size);
        AltPlayer.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = .33f;
        statManager.Deaths += 1;
        statManager.SavePrefs();
        deathmenu.Dead = true;
    }

    // collision data
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.IsTouching(Bottom) && collision.tag == "Floor")
        {
            CrushFX.SetActive(false);
            Crush = 1;
            Landed = true;
            Falling = false;
            Jumps = JumpLimit;
            JumpCol.enabled = false;
            WalkCol.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor" ^ collision.tag == "Jumper")
        {
                Landed = false;
                Falling = true;
        }
    }

    //Atrition data
    public void SizeDown()
    {
        Size = Mathf.Sqrt(SizeUpValue) * gameManager.Survivial;
        Size = Size * .99f;
        float Radius;
        Radius = Size / 2;
        Power = Mathf.PI * Mathf.Pow(Radius, 2) / 2;
        GameObject Death = Instantiate(DeathFX, AbosrbFXSpawn.position, AbosrbFXSpawn.rotation);
        transform.localScale = new Vector2(Size, Size);
        if (Size < .1)
        {
            Dead = true;
        }
    }
    public IEnumerator Decay()
    {
        yield return new WaitForSeconds(13f);
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(.7f);
                if (Full == false)
                {
                    SizeUpValue -= .025f;
                    SizeDown();
                }
                else
                {
                    i -= 1;
                }
            }
        }
    }
}
