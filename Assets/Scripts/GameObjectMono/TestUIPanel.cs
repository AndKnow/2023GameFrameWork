using FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestUIPanel : UGUIPanelBaseController
{
    protected override void InitCallBack()
    {
        TestCustomEvent();
    } 

    public void TestCustomEvent()
    {
        var button1 = GetUIComponent<Button>("Button1");
        UGUIPanelManager.AddCustomEventCallback(button1, EventTriggerType.Drag, x =>
        {
            button1.transform.position = x.currentInputModule.input.mousePosition;
        });
    }

    public void OnClickButton1()
    {
        Debug.Log("OnClickButton1");
    }

    public void OnClickButton2()
    {
        Debug.Log("OnClickButton2");
    }

    public override void HandleButton(string name)
    {
        if (name == "Button1")
        {
            OnClickButton1();
        }
        else if (name == "Button2")
        {
            OnClickButton2();
        }
    }

}
