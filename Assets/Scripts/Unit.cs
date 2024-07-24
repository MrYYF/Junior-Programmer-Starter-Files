using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Base class for all Unit. It will handle movement order given through the UserControl script.
// ���� Unit �Ļ��ࡣ��������ͨ�� UserControl �ű��������ƶ�˳��
// It require a NavMeshAgent to navigate the scene.
// ����Ҫ NavMeshAgent ������������
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : MonoBehaviour,UIMainScene.IUIInfoContent
{
    public float Speed = 3;

    protected NavMeshAgent m_Agent;
    protected Building m_Target;

    protected void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.speed = Speed;
        m_Agent.acceleration = 999; //���ٶ�
        m_Agent.angularSpeed = 999; //�Ƕ��ٶ�
    }

    private void Start()
    {
        if (MainManager.Instance != null)
        {
            SetColor(MainManager.Instance.TeamColor);
        }
    }

    //�ı䵥λ��ɫ
    void SetColor(Color c)
    {
        var colorHandler = GetComponentInChildren<ColorHandler>();
        if (colorHandler != null)
        {
            colorHandler.SetColor(c);
        }
    }

    private void Update()
    {
        if (m_Target != null)
        {
            float distance = Vector3.Distance(m_Target.transform.position, transform.position);
            if (distance < 2.0f)
            {
                m_Agent.isStopped = true;
                BuildingInRange();
            }
        }
    }

    //�ⲿ���ã������ƶ��յ�
    public virtual void GoTo(Building target)
    {
        m_Target = target;

        if (m_Target != null)
        {
            m_Agent.SetDestination(m_Target.transform.position);
            m_Agent.isStopped = false;
        }
    }

    public virtual void GoTo(Vector3 position)
    {
        //we don't have a target anymore if we order to go to a random point.
        //�����������ȥһ������㣬���Ǿ�û��Ŀ���ˡ�
        m_Target = null;
        m_Agent.SetDestination(position); //SetDestination()���������Զ�Ѱ·
        m_Agent.isStopped = false;
    }


    /// <summary>
    /// Override this function to implement what should happen when in range of its target.
    /// ��д�˺�����ʵ������Ŀ�귶Χ��Ӧ�����������
    /// Note that this is called every frame the current target is in range, not only the first time we get in range! 
    /// ��ע�⣬�ⱻ��Ϊ��ǰĿ���ڷ�Χ�ڵ�ÿһ֡���������������ǵ�һ�ν��뷶Χ��
    /// </summary>
    protected abstract void BuildingInRange();

    //Implementing the IUIInfoContent interface so the UI know it should display the UI when this is clicked on.
    //Implementation of all the functions are empty as default, but they are set as virtual so subclass units can
    //override them to offer their own data to it.
    public virtual string GetName()
    {
        return "Unit";
    }

    public virtual string GetData()
    {
        return "";
    }

    public virtual void GetContent(ref List<Building.InventoryEntry> content)
    {
        
    }
}
