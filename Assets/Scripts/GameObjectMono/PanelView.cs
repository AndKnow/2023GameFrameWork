using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC;
using UnityEngine.UI;

public class PanelView : UGUIBaseView<PanelModel>
{
    public override void RefreshView(PanelModel data)
    {
        GetViewComponent<Text>("CurValue").text = data.ModelData.ToString();
    }
}