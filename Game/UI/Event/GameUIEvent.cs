using System.Collections.Generic;
using PROTO;
using Google.Protobuf;
using System.Collections;
using UnityEngine;

namespace GameEvent
{
    public interface UIListener
    {
        public void ListenerUpdate (UISubject subject);
    }
    public interface UISubject
    {
        public void StartListening(UIListener listener);
        public void StopListening(UIListener listener);
        public void Notice();
    }
}
namespace GameEvent.UI
{
    public enum GameUIEventState
    {
        None,
        ChangeDay,
        InitHp,
        ChangeHp,
        InitAtk,
        ChangeAtk,
        InitCoolTime,
        ChangeTimer,
        ShowJob,
        RoomMasterChanged,
        GameStarted,
        GameEnded, 
        InitUserPanel,
        LowerPanelOpen,
        LowerPanelClose,
        SkillUsableNotice,
        CoolTurn,
        UltimatePointNotice,
    }

    public class GameUIEvent : MonoSingleTon<GameUIEvent>, UISubject
    {
        // For the sake of simplicity, the Subject's state, essential to all
        // subscribers, is stored in this variable.
        public GameUIEventState State { get; set; } = GameUIEventState.None;

        // List of subscribers. In real life, the list of subscribers can be
        // stored more comprehensively (categorized by event type, etc.).
        private List<UIListener> _observers = new List<UIListener>();

        private IMessage packet;
        public IMessage Packet => packet;

        

        public void SetPacket(IMessage message, GameUIEventState state)
        {
            packet = message;
            State = state;
            Notice();
        }
        public void SetPacket(GameUIEventState state)
        {
            State = state;
            Notice();
        }
        public void Notice()
        {
                foreach (var observer in _observers)
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