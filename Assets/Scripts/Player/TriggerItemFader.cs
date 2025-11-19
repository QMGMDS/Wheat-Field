using UnityEngine;

//挂载在Player身上
//碰到带有触发器的物体实现物体半透明
public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();

        if (faders.Length > 0)
        {
            foreach (var item in faders) //item相当于faders数组中遍历到的每一个变量通称
            {
                item.FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();

        if (faders.Length > 0)
        {
            foreach (var item in faders) //item相当于faders数组中遍历到的每一个变量通称
            {
                item.FadeIn();
            }
        }
    }
}
