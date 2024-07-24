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

    public float ProductionSpeed = 0.5f; //��Դ�����ٶ�

    private float m_CurrentProduction = 0.0f; //��ǰ��Դ����

    private void Update()
    {
        //���������Դ����
        if (m_CurrentProduction > 1.0f)
        {
            int amountToAdd = Mathf.FloorToInt(m_CurrentProduction); //ȡ��
            int leftOver = AddItem(Item.Id, amountToAdd); //���/���ٶ�Ӧid����Դ����

            m_CurrentProduction = m_CurrentProduction - amountToAdd + leftOver;
        }
        
        if (m_CurrentProduction < 1.0f)
        {
            m_CurrentProduction += ProductionSpeed * Time.deltaTime;
        }
    }

    public override string GetData()
    {
        return $"Producing at the speed of {ProductionSpeed}/s";
    }
    
}
