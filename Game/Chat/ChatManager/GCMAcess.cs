using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCMAcess : MonoSingleTon<GCMAcess>
{
    [SerializeField]
    private GameChatManager chatManager;
    public GameChatManager ChatManager => chatManager;
}
