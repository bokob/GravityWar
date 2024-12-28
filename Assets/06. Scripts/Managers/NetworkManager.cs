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
        // 싱글톤
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

    // 서버 연결
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

    // 마스터 서버 접속 성공 시 호출되는 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 접속에 성공, \n로비에 진입함");
    }

    // 게임 방 참여
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // 방 접속 성공 시 호출되는 함수
    public override void OnJoinedRoom()
    {
        Debug.Log("방에 들어왔음");
    }

    // 방 접속 실패 시 호출되는 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("접속 가능한 랜덤 방이 없음\n그러므로 방 하나 생성할 예정");
        
        // 2명이 접속할 수 있는 방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            MaxPlayers = _maxPlayersPerRoom
        });
    }

    // 연결 종료
    public void DisConnect()
    {
        PhotonNetwork.Disconnect();
    }

    // 연결 종료 시 호출되는 함수
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"연결 종료 이유: {cause}");
    }

}