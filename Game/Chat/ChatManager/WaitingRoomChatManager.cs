using PROTO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomChatManager : ChatManager
{
    [SerializeField]
    private TMP_InputField textField;
    [SerializeField]
    private RectTransform contentRect;
    [SerializeField]
    private Scrollbar scrollbar;

    public void OnPrevChat()
    {
        if (textField.text.Trim() == "") return; //스페이스, 엔터 걸러줌
        textField.text = textField.text.Trim();

        SendPrevChat(textField.text);
        textField.text = string.Empty;
    }

    public void SendPrevChat(string str)
    {
        chatMessage chatMessage = new chatMessage();
        chatMessage.Text = str;
        NetworkManager.Instance.Send(chatMessage, PacketID.ChatMessage);
    }
    public void WaitingRoomChatClear()
    {
        ChatClear(contentRect);
    }
    public void WaitingRoomNotice(string notice)
    {
        Notice(contentRect, scrollbar, notice);
    }
    public void WaitingRoomChat(bool isSend, string text, string user, Sprite picture = null)
    {
        Chat(contentRect, scrollbar, isSend, text, user, picture, ChatType.Dayalight);
    }
}
