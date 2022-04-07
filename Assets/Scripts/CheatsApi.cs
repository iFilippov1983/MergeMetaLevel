
using Data;
using Features._Events;
using UnityEngine;

public class CheatsApi 
{
    private DynamicData _data;
    private RootEvents _events;

    public void SetCtx(CoreRoot root)
    {
        _data = root.Data;
        _events = root.Events;
    }
    
    public void ResetProgress()
    {
        _data.Profile = null;
        _events.App.OnDataSave = null;
        Application.Quit();
    }
}