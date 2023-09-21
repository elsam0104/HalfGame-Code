using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class NoneParamButton : InGamePacketButton
{
    public List<DicEventType> type;
    public override void Run()
    {
        Debug.Log(name);
        for (int i = 0; i < type.Count; i++)
        {
            EventManager.Instance.TriggerEvent(type[i]);
        }
    }
}
