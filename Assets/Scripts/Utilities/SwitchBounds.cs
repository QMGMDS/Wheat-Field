using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//挂载在Virtual Camera身上
//获取场景中摄像机的边界
public class SwitchBounds : MonoBehaviour
{

    private void OnEnable()
    {
        EventHandler.AfterSceneloadedEvent += SwitchConfinerShape;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneloadedEvent -= SwitchConfinerShape;
    }


    private void SwitchConfinerShape()
    {
        // Debug.Log("触发了");
        PolygonCollider2D ConfinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = ConfinerShape;

        confiner.InvalidatePathCache(); //每一次获取边界之后要清除缓存确保切换场景时下一次成功获取边界
    }
}
