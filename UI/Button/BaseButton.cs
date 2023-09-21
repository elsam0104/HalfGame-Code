using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEvent;
using PROTO;
using Google.Protobuf.Collections;

[RequireComponent(typeof(Button))]
public abstract class BaseButton : MonoBehaviour
{
    protected virtual void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Run);
    }
    public abstract void Run();
}
public abstract class PacketButton : BaseButton
{
    public PacketID packetID;
}
public abstract class InGamePacketButton : PacketButton
{
    public gameInfo_fromC gameInfo;
}
/// <summary>
/// make for separation.
/// </summary>
public abstract class GlobalPacketButton : PacketButton
{ }
