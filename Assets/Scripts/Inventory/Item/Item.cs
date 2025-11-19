using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;

        private SpriteRenderer spriteRenderer; //子物体身上的SpriteRenderer
        public ItemDetails itemDetails;
        private BoxCollider2D coll;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (itemID != 0)
            {
                Init(itemID);
            }
        }


        /// <summary>
        /// 初始化：根据itemID让挂载Item的物品获得itemDetail，让该物体具有sprite并适配图片的碰撞体
        /// </summary>
        /// <param name="ID"></param>
        public void Init(int ID)
        {
            //itemID = ID;
            
            //Inventory获得当前数据
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

            if(itemDetails != null)
            {
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;

                //修改碰撞体尺寸
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x,spriteRenderer.sprite.bounds.size.y);
                coll.size = newSize;
                coll.offset = new Vector2(0,spriteRenderer.sprite.bounds.center.y);
            }
        }
    }
}