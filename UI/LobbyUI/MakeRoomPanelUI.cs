using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeRoomPanelUI : LobbyUIVoidF
{
    [SerializeField]
    private TMP_InputField roomName;
    [SerializeField]
    private TMP_InputField password;
    [SerializeField]
    private TMP_InputField maxPlayer;
    [SerializeField]
    private Toggle anonymousToggle;
    public override void Run()
    {
        CreateRoom();
    }
    public override bool HaveRunType()
    {
        return false;
    }

    private void CreateRoom()
    {
        LobbyPacketManager.Instance.CreateRoom(roomName.text, anonymousToggle.isOn, password.text, int.Parse(maxPlayer.text));
        PanelChangeManager.Instance.ChangePanel(PanelType.CreateRoom, true, false);
    }
}
