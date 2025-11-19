using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MFarm.Inventory
{
    //该特性明确表示这个脚本依赖于SlotUI组件才能正常工作
    //如果GameObject上没有SlotUI组件，Unity会自动添加一个
    //如果尝试删除SlotUI组件，Unity会阻止这个操作并显示警告
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SlotUI slotUI;
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();



        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }


        //鼠标停留显示详情
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(slotUI.itemAmount != 0)
            {
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                inventoryUI.itemToolTip.SetupToolTip(slotUI.itemDetails, slotUI.slotType);

                inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0);
                inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 60; //后面的transform.position指挂载脚本的对象身上的transform.position，即Slot的
            }
            else
            {
                inventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }

    }

}

