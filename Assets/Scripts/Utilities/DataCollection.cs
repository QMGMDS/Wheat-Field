using UnityEngine;

//定义一个类，是所有物品信息的模板
[System.Serializable] //序列化，让Unity保存里面的信息，哪怕在游戏结束运行，能在下次游戏开始显示上次游戏结束时的数据
public class ItemDetails //物品信息
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemDescription;
    public int itemUseRadius;
    public bool canPickedup;
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;
    [Range(0, 1)] //sellPercentage的范围
    public float sellPercentage;
}

[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}

[System.Serializable]
public class AnimatorType
{
    public PartType partType; //动画类型（举起，破坏）
    public PartName partName; //动画发起者（Arm，Hair）
    public AnimatorOverrideController overrideController;  //动画
}

[System.Serializable]
public class SerializableVector3 //场景物品的坐标
{
    public float x,y,z;

    public SerializableVector3(Vector3 pos) //序列化物品坐标
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    public Vector3 ToVector3() //读取之前物体坐标
    {
        return new Vector3(x,y,z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

[System.Serializable]
public class SceneItem
{
    public int itemID;
    public SerializableVector3 position;
}