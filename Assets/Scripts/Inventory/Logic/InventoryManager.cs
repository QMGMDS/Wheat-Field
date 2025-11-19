using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory //要使用InventoryManager里面的方法需要引用该命名空间
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;

        [Header("背包数据")]
        public InventoryBag_SO playerBag;


        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }


        /// <summary>
        /// 通过ID返回物品信息
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            //在itemDetailsList中寻找一个itemDetails
            //如果找到的itemDetails.itemID等于传入的ID，返回对应的itemDetails
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// 添加物品到Player的背包里
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">是否要销毁物品</param>
        public void AddItem(Item item,bool toDestory)
        {
            //背包是否已经有该物品
            var index = GetItemIndexInBag(item.itemID);
            
            AddItemAtIndex(item.itemID, index, 1);

            Debug.Log(item.itemID + "Name" + GetItemDetails(item.itemID).itemName);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }

            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }



        /// <summary>
        /// 检查背包里是否有空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 通过物品ID查找背包该物品所处位置并返回位置序号
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <returns>-1则背包无该物体</returns>
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 在指定背包位置添加物品
        /// </summary>
        /// <param name="ID">被添加物品的ID</param>
        /// <param name="index">被添加物品应添加位置的序号</param>
        /// <param name="amount">被添加物品的数量</param>
        private void AddItemAtIndex(int ID,int index,int amount)
        {
            if(index == -1 && CheckBagCapacity()) //背包里没有该物品 同时背包有空位
            {
                var item = new InventoryItem{ itemID = ID , itemAmount = amount };
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else //背包有这个物品
            {
                int currentAmount = playerBag.itemList[index].itemAmount + amount;
                var item = new InventoryItem{ itemID = ID , itemAmount = currentAmount };

                playerBag.itemList[index] = item;
            }
        }

        /// <summary>
        /// Player背包范围内交换物品
        /// </summary>
        /// <param name="fromIndex">交换的起始物品序号</param>
        /// <param name="targetIndex">交换的目标物品序号</param>
        public void SwapItem(int fromIndex, int targetIndex)
        {
            InventoryItem currentItem = playerBag.itemList[fromIndex];
            InventoryItem targetItem = playerBag.itemList[targetIndex];

            //背包中的物品交换
            if(targetItem.itemID != 0)
            {
                playerBag.itemList[targetIndex] = currentItem;
                playerBag.itemList[fromIndex] = targetItem;
            }
            else
            {
                playerBag.itemList[targetIndex] = currentItem;
                playerBag.itemList[fromIndex] = new InventoryItem();
            }
            //交换物品后更新背包数据
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
    }
}

