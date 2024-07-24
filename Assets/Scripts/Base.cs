using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A special building that hold a static reference so it can be found by other script easily (e.g. for Unit to go back to it)
/// һ������Ľ���������һ����̬�����ã����������Ժ����׵ر������ű��ҵ������磬�õ�Ԫ�ص�����
/// </summary>
//ж����
public class Base : Building
{ 
    public static Base Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
