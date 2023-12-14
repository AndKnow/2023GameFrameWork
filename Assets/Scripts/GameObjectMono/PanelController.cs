using MVC;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : UGUIBaseController<PanelModel>
{
    protected override void HandleControls()
    {
        throw new NotImplementedException();
    }

    protected override void InitViewCallback()
    {
        _safeView.GetViewComponent<Button>("Add").onClick.AddListener(() =>
        {
            _data.UpdateData();
        });
    }

    protected override void RefreshView(PanelModel data)
    {
        base.RefreshView(data);
        Debug.Log("非法访问" + _data.ModelData);
    }
}