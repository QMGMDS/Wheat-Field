using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameSecond,gameMinute,gameHour,gameDay,gameMonth,gameYear;
    private Season gameSeason = Season.春天;
    private int monthInSeason = 3;

    public bool gameClockPause; //时间是否被暂停

    private float tikTime;


    private void Awake()
    {
        NewGameTime();
    }

    private void Start()
    {
        EventHandler.CallGameMinuteEvent(gameMinute,gameHour);
        EventHandler.CallGameDataEvent(gameHour,gameDay,gameMonth,gameYear,gameSeason);
    }

    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;

            if(tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }
    }


    /// <summary>
    /// 初始化游戏时间
    /// </summary>
    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2025;
        gameSeason = Season.春天;
    }

    /// <summary>
    /// 游戏时间更新
    /// </summary>
    private void UpdateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;
            
            if(gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if(gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if(gameDay > Settings.dayHold)
                    {
                        gameMonth++;
                        gameDay = 1;

                        if(gameMonth > 12)
                            gameMonth = 1;
                        
                        monthInSeason--; //一个季节三个月
                        if(monthInSeason == 0) //过了三个月后
                        {
                            monthInSeason = 3;

                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
                            if(seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }

                            gameSeason = (Season)seasonNumber;
                        }
                    }
                }
                EventHandler.CallGameDataEvent(gameHour,gameDay,gameMonth,gameYear,gameSeason);
            }
        }

        //Debug.Log("Second: " + gameSecond + "Minute: " + gameMinute);
        EventHandler.CallGameMinuteEvent(gameMinute,gameHour);
    }
}
