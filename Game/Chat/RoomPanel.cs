using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PROTO;
using UnityEngine.UI;
using TMPro;
public class RoomPanel : MonoBehaviour
{
    public int roomID;
    public bool isPrivate;
    [SerializeField]
    private TMP_InputField password;
    [SerializeField]
    private Button panelBtn;
    [SerializeField]
    private Button enterBtn;
    [SerializeField]
    private GameObject conformImage;

    [SerializeField] private TMP_Text roomName;
    [SerializeField] private TMP_Text roomId;
    [SerializeField] private TMP_Text maxPlayer;

    [SerializeField]
    private GameObject anonymousIcon;
    [SerializeField]
    private GameObject privateIcon;

    public static RoomPanel clickedPanel;
    private void Start()
    {
        panelBtn.onClick.AddListener(CheckActivate);
        enterBtn.onClick.AddListener(EnterChat);
    }

    bool isActive = false;
    public void CheckActivate()
    {
        isActive = !isActive;
        Activate(isActive);
    }

    public void Activate(bool isActive)
    {
        this.isActive = isActive;
        if(isActive)
        {
            if(clickedPanel!=this)
                clickedPanel?.Activate(false);
            clickedPanel = this;
        }

        //conformImage.SetActive(true);
        enterBtn.gameObject.SetActive(isActive);
        password.gameObject.SetActive(isPrivate);
    }


    public void EnterChat()
    {
        if (isPrivate && password.text == "") return;
        LobbyPacketManager.Instance.EnterChat(roomID, password.text);
    }

    public void InitRoomData(Room room)
    {

        roomID = room.RoomID;
        isPrivate = room.IsPrivate;
        roomName.text = $"{room.RoomID + 1}. {room.RoomName}";


        roomId.text = $"{room.RoomID + 1}.";
        maxPlayer.text = $"{room.CurrentPlayer}/{room.MaxPlayer}";

        anonymousIcon.SetActive(room.IsAnonymous);
        privateIcon.SetActive(room.IsPrivate);
    }
}
