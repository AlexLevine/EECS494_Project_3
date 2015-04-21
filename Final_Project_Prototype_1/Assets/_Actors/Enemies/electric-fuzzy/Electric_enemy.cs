using UnityEngine;
using System.Collections;
using System;

public class Electric_enemy : Point_lerp_enemy
{
    public override float attack_power
    {
        get
        {
            return 1;
        }
    }

    public override float max_health
    {
        get
        {
            return 3;
        }
    }
}
