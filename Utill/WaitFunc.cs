using GameEvent;
using GameEvent.UI;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaitFuncType
{
    None,
    StartGame,
}
public class WaitFunc : MonoSingleTon<WaitFunc>
{
    Dictionary<WaitFuncType, List<Action>> waitFuncs = new Dictionary<WaitFuncType, List<Action>>();

    public void TriggerEvent(WaitFuncType type)
    {
        if(waitFuncs.ContainsKey(type))
        {
            waitFuncs[type].ForEach(t => { t.Invoke(); });
        }
    }
    public void InitAction(WaitFuncType type, Action action)
    {
        List<Action> thisAction = new List<Action>();
        if (waitFuncs.TryGetValue(type, out thisAction))
        {
            waitFuncs[type].Add(action);
        }
        else
        {
            thisAction.Add(action);
            waitFuncs.Add(type, thisAction);
        }
    }
}
