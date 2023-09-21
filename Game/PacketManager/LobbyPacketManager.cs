using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PROTO;
using GameEvent;
using System;

public class LobbyPacketManager : MonoSingleton<LobbyPacketManager>
{
    private void Start()
    {
        EventManager.Instance.StartListening(DicEventType.LeaveChat,new Action(LeaveRoom));
        EventManager.Instance.StartListening(DicEventType.RefreshRoomList, new Action(Refresh));
    }
    public void EnterChat(int roomId,string password)
    {
        joinRoom joinRoom = new joinRoom();
        joinRoom.RoomID = roomId;
        joinRoom.Password = password;
        NetworkManager.Instance.Send(joinRoom, PacketID.JoinRoom);
    }
    public void GameStart(int roomId)
    {
        gameStart gameStart = new gameStart();
        gameStart.RoomID = roomId;
        NetworkManager.Instance.Send(gameStart, PacketID.GameStart);
    }
    public void CreateRoom(string roomName, bool isAnonymous, string password,int maxPlayer,int maxCardLevel = 1)
    {
        createRoom createRoom = new createRoom();
        createRoom.RoomName = roomName;
        bool isPrivate = (password.Length != 0);
        createRoom.IsPrivate = isPrivate;
        createRoom.IsAnonymous = isAnonymous;
        createRoom.Password = password;
        createRoom.MaxPlayer = maxPlayer;
        createRoom.MaxCardLevel = maxCardLevel;
        NetworkManager.Instance.Send(createRoom, PacketID.CreateRoom);
    }

    public void LeaveRoom()
    {
        leaveRoom leaveRoom = new leaveRoom();
        leaveRoom.RoomID = UserSOManager.Instance.dataSO.curRoomID;
        NetworkManager.Instance.Send(leaveRoom, PacketID.LeaveRoom);
    }
    public void Refresh()
    {
        requireRoomList requireRoomList = new requireRoomList();
        NetworkManager.Instance.Send(requireRoomList, PacketID.RequireRoomList);
    }
}
