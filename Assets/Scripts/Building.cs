using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for building on the map that hold a Resource inventory and that can be interacted with by Unit.
/// 用于在地图上构建资源生产点并可与 Unit 对象进行交互的基类。
/// This Base class handle modifying the inventory of resources.
/// 此基类处理修改资源生产点。
/// </summary>
//资源生成点基类
public abstract class Building : MonoBehaviour,UIMainScene.IUIInfoContent
{
    //need to be serializable for the save system, so maybe added the attribute just when doing the save system
    //需要对保存系统进行序列化，因此可能在执行保存系统时添加属性
    [System.Serializable]
    public class InventoryEntry
    {
        public string ResourceId;
        public int Count;
    }

    //[Tooltip("")]属性用于在编辑器中Inspetor面板鼠标悬停在属性上时显示注释
    [Tooltip("-1 is infinite")]
    public int InventorySpace = -1;
    
    protected List<InventoryEntry> m_Inventory = new List<InventoryEntry>();
    public List<InventoryEntry> Inventory => m_Inventory; //'=>' 表达式-bodied 语法，实现只读封装

    protected int m_CurrentAmount = 0;

    //return 0 if everything fit in the inventory, otherwise return the left over amount
    //如果所有东西都适合库存，则返回 0，否则返回剩余数量
    public int AddItem(string resourceId, int amount)
    {
        //as we use the shortcut -1 = infinite amount, we need to actually set it to max value for computation following
        //库存设为无限大时防止数据溢出
        int maxInventorySpace = InventorySpace == -1 ? Int32.MaxValue : InventorySpace;
        
        if (m_CurrentAmount == maxInventorySpace)
            return amount;

        int found = m_Inventory.FindIndex(item => item.ResourceId == resourceId);
        int addedAmount = Mathf.Min(maxInventorySpace - m_CurrentAmount, amount);

        //couldn't find an entry for that resource id so we add a new one.
        //找不到该资源id的条目，因此我们添加了一个新的条目。
        if (found == -1)
        {
            m_Inventory.Add(new InventoryEntry()
            {
                Count = addedAmount,
                ResourceId = resourceId
            });
        }
        else
        {
            m_Inventory[found].Count += addedAmount;
        }

        m_CurrentAmount += addedAmount;
        return amount - addedAmount;
    }

    //return how much was actually removed, will be 0 if couldn't get any.
    //返回实际删除的量，如果无法获得任何内容，则为 0。
    public int GetItem(string resourceId, int requestAmount)
    {
        int found = m_Inventory.FindIndex(item => item.ResourceId == resourceId);
        
        //couldn't find an entry for that resource id so we add a new one.
        if (found != -1)
        {
            int amount = Mathf.Min(requestAmount, m_Inventory[found].Count);
            m_Inventory[found].Count -= amount;

            if (m_Inventory[found].Count == 0)
            {//no more of that resources, so we remove it
                m_Inventory.RemoveAt(found);
            }

            m_CurrentAmount -= amount;

            return amount;
        }

        return 0;
    }

    public virtual string GetName()
    {
        return gameObject.name;
    }

    public virtual string GetData()
    {
        return "";
    }

    public void GetContent(ref List<InventoryEntry> content)
    {
        content.AddRange(m_Inventory);
    }
}
