using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAreaUp : GravityArea
{
    public override Vector3 GetGravityDirection(ApplyGravity applyGravity)
    {
        return -transform.up;
    }
}
