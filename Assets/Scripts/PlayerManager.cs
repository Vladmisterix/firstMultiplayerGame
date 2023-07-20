using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private System.Random random = new System.Random();
    private PhotonView _photonView;
    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        if(_photonView.IsMine)
            CreatePlayer();
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate("PLayer", new Vector3(random.Next(-20,20), 2.16f, random.Next(-20, 20)), Quaternion.Euler(-90f, 0f, 0f));
    }
}
