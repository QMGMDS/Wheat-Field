using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Text valueText;
    [SerializeField] private GameObject bottomPart;



    /// <summary>
    /// ToolTip的UI与数据同步
    /// </summary>
    /// <param name="itemDetails"></param>
    /// <param name="slotType"></param>
    public  void SetupToolTip(ItemDetails itemDetails, SlotType slotType)
    {
        nameText.text = itemDetails.itemName;

        typeText.text = GetItemType(itemDetails.itemType);

        descriptionText.text = itemDetails.itemDescription;

        //显示售卖价格
        if(itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            bottomPart.SetActive(true);

            var price = itemDetails.itemPrice;
            if(slotType == SlotType.Bag)
            {
                price = (int)(price * itemDetails.sellPercentage);
            }
            valueText.text = price.ToString();
        }
        else
        {
            bottomPart.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()); //即时渲染物品详情显示框
    }

    /// <summary>
    /// 物品详情的类型显示中文
    /// </summary>
    /// <param name="itemType">被选中的物品类型</param>
    /// <returns></returns>
    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.Commodity => "商品",
            ItemType.Furniture => "家具",
            ItemType.BreakTool => "工具",
            ItemType.HoeTool => "工具",
            ItemType.ChopTool => "工具",
            ItemType.ReapTool => "工具",
            ItemType.WaterTool => "工具",
            ItemType.CollectTool => "工具",
            _ => "无"
        };
    }
}
