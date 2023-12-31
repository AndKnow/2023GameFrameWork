using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC;

public class PanelModel : UGUIBaseModel<PanelModel>, ICustomData
{
    protected int _modelData;
    public int ModelData
    {
        get => _modelData;
        set 
        {
            _modelData = value;
        }
    }

    public override void ConcreteUpdate()
    {
        _modelData += 100;
    }

    public override PanelModel InitData()
    {
        _modelData = 500;
        return this;
    }

    public override void SaveData()
    {
        if (_modelData % 200 == 0)
        {
            KeyValueDataManager.Instance.SetData("Password", this);
        }
    }
}
