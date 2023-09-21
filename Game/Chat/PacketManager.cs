using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Google.Protobuf;
using PROTO;
public interface IPacketHandler
{
    public void Process(IMessage packet);
}

public class PacketManager:MonoSingleTon<PacketManager>
{
    public Dictionary<PacketID,Action<ArraySegment<byte>,PacketID>> _OnRecv = new Dictionary<PacketID, Action<ArraySegment<byte>, PacketID>>();
    public Dictionary<PacketID,IPacketHandler> PacketHandler = new Dictionary<PacketID, IPacketHandler>();

    private void Awake() {
        _OnRecv[PacketID.ChatMessage] = MakePacket<chatMessage>;
        _OnRecv[PacketID.ShowRoomList] = MakePacket<ShowRoomList>;
        _OnRecv[PacketID.OnJoinRoom] = MakePacket<OnJoinRoom>;
        _OnRecv[PacketID.OnRoomMasterSelected] = MakePacket<OnRoomMasterSelected>;
        _OnRecv[PacketID.OnLeaveRoom] = MakePacket<OnLeaveRoom>;
        _OnRecv[PacketID.OnTextPanel] = MakePacket<TextPanel>;
        _OnRecv[PacketID.OnGameStart] = MakePacket<OnGameStart>;
        _OnRecv[PacketID.ShowMyJob] = MakePacket<ShowMyJob>;
        _OnRecv[PacketID.OnGameChatMessage] = MakePacket<OnGameChatMessage>;
        _OnRecv[PacketID.Timer] = MakePacket<Timer>;
        _OnRecv[PacketID.DayPass] = MakePacket<DayPass>;
        _OnRecv[PacketID.HpChange] = MakePacket<HpChange>;
        _OnRecv[PacketID.SkillUsableNotice] = MakePacket<SkillUsableNotice>;
        _OnRecv[PacketID.CoolDownStart] = MakePacket<CoolDownStart>;
        _OnRecv[PacketID.OnKilled] = MakePacket<OnKilled>;
        _OnRecv[PacketID.OnVoteKilled] = MakePacket<OnVoteKilled>;
        _OnRecv[PacketID.OnSomeoneKilled] = MakePacket<OnSomeoneKilled>;
        _OnRecv[PacketID.OnSomeoneVoteKilled] = MakePacket<OnSomeoneVoteKilled>;
        _OnRecv[PacketID.CloseGame] = MakePacket<CloseGame>;
        _OnRecv[PacketID.ImagePanelAndMessage] = MakePacket<ImagePanelAndMessage>;
        _OnRecv[PacketID.PlayerBox] = MakePacket<PlayerBox>;
        _OnRecv[PacketID.ForceLowerPanel] = MakePacket<ForceLowerPanel>;
        _OnRecv[PacketID.CoolTurnNotice] = MakePacket<CoolTurnNotice>;
        _OnRecv[PacketID.UltimatePointChanged] = MakePacket<UltimatePointChanged>;
        _OnRecv[PacketID.SeatEffect] = MakePacket<SeatEffect>;
        _OnRecv[PacketID.PopUpText] = MakePacket<PopUpText>;

        PacketHandler[PacketID.ChatMessage] = new OnChatMessageHandler();
        PacketHandler[PacketID.ShowRoomList] = new OnRoomListHandler();
        PacketHandler[PacketID.OnJoinRoom] = new OnJoinRoomHandler();
        PacketHandler[PacketID.OnRoomMasterSelected] = new OnRoomMasterSelectedHandler();
        PacketHandler[PacketID.OnLeaveRoom] = new OnLeaveRoomHandler();
        PacketHandler[PacketID.OnTextPanel] = new OnTextPanelHandler();
        PacketHandler[PacketID.OnGameStart] = new OnGameStartHandler();
        PacketHandler[PacketID.ShowMyJob] = new OnShowMyJobHandler();
        PacketHandler[PacketID.OnGameChatMessage] = new OnGameChatMessageHandler();
        PacketHandler[PacketID.Timer] = new OnTimerHandler();
        PacketHandler[PacketID.DayPass] = new OnDayPassHandler();
        PacketHandler[PacketID.HpChange] = new OnHpChangeHandler();
        PacketHandler[PacketID.SkillUsableNotice] = new OnSkillUseableNoticeHandler();
        PacketHandler[PacketID.CoolDownStart] = new OnCoolDownStartHandler();
        PacketHandler[PacketID.OnKilled] = new OnKilledHandler();
        PacketHandler[PacketID.OnVoteKilled] = new OnVoteKilledHandler();
        PacketHandler[PacketID.OnSomeoneKilled] = new OnSomeoneKilledHandler();
        PacketHandler[PacketID.OnSomeoneVoteKilled] = new OnSomeoneVoteKilledHandler();
        PacketHandler[PacketID.CloseGame] = new OnCloseGameHandler();
        PacketHandler[PacketID.ImagePanelAndMessage] = new OnIllustHandler();
        PacketHandler[PacketID.PlayerBox] = new OnPlayerBoxHandler();
        PacketHandler[PacketID.ForceLowerPanel] = new ForceLowerPanelHandler();
        PacketHandler[PacketID.CoolTurnNotice] = new CoolTurnNoticeHandler();
        PacketHandler[PacketID.UltimatePointChanged] = new UltimatePointChangedHandler();
        PacketHandler[PacketID.SeatEffect] = new SeatEffectHandler();
        PacketHandler[PacketID.PopUpText] = new PopUpTextHandler();



    }

    public void MakePacket<T>(ArraySegment<byte> data,PacketID packetId) where T:IMessage,new()
    {
        Debug.Log("makePacket");
        T packet = new T();
        Debug.Log("make binary to packet id:"+ packetId);
        try
        {
        packet.MergeFrom(data.Array,data.Offset+4,data.Count-4);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
        NetworkManager.Instance._RecvQueue.Enqueue(new PacketAndID{packetId = packetId,packet = packet});
    }    
}
