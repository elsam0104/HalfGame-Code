using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using PROTO;

public class IntParamButton : InGamePacketButton
{
    public List<DicEventType> type;
    public List<int> value;
    public override void Run()
    {
        Debug.Log("dafdsafdsaf");
        for (int i = 0; i < type.Count; i++)
        {
            Debug.Log("dafdsafdsa"+i);
            EventManager.Instance.TriggerEvent(type[i], value[i]);
          
        }
    }
}
