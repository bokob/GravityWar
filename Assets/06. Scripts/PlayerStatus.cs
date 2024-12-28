using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerStatus : MonoBehaviourPunCallbacks
{
    [field: SerializeField] public bool IsLocalPlayer { get; set; }
    [SerializeField] public int HP { get; set; }
    [field: SerializeField] public int Time { get; set; }
    [field: SerializeField] public bool IsDie { get; set; }
    [field: SerializeField] public bool IsGround { get; set; }
    [field: SerializeField] public bool IsFall { get; set; }
    [field: SerializeField] public bool IsAim { get; set; }

    void Awake()
    {

    }

    void Init()
    {
        HP = 100;
    }

    void SetTime()
    {
        Time = 40;
    }

    void TakeDamage()
    {

    }
}