using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFarm.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;

        private Transform itemParent;


        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.AfterSceneloadedEvent += OnAfterSceneUnloadEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.AfterSceneloadedEvent -= OnAfterSceneUnloadEvent;
        }

        private void OnAfterSceneUnloadEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }


        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            // Debug.Log("触发了");
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }
    }
}