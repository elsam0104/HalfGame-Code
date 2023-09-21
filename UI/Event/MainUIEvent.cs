using GameEvent;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameEvent.UI
{
    public enum MainUIEventState
    {
        None,
        ShowUpperUI,
        ShowRoomList,
        InitWaitMemUI,
        OnJoinWaitMemUI,
        OnLeaveWaitMemUI,
        ChangeRoomMasterUI,
        CreateChatPanel,
        LeaveChatPanel,

        DefaultPanel,
        ShowRoomsPanel,
        ShowRankPanel,
        ShowTournamentPanel,
    }

    public class MainUIEvent : MonoSingleTon<MainUIEvent>, UISubject
    {

        public MainUIEventState State { get; set; } = MainUIEventState.None;
         
        private List<UIListener> _observers = new List<UIListener>();

        private IMessage packet;
        public IMessage Packet => packet;

        public void SetPacket(IMessage message, MainUIEventState state)
        {
            packet = message;
            State = state;
            Notice();
        }
        public void SetPacket(MainUIEventState state)
        {
            State = state;
            Notice();
        }

        public void Notice()
        {
            var temp = new List<UIListener>(_observers);
            foreach (var observer in temp)
            {
                observer.ListenerUpdate(this);
            }
        }

        public void StartListening(UIListener listener)
        {
            _observers.Add(listener);
        }

        public void StopListening(UIListener listener)
        {
            _observers.Remove(listener);
        }
    }

}