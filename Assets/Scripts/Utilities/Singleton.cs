using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
//此处Singleton<T>中的<T>表示此时你要创建一个泛型类,<T>相当于泛型的标识，表明这个类或方法具有泛型特性
//where T : Singleton<T>是一个约束，表示我在调用这个类的时候传入的参数是Singleton<T>类型的
{
    private static T instance;

    public static T Instance
    {
        get => instance;
        // get { return instance; } 的简写形式
        //定义一个泛型属性，只允许被外部阅读，无法被修改
    }

    //访问修饰符 protected 表示仅允许被本类和子类中访问和修改
    // virtual 关键字让 Singleton<T> 的子类允许覆盖 Awake() 和 OnDestroy() 的函数方法
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    /// <summary>
    /// 单例的销毁
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

}
