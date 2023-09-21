using GameEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent.UI;
using PROTO;
using TMPro;

public class ChatPanelUI : MonoBehaviour, UIListener
{
    public OnJoinRoom packet;
    [SerializeField]
    private GameObject chatPanel;
    [SerializeField]
    private TMP_Text roomName;
    [field:SerializeField]
    public WaitingRoomChatManager waitingRoomChatManager { get; private set; }
    [field: SerializeField]
    public RoomMasterUI roomMasterUI { get; private set; }
    [field: SerializeField]
    public WaitMemUI waitMemUI { get; private set; } 


    private Stack<int> currentRoomID = new Stack<int>();

    private void Awake()
    {
        waitingRoomChatManager = GetComponent<WaitingRoomChatManager>();
        roomMasterUI = GetComponent<RoomMasterUI>();
        MainUIEvent.Instance.StartListening(this);
        GameUIEvent.Instance.StartListening(this);
    }

    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
        MainUIEvent.Instance.StopListening(this);
    }
    private void Start()
    {
        MainUIEvent.Instance.SetPacket(packet, MainUIEventState.ShowUpperUI);
        MainUIEvent.Instance.SetPacket(packet, MainUIEventState.ChangeRoomMasterUI);
        MainUIEvent.Instance.SetPacket(packet, MainUIEventState.InitWaitMemUI);
        MainUIEvent.Instance.SetPacket(packet, MainUIEventState.OnJoinWaitMemUI);
    }
    public void ShowUpperUI(int roomId, string name) //add room name and users later
    {
        roomName.text = name;
        currentRoomID.Push(roomId);
    }

    public void LeaveChat()
    {
        chatPanel.SetActive(false);
        currentRoomID.Pop();
    }

    public void ListenerUpdate(UISubject subject)
    {
        if (subject is MainUIEvent)
        {
            MainUIEvent ui = (MainUIEvent)subject;

            if (ui.State == MainUIEventState.ShowUpperUI)
            {
                OnJoinRoom packet = ui.Packet as OnJoinRoom;
                {
                    ShowUpperUI(packet.Room.RoomID, packet.Room.RoomName??"");
                }
            }
        }
    }

}
