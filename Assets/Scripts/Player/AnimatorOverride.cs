using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animators;
    public SpriteRenderer holdItem;


    [Header("各部分动画列表")]
    public List<AnimatorType> animatorTypes;

    public Dictionary<string,Animator> animatorNameDict = new Dictionary<string, Animator>();

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();  //存储Player子物体身上的Body，Hair，Arm上的Animator

        foreach (var anim in animators)
        {
            animatorNameDict.Add(anim.name,anim); //将子物体的Animator做成字典
        }
    }

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
    }


    private void OnBeforeSceneUnloadEvent()
    {
        holdItem.enabled = false; //关闭举起物体的图片显示
        SwitchAnimator(PartType.None); //切换为不选择任何物品的动画
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        //WORKFLOW：不同的工具返回不同的动画在这里补全
        PartType currentType = itemDetails.itemType switch //根据被选中物品的信息判断物品类型（是否可被举起）
        {
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            _ => PartType.None
        };


        if(isSelected == false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            if(currentType == PartType.Carry) //物品可被举起就加载被举起的图片
            {
                holdItem.sprite = itemDetails.itemOnWorldSprite;
                holdItem.enabled = true;
            }
        }

        SwitchAnimator(currentType); //根据被选中的物品类别切换不同Arm动画
    }

    private void SwitchAnimator(PartType partType)
    {
        foreach (var item in animatorTypes) //在Inspector窗口中添加的动画列表中
        {
            if(item.partType == partType) //寻找对应类型的动画（Arm_Hold动画是Carry类型与Arm动画是None类型）
            {
                //用动画列表中存储的动画替换Player子物体对应部位正在运行的动画
                //Debug.Log(item.partName.ToString());
                animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
