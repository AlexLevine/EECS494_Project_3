using UnityEngine;
using System.Collections;

public class Boring_enemy : Enemy
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
    }// Update

    //--------------------------------------------------------------------------

    protected override int max_health
    {
        get
        {
            return 5;
        }
    }

    //--------------------------------------------------------------------------

    protected override int attack_power
    {
        get
        {
            return 5;
        }
    }// attack_power
}
