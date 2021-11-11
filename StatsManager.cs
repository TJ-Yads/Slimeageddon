using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour//holds and saves the data for various stats such as kills and deaths, along with passing data for in game trophies
{
    public int Kills, Deaths, WorldsEaten, CloudsEaten, SlamKills, PaulsKilled, LargestSize;
    public float MaxTimeAlive, CurrentTimeAlive, CurrentARTTime, MaxARTTime;

    private TrophyList Stats;
    void Start()
    {
        LoadPrefs();
    }
    public void SavePrefs()//happens after a game is finished and will add to various counters, will also check the players best time values for survival and attrition if those modes are active
    {
        if (CurrentTimeAlive > MaxTimeAlive)
        {
            MaxTimeAlive = CurrentTimeAlive;
            MaxTimeAlive = Mathf.Round(MaxTimeAlive * 100f) / 100f;
        }
        if (CurrentARTTime > MaxARTTime)
        {
            MaxARTTime = CurrentARTTime;
            MaxARTTime = Mathf.Round(MaxARTTime * 100f) / 100f;
        }
        PlayerPrefs.SetInt("Kills", Kills);
        PlayerPrefs.SetInt("SlamKills", SlamKills);
        PlayerPrefs.SetInt("Deaths", Deaths);
        PlayerPrefs.SetInt("WorldsEaten", WorldsEaten);
        PlayerPrefs.SetInt("CloudsEaten", CloudsEaten);
        PlayerPrefs.SetInt("PaulsKilled", PaulsKilled);
        PlayerPrefs.SetFloat("MaxTimeAlive", MaxTimeAlive);
        PlayerPrefs.SetFloat("MaxARTTime", MaxARTTime);

        PlayerPrefs.Save();
    }

    public void LoadPrefs()//on start the will load the players save data and pass it on to the main menu and trophies
    {
        GameObject statControllerObject = GameObject.FindWithTag("MenuController");
        Stats = statControllerObject.GetComponent<TrophyList>();
        Stats.SlamKills = PlayerPrefs.GetInt("SlamKills");
        Stats.Kills = PlayerPrefs.GetInt("Kills");
        Stats.Deaths = PlayerPrefs.GetInt("Deaths");
        Stats.WorldsEaten = PlayerPrefs.GetInt("WorldsEaten");
        Stats.CloudsEaten = PlayerPrefs.GetInt("CloudsEaten");
        Stats.PaulsKilled = PlayerPrefs.GetInt("PaulsKilled");
        Stats.MaxTimeAlive = PlayerPrefs.GetFloat("MaxTimeAlive");
        SlamKills = PlayerPrefs.GetInt("SlamKills");
        Kills = PlayerPrefs.GetInt("Kills");
        Deaths = PlayerPrefs.GetInt("Deaths");
        WorldsEaten = PlayerPrefs.GetInt("WorldsEaten");
        CloudsEaten = PlayerPrefs.GetInt("CloudsEaten");
        PaulsKilled = PlayerPrefs.GetInt("PaulsKilled");
        MaxTimeAlive = PlayerPrefs.GetFloat("MaxTimeAlive");
        MaxARTTime = PlayerPrefs.GetFloat("MaxARTTime");
        Stats.LoadText();
    }
}
