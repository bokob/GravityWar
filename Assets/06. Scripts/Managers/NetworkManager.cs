using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    static NetworkManager s_instance; 
    public NetworkManager Instance { get => s_instance; }

    [SerializeField] byte _maxPlayersPerRoom = 2;
    [SerializeField] string _gameVersion;

    void Awake()
    {
        // �̱���
        if (s_instance != null)
        {
            Destroy(this);
        }
        else
        {
            s_instance = this;
            DontDestroyOnLoad(s_instance);
        }
    }

    // ���� ����
    public void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            DisConnect();
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }

    // ������ ���� ���� ���� �� ȣ��Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ���� ���ӿ� ����, \n�κ� ������");
    }

    // ���� �� ����
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // �� ���� ���� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 ������");
    }

    // �� ���� ���� �� ȣ��Ǵ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� ������ ���� ���� ����\n�׷��Ƿ� �� �ϳ� ������ ����");
        
        // 2���� ������ �� �ִ� �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            MaxPlayers = _maxPlayersPerRoom
        });
    }

    // ���� ����
    public void DisConnect()
    {
        PhotonNetwork.Disconnect();
    }

    // ���� ���� �� ȣ��Ǵ� �Լ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"���� ���� ����: {cause}");
    }

}