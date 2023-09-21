using GameEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum LobbytType
public abstract class LobbyUI : MonoBehaviour
{
    public DicEventType eventType;
    public abstract bool HaveRunType();
}

public abstract class LobbyUIVoidF : LobbyUI
{
    public abstract void Run();
}
public abstract class LobbyUIHaveT : LobbyUI
{
    //must have own Listening code that using Event
    public abstract void Listening();
}
