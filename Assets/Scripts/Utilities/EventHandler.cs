using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;

    /// <summary>
    /// 更新容器物品数据
    /// </summary>
    /// <param name="location">要更新哪个地方的UI</param>
    /// <param name="list">要更新的数据</param>
    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location, list);
        //？.简写
        //完整写法：
        //if (UpdateInventoryUI != null){ UpdateInventoryUI.Invoke(location, list); }
    }

    public static event Action<int, Vector3> InstantiateItemInScene;

    /// <summary>
    /// 丢物品到场景中
    /// </summary>
    /// <param name="ID">要丢弃的物品ID</param>
    /// <param name="pos">要丢弃的目标位置</param>
    public static void CallInstantiateItemInScene(int ID, Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID, pos);
    }


    
    public static event Action<ItemDetails,bool> ItemSelectedEvent;

    /// <summary>
    /// 选中物品的事件
    /// </summary>
    /// <param name="itemDetails">被选中的物品信息</param>
    /// <param name="isSelected">物品是否被选中</param>
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails,isSelected);
    }


    public static event Action<int, int> GameMinuteEvent;

    /// <summary>
    /// 更新小时间的UI事件
    /// </summary>
    /// <param name="minute"></param>
    /// <param name="hour"></param>
    public static void CallGameMinuteEvent(int minute, int hour)
    {
        GameMinuteEvent?.Invoke(minute, hour);
    }


    public static event Action<int, int, int, int, Season> GameDataEvent;

    /// <summary>
    /// 更新大时间的UI事件
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="day"></param>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <param name="season"></param>
    public static void CallGameDataEvent(int hour, int day, int month, int year, Season season)
    {
        GameDataEvent?.Invoke(hour, day, month, year, season);
    }


   
    public static event Action<string, Vector3> TransitionEvent;

    /// <summary>
    /// 场景切换事件
    /// </summary>
    /// <param name="sceneName">目标场景名字</param>
    /// <param name="pos">目的坐标</param>
    public static void CallTransitionEvent(string sceneName, Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName,pos);
    }



    public static event Action BeforeSceneUnloadEvent;

    //场景卸载之后的事件
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }


    public static event Action AfterSceneloadedEvent;

    //场景切换之后的事件
    public static void CallAfterSceneloadedEvent()
    {
        AfterSceneloadedEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;

    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }

}
