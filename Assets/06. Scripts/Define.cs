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
        Default,  // �⺻
        Aim,      // ����
        Observer, // ������ ����
        Tracer    // �߻�ü ����
    }

    public enum GravityObject
    {
        None,
        Player,
        Launcher,
        Projectile
    }
}