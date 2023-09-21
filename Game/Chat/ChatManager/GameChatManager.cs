using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using PROTO;

public class GameChatManager : ChatManager
{
    [SerializeField]
    private TMP_InputField textField;
    [SerializeField]
    private RectTransform contentRect;
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private Button chatButton;
    [SerializeField]
    private GameObject ImageAreaPrefab;

    private void Start()
    {
        chatButton.onClick.AddListener(OnClickSubmit);
    }
    public void OnClickSubmit()
    {
        if (textField.text.Trim() == "") return; //스페이스, 엔터 걸러줌
        textField.text = textField.text.Trim();

        PlayerManager.Instance.SendChat(textField.text);
        //Chat(!isother, textField.text, isother ? "타인" : "나");
        textField.text = string.Empty;
    }

    public void GameChatClear()
    {
        ChatClear(contentRect);
    }
    public void GameNotice(string notice)
    {
        Notice(contentRect, scrollbar, notice);
    }
    public void GameChat(bool isSend, string text, string user,int seat = 0, ChatType chatType = ChatType.Dayalight)
    {
        Chat(contentRect, scrollbar, isSend, text, user, JobImage.Instance.IconImage[UserSOManager.Instance.memoSO.memoJobs[seat-1]],chatType);
    }
    public void GameImage(string texture, string text)
    {
        ImageNotice(contentRect, scrollbar, texture, text, ImageAreaPrefab);
    }
}