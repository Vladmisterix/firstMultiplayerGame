using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UIElements;


public class RoomMenu : MonoBehaviour
{
    private Button _startGameBtn;
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Label roomName = root.Query<Label>(name: "RoomName");
        roomName.text = "Комната создана: " + PhotonNetwork.CurrentRoom.Name;

        _startGameBtn = root.Query<Button>(name: "StartGameBattle");
        _startGameBtn.clicked += StartGameBtnOnClicked;

        if (!PhotonNetwork.IsMasterClient)
        {
            _startGameBtn.SetEnabled(false);
        }
    }

    private void StartGameBtnOnClicked()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
