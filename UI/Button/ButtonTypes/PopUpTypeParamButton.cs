using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using PROTO;

public class PopUpTypeParamButton : InGamePacketButton
{
    public List<DicEventType> type;
    public List<LobbyPopUpType> value;
    public override void Run()
    {
        for (int i = 0; i < type.Count; i++)
        {
            EventManager.Instance.TriggerEvent(type[i], value[i]);
        }
    }
}