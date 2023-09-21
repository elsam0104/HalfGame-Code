using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeSerializableDictionary;
using GameEvent;
using System;

public enum GamePopUpType
{
    None,
    JobPanel,
    TimeChange,
    JobMemo,
}

public abstract class GamePopUpPanel :MonoBehaviour
{
    public abstract void OnPushed();
    public abstract void OnPopped();
}
public class GamePopUpManager : MonoSingleTon<GamePopUpManager>
{
    [SerializeField]
    private SerializableDictionary<GamePopUpType, GamePopUpPanel> popUpDic = new SerializableDictionary<GamePopUpType, GamePopUpPanel>();
    [SerializeField]
    private GameObject popUpParantPanel;

    private GameObject undoPanel;
    private Stack<GamePopUpType> popUpStack = new Stack<GamePopUpType>();

    private void Start()
    {
        EventManager.Instance.StartListening(DicEventType.PushGamePopUp, new Action<GamePopUpType>(Push));
        EventManager.Instance.StartListening(DicEventType.PopGamePopUp, new Action(Pop));
        undoPanel = popUpParantPanel.transform.GetChild(0).gameObject;
    }
    private void OnDestroy()
    {
        EventManager.Instance.StopListening(DicEventType.PushGamePopUp, new Action<GamePopUpType>(Push));
        EventManager.Instance.StopListening(DicEventType.PopGamePopUp, new Action(Pop));
    }
    public void Pop() //type 변수는 항상 None이다.
    {
        GamePopUpType type = GamePopUpType.None;
        Debug.Log("Pop");
        {
            if (popUpStack.TryPop(out type))
            {
                popUpDic[type].OnPopped();
                if (popUpStack.Count == 0)
                    ChangeUndoPanel(false);
            }
        }
    }
    /*
    public void Pop(GamePopUpType gamePopUpType)
    {
        if(popUpDic.ContainsKey(gamePopUpType))
        {

            popUpDic[gamePopUpType].SetActive(false);

            if (popUpStack.Count == 0)
                ChangeUndoPanel(false);
        }
    }
    */
    public void Push(GamePopUpType type)
    {
        if (isFirstPush())
            ChangeUndoPanel(true);
        popUpStack.Push(type);
        popUpDic[type].OnPushed();
    }

    private void ChangeUndoPanel(bool turnOn)
    {
        undoPanel.SetActive(turnOn);
    }
    private bool isFirstPush()
    {
        foreach (Transform child in popUpParantPanel.transform)
        {
            if (child.childCount != 0 && child.gameObject.activeSelf == true)
                return false;
        }
        return true;
    }
}