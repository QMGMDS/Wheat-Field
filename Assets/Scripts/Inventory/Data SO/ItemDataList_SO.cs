using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//记录所有物品信息数据的列表容器
[CreateAssetMenu(fileName = "ItemDataList_SO", menuName = "Inventory/ItemDataList")]
public class ItemDataList_SO : ScriptableObject
{
    public List<ItemDetails> itemDetailsList; //定义一个列表（类似数组），里面存储的是ItemDetails类型的数据
}
