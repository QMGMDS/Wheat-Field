using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Linq;
using UnityEngine.Assertions.Must;

public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase;
    private List<ItemDetails> itemList = new List<ItemDetails>();
    private VisualTreeAsset itemRowTemplate; //拿到列表物品的子模板

    private Sprite defaultIcon; //默认预览图片
    private ListView itemListView; //获得VisualElement
    private VisualElement iconPreview;
    private ScrollView itemDetailsSection;
    private ItemDetails activeItem;


    [MenuItem("STUDIO/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }


    public void CreateGUI() //当打开"STUDIO/ItemEditor"时执行
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        // VisualElement label = new Label("Hello World! From C#");
        // root.Add(label);

        // Instantiate UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        //拿到模板数据
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI builder/ItemRowTemplate.uxml"); //根据子模版的绝对路径拿到子模版

        //拿默认图片
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        //变量赋值
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView"); //拿到主的ItemEditor中在UIBuilder编辑窗口左侧的ListView
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");

        //获得按键
        root.Q<Button>("AddButton").clicked += OnAddButtonClicked;
        root.Q<Button>("DeleteButton").clicked += OnDeleteButtonClicked;

        //加载数据
        LoadDataBase();

        //生成ListView
        GenerateListView();
    }

    #region 按钮事件
    private void OnAddButtonClicked()
    {
        ItemDetails newItem = new ItemDetails();
        newItem.itemName = "NEW ITEM";
        newItem.itemID = 1000 + itemList.Count;
        itemList.Add(newItem);
        itemListView.Rebuild(); //更新左侧列表显示
    }

    private void OnDeleteButtonClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }
    #endregion

    /// <summary>
    /// 拿取数据库中的物品数据，赋值给itemList，此后itemList为物品数据列表
    /// </summary>
    private void LoadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDataList_SO"); //在Project窗口的文件中寻找名字为ItemDataList_SO的文件，返回一个字符串数组，里面存储每一个找到的文件的GUID（GUID相当于每个文件路径的内存地址）
        //var：让编译器根据赋值表达式自动推断变量的隐式类型
        //特点1：var变量必须在声明时赋值
        //特点2：一旦确定就不能改变
        if (dataArray.Length > 1) //条件成立表明找到了该文件
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]); //AssetDatabase.GUIDToAssetPath把第一个找到的ItemDataList_SO文件的GUID转化为该文件的绝对路径（相当于解引）
            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO)) as ItemDataList_SO; //AssetDatabase.LoadAssetAtPath加载path路径文件，加载的类型是typeof(ItemDataList_SO)，加载后可以拿dataBase来使用ItemDataList_SO中的属性和方法
            //as ItemDataList_SO即返回值强转成ItemDataList_SO类型，如果转换失败，返回 null
        }

        itemList = dataBase.itemDetailsList;
        //如果不标记则无法保存数据
        EditorUtility.SetDirty(dataBase); //让Unity保存数据的实时更改，在重启Unity后项目中的数据仍然被保存
    }

    /// <summary>
    /// 左侧列表初始化物品信息
    /// </summary>
    private void GenerateListView() //左侧列表ItemList
    {
        Func<VisualElement> makeItem = ()=> itemRowTemplate.CloneTree(); //将模板文件复制一份给makeItem，即Func<VisualElement> makeItem = ()=>这个动作表示创建一个子模版

        Action<VisualElement,int> bindItem = (e, i) => //e代指每一个子模版代称，i表示第i个创建出来的子模版，Action<VisualElement,int> bindItem这个动作表示为模板中的显示数据赋值
        {
            if (i < itemList.Count) //确保第i个物品在数据库总物品数量的范围内
            {
                if(itemList[i].itemIcon != null)
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture; //列表中新创建的物品的图片同步数据库中物品图片
                e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEM" : itemList[i].itemName;  //列表中新创建的物品的名字同步数据库中物品名字
            }
        };

        //上面一步只是在背后创建子模板数据并同步数据库，并没有在主的ItemEditor中列表上实现可视化
        //列表的可视化依赖于拿到的数据库列表itemList，也就是说，itemList中有几个ItemDetails,左侧列表中就可以可视化几个子模版并且每个子模版同步对应数据库中的物品信息
        //下面是实现可视化
        itemListView.fixedItemHeight = 60; //加大单个物品列表显示大小
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;

        itemListView.selectionChanged += OnListSelectionChange; //当itemListView中的物品被选择时触发OnListSelectionChange方法

        //未选择左侧列表物品右侧信息面板不可见
        itemDetailsSection.visible = false;
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItem) //激活被按下的物品
    {
        activeItem = (ItemDetails)selectedItem.First(); //拿到被选择物品的ItemDetails
        GetItemDetails();
        itemDetailsSection.visible = true;
    }

    private void GetItemDetails() //右侧加载物品详情ItemDetail
    {
        itemDetailsSection.MarkDirtyRepaint(); //让Unity保存数据的实时更改，在重启Unity后项目中的数据仍然被保存

        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt => //回调，每一次在ItemDetail中更改数据时发生数据库中数据的更新，在这里实现了在ItemDetail中更改ID，真实的数据库中物品ID也发生了更改
        {
            activeItem.itemID = evt.newValue; //每一次更改的数据以事件的方式存储，赋值给activeItem.itemID
        });

        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild(); //当在ItemDetails中修改了ItemName时，ItemListView中对应物品信息更新;
        });

        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType); //初始化枚举
        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType)evt.newValue;
        });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture; //左侧图片预览在点击时更新
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon; //注意：此处ObjectField的元素类型指的是UnityEditor.UIElement.ObjectField而不是UnityEngine.UIElement.ObjectField
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture; //当ItemDetails中ItemIcon被修改时，左侧预览中的图片也被修改
            itemListView.Rebuild(); //左侧列表中的图片也要被更改
        });

        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorldSprite;
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemOnWorldSprite = evt.newValue as Sprite;
        });

        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemUseRdius").value = activeItem.itemUseRadius;
        itemDetailsSection.Q<IntegerField>("ItemUseRdius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanPickedUp").value = activeItem.canPickedup;
        itemDetailsSection.Q<Toggle>("CanPickedUp").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickedup = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("Price").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });
    }

    
    
}