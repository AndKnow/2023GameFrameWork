using FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIPanel : UGUIPanelBaseController
{
    protected override void InitCallBack()
    {
        GetUIComponent<Button>("Button1").onClick.AddListener(OnClickButton1);
        GetUIComponent<Button>("Button2").onClick.AddListener(OnClickButton2);
    } 

    public void OnClickButton1()
    {
        Debug.Log("OnClickButton1");
    }

    public void OnClickButton2()
    {
        Debug.Log("OnClickButton2");
    }
    
}
