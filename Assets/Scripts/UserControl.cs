using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handle all the control code, so detecting when the users click on a unit or building and selecting those
/// If a unit is selected it will give the order to go to the clicked point or building when right clicking.
/// </summary>
public class UserControl : MonoBehaviour
{
    public Camera GameCamera;
    public float PanSpeed = 10.0f;
    public GameObject Marker;
    
    private Unit m_Selected = null;

    private void Start()
    {
        Marker.SetActive(false);
    }

    //处理鼠标选择
    public void HandleSelection()
    {
        var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //the collider could be children of the unit, so we make sure to check in the parent
            //射线检测可能是unit的子类，所以检测时要保证父类是Unit
            var unit = hit.collider.GetComponentInParent<Unit>();
            m_Selected = unit;


            //check if the hit object have a IUIInfoContent to display in the UI
            //检查射线检测对象是否具有要在 UI 中显示的 IUIInfoContent
            //if there is none, this will be null, so this will hid the panel if it was displayed
            //如果没有，说明射线未检测到，所以要将选中后的面板隐藏
            var uiInfo = hit.collider.GetComponentInParent<UIMainScene.IUIInfoContent>();
            UIMainScene.Instance.SetNewInfoContent(uiInfo);
        }
    }

    //处理移动
    public void HandleAction()
    {
        //right click give order to the unit
        //实现选中单位后鼠标右键移动
        var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var building = hit.collider.GetComponentInParent<Building>();

            if (building != null)
            {
                m_Selected.GoTo(building);
            } else
            {
                m_Selected.GoTo(hit.point);
            }
        }
    }

    private void Update()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        GameCamera.transform.position = GameCamera.transform.position + new Vector3(move.y, 0, -move.x) * PanSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
        else if (m_Selected != null && Input.GetMouseButtonDown(1))
        {
            HandleAction();
        }

        MarkerHandling();
    }

    // Handle displaying the marker above the unit that is currently selected (or hiding it if no unit is selected)
    // 处理在当前所选单位上方显示标记（或在未选择单位时将其隐藏）
    void MarkerHandling()
    {
        //activeInHierarchy表示该游戏对象及其所有父对象是否都是激活状态
        if (m_Selected == null && Marker.activeInHierarchy)
        {
            Marker.SetActive(false);
            Marker.transform.SetParent(null);
        }
        else if (m_Selected != null && Marker.transform.parent != m_Selected.transform)
        {
            Marker.SetActive(true);
            Marker.transform.SetParent(m_Selected.transform, false);
            Marker.transform.localPosition = Vector3.zero;
        }    
    }
}
