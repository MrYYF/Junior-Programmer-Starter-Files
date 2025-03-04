using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A subclass of Building that produce resource at a constant rate.
/// </summary>
public class ResourcePile : Building
{
    public ResourceItem Item;

    private float m_ProductionSpeed = 0.5f; //资源生产速度
    public float ProductionSpeed
    {
        get { return m_ProductionSpeed; }
        set
        {
            if (value < 0.0f)
            {
                Debug.LogError("You can't set a negative production speed!");
            } else
            {
                m_ProductionSpeed = value; // original setter now in if/else statement
            }
        }

    }

    private float m_CurrentProduction = 0.0f; //当前资源数量

    private void Update()
    {
        //如果已有资源数量
        if (m_CurrentProduction > 1.0f)
        {
            int amountToAdd = Mathf.FloorToInt(m_CurrentProduction); //取正
            int leftOver = AddItem(Item.Id, amountToAdd); //添加/减少对应id的资源数量

            m_CurrentProduction = m_CurrentProduction - amountToAdd + leftOver;
        }
        
        if (m_CurrentProduction < 1.0f)
        {
            m_CurrentProduction += m_ProductionSpeed * Time.deltaTime;
        }
    }

    public override string GetData()
    {
        return $"Producing at the speed of {m_ProductionSpeed}/s";
    }
    
}
