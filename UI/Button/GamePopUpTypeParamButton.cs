using GameEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DicEventAndGamePopUp
{
    public DicEventType type;
    public GamePopUpType value;
}
public class GamePopUpTypeParamButton : BaseButton
{
    
    public DicEventAndGamePopUp[] dicEventAndGamePopUp;
    public override void Run()
    {
        Debug.LogWarning(dicEventAndGamePopUp.Length);
        foreach(var item in dicEventAndGamePopUp)
        {
            EventManager.Instance.TriggerEvent(item.type, item.value);
        }
    }
}
