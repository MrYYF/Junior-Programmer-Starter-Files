using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Base class for all Unit. It will handle movement order given through the UserControl script.
// 所有 Unit 的基类。它将处理通过 UserControl 脚本给出的移动顺序。
// It require a NavMeshAgent to navigate the scene.
// 它需要 NavMeshAgent 来导航场景。
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
        m_Agent.acceleration = 999; //加速度
        m_Agent.angularSpeed = 999; //角度速度
    }

    private void Start()
    {
        if (MainManager.Instance != null)
        {
            SetColor(MainManager.Instance.TeamColor);
        }
    }

    //改变单位颜色
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

    //外部调用，设置移动终点
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
        //如果我们下令去一个随机点，我们就没有目标了。
        m_Target = null;
        m_Agent.SetDestination(position); //SetDestination()函数用于自动寻路
        m_Agent.isStopped = false;
    }


    /// <summary>
    /// Override this function to implement what should happen when in range of its target.
    /// 重写此函数以实现在其目标范围内应发生的情况。
    /// Note that this is called every frame the current target is in range, not only the first time we get in range! 
    /// 请注意，这被称为当前目标在范围内的每一帧，而不仅仅是我们第一次进入范围！
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
