using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Subclass of Unit that will transport resource from a Resource Pile back to Base.
/// </summary>
public class TransporterUnit : Unit
{
    public int MaxAmountTransported = 1;

    private Building m_CurrentTransportTarget; //当前运输的资源点
    private Building.InventoryEntry m_Transporting = new Building.InventoryEntry();

    // We override the GoTo function to remove the current transport target, as any go to order will cancel the transport
    // 重写基类GoTo函数，使其可以通过自由移动来取消运输指令
    public override void GoTo(Vector3 position)
    {
        base.GoTo(position);
        m_CurrentTransportTarget = null;
    }
    
    
    protected override void BuildingInRange()
    {

        if (m_Target == Base.Instance)
        {
            //we arrive at the base, unload!
            //到达卸货点，叉车上有货物
            if (m_Transporting.Count > 0)
                m_Target.AddItem(m_Transporting.ResourceId, m_Transporting.Count);

            //we go back to the building we came from
            //返回装载点/资源生成点
            GoTo(m_CurrentTransportTarget);
            m_Transporting.Count = 0;
            m_Transporting.ResourceId = "";
        }
        else
        {
            if (m_Target.Inventory.Count > 0)
            {
                m_Transporting.ResourceId = m_Target.Inventory[0].ResourceId;
                m_Transporting.Count = m_Target.GetItem(m_Transporting.ResourceId, MaxAmountTransported);
                m_CurrentTransportTarget = m_Target;
                GoTo(Base.Instance);
            }
        }
    }
    
    //Override all the UI function to give a new name and display what it is currently transporting
    public override string GetName()
    {
        return "Transporter";
    }

    public override string GetData()
    {
        return $"Can transport up to {MaxAmountTransported}";
    }

    public override void GetContent(ref List<Building.InventoryEntry> content)
    {
        if (m_Transporting.Count > 0)
            content.Add(m_Transporting);
    }
}
