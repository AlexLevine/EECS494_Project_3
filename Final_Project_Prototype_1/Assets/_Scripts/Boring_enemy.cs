using UnityEngine;
using System.Collections;

public class Boring_enemy : Enemy
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }// Start

    //--------------------------------------------------------------------------

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();
    }// Update

    //--------------------------------------------------------------------------

    public override int max_health
    {
        get
        {
            return 5;
        }
    }

    //--------------------------------------------------------------------------

    public override int attack_power
    {
        get
        {
            return 5;
        }
    }// attack_power
}
