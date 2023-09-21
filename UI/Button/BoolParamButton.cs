using GameEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolParamButton : InGamePacketButton
{
    public List<DicEventType> type;
    public List<bool> values;
    public override void Run()
    {
        for (int i = 0; i < type.Count; i++)
        {
            EventManager.Instance.TriggerEvent(type[i], values[i]);
        }
    }
}
