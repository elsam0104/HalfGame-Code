using System;
using System.Collections.Generic;
using UnityEngine;
using NativeSerializableDictionary;
namespace GameEvent
{
    //All events's name. Use to call event.
    public enum DicEventType
    {
        None,
        Login,
        RefreshRoomList,
        CreateRoom,
        EnterChat,
        LeaveChat,
        ReturnLobby,
        PopMainPopUp,
        PushMainPopUp,
        ChangePanel,
        SkillBarToggle,
        PopGamePopUp,
        PushGamePopUp,
        TimerOperator,
        MemoJob,
        ClickSkillBtn,
        ResetSkillType,
    }

    public class EventManager : MonoSingleton<EventManager>
    {
        [SerializeField]
        private Dictionary<DicEventType, Delegate> eventDictionary = new Dictionary<DicEventType, Delegate>();
        //Example use
        //EventManager.Instance.StartListening(DicEventType.PushMainPopUp, new EventManager.deleV(MethodA));
        //EventManager.Instance.StartListening(DicEventType.PushMainPopUp, new EventManager.deleT<int>(MethodB));

        public void StartListening(DicEventType eventName, Delegate listener)
        {
            
            Delegate thisEvent;

            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                instance.eventDictionary[eventName] = Delegate.Combine(thisEvent, listener);
            }
            else
            {
                instance.eventDictionary.Add(eventName, listener);
            }
        }
        
        public void StopListening(DicEventType eventName, Delegate listener)
        {
            if(instance.eventDictionary.ContainsKey(eventName))
            {
                instance.eventDictionary[eventName] = Delegate.Remove(eventDictionary[eventName], listener);
            }
        }
        /*

        public void TriggerEvent(DicEventType eventName)
        {
            Delegate thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent?.DynamicInvoke();
            }

        }

        public void TriggerEvent<T>(DicEventType eventName, T message)
        {
            Delegate thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent?.DynamicInvoke(message);
            }

        }
        */
        public void TriggerEvent(DicEventType eventName, params object[] message)
        {
            Debug.LogWarning(message);
            Delegate thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent?.DynamicInvoke(message);
            }
        }
        
    }
}