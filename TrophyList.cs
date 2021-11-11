using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyList : MonoBehaviour//used to present and show info for various in game trophies and the powers they provide
{
    //Holder for powers
    public GameObject Clouds, Slams, Survived, WorldEat, PaulKill, ARTTime;
    //holder for trophy blackouts
    public GameObject CloudBG, SlamBG, SurvivedBG, WorldEatBG, PaulKillBG, ARTTimeBG;
    //data from save
    public int CloudsEaten, SlamKills, WorldsEaten, Kills, Deaths, PaulsKilled;
    public float MaxTimeAlive, MaxARTTime;
    //data for how much each trophy needs
    public int CloudsNeed, SlamNeed, WorldNeed, PaulNeed;
    public float SurviveNeed, ARTNeed;
    //text for stats menu
    public Text KillsTex, DeathTex, MaxTimeTex, LargestSize, SlamKillsTex;
    //text for trophy menu
    public Text CloudTex, SlamsTex, SurvivedTex, WorldEatTex, PaulKillTex, ARTTimeTex;
    public void LoadText()//this would show the data for all trophies and tell the player how much had to be done for each to unlock it
    {
        KillsTex.text = "Kills: " + Kills;
        SlamKillsTex.text = "Slam kills: " + SlamKills;
        DeathTex.text = "Deaths: " + Deaths;
        MaxTimeTex.text = "Longest time survived: " + MaxTimeAlive;

        CloudTex.text = "Consumed " + CloudsEaten + "/" + CloudsNeed + " clouds";
        SlamsTex.text = "Slammed " + SlamKills + "/" + SlamNeed + " edibles";
        SurvivedTex.text = "Survived " + MaxTimeAlive + "s/" + SurviveNeed + "s in Survival";
        WorldEatTex.text = "Consumed " + WorldsEaten + "/" + WorldNeed + " Worlds";
        PaulKillTex.text = "Consumed " + PaulsKilled + "/" + PaulNeed + " Pauls";
        ARTTimeTex.text = "Survived " + MaxARTTime + "s/" + ARTNeed + "s in Atrition";
        //if a trophy was complete then the respective power it provided would become visible, powers would give stats such as starting size or jump count
        if (CloudsEaten >= CloudsNeed)
        {
            CloudBG.SetActive(false);
            Clouds.SetActive(true);
        }
        if (SlamKills >= SlamNeed)
        {
            SlamBG.SetActive(false);
            Slams.SetActive(true);
        }
        if (MaxTimeAlive >= SurviveNeed)
        {
            SurvivedBG.SetActive(false);
            Survived.SetActive(true);
        }
        if (WorldsEaten >= WorldNeed)
        {
            WorldEatBG.SetActive(false);
            WorldEat.SetActive(true);
        }
        if (PaulsKilled >= PaulNeed)
        {
            PaulKillBG.SetActive(false);
            PaulKill.SetActive(true);
        }
        if (MaxARTTime >= ARTNeed)
        {
            ARTTimeBG.SetActive(false);
            ARTTime.SetActive(true);
        }
    }
}
