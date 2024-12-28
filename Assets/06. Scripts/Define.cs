using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public enum WorldObject
    {
        Unknown,
        Player,
    }
    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum CameraMode
    {
        Observer, // 관찰자 시점
        Turn,     // 플레이 차례 시점
        NotTurn
    }
}