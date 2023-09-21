using GameEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class IntIntParamTrigger
{
    public DicEventType type;
    public int param1;
    public int param2;
}
public class IntIntParamButton : InGamePacketButton
{
    public List<IntIntParamTrigger> triggers;

    public override void Run()
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            EventManager.Instance.TriggerEvent(triggers[i].type, triggers[i].param1, triggers[i].param2);
        }
    }
}
