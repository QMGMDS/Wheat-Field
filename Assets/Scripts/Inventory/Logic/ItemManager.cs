using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFarm.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;

        private Transform itemParent;

        //记录场景Item
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();


        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneloadedEvent += OnAfterSceneUnloadEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneloadedEvent -= OnAfterSceneUnloadEvent;
        }

        private void OnBeforeSceneUnloadEvent()
        {
            //GetAllSceneItems();
        }

        private void OnAfterSceneUnloadEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            //ReCreateAllItems();
        }


        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            // Debug.Log("触发了");
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }


        /// <summary>
        /// 获得当前场景中所有Item物品数据（坐标和ID）
        /// </summary>
        private void GetAllSceneItems()
        {
            #region 查找已加载场景中携带Item的对象,存入列表中
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };

                currentSceneItems.Add(sceneItem);
                //Debug.Log(sceneItem.itemID);
            }
            #endregion

            #region 将列表存入字典中
            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name)) //如果当前加载的场景曾经加载过，则更新之前的场景物品数据
            {
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else //如果是新场景，添加一个场景物品数据
            {
                sceneItemDict.Add(SceneManager.GetActiveScene().name,currentSceneItems);
            }
            #endregion

        }

        /// <summary>
        /// 刷新重建当前场景中的物品
        /// </summary>
        private void ReCreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

             //字典查找当前激活的场景，TryGetValu()使得如果没有找到则返回flase,有则返回true并将key对应数据赋值给currentSceneItem
            if(sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {

                if(currentSceneItems != null) //找到的数据不为空
                {
                    //清场
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItems)
                    {
                        var newItem = Instantiate(itemPrefab,item.position.ToVector3(),Quaternion.identity,itemParent);
                        //newItem.Init(item.itemID);
                    }
                }
            }
        }
    }
}