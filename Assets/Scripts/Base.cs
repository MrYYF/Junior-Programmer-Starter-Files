using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A special building that hold a static reference so it can be found by other script easily (e.g. for Unit to go back to it)
/// 一个特殊的建筑，它有一个静态的引用，所以它可以很容易地被其他脚本找到（例如，让单元回到它）
/// </summary>
//卸货点
public class Base : Building
{ 
    public static Base Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
