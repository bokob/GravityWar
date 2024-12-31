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
        Default,  // 기본
        Aim,      // 조준
        Observer, // 관찰자 시점
        Tracer    // 발사체 추적
    }

    public enum GravityObject
    {
        None,
        Player,
        Launcher,
        Projectile
    }
}