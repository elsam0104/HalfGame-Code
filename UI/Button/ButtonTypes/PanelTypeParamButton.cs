using GameEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTypeParamButton : InGamePacketButton
{
    public List<DicEventType> type;
    public List<PanelType> value;
    public override void Run()
    {
        for (int i = 0; i < type.Count; i++)
        {
            EventManager.Instance.TriggerEvent(type[i], value[i]);
        }
    }
}
