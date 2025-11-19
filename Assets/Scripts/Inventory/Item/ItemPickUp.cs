using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载Player身上
//让Player具有拾取物品的能力
namespace MFarm.Inventory
{
    public class ItemPickUp : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();

            if(item != null)
            {
                if (item.itemDetails.canPickedup)
                {
                    //拾取物品添加到背包
                    InventoryManager.Instance.AddItem(item,true);
                }
            }
        }
    }
}
