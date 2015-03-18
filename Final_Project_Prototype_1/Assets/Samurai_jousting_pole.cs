﻿using UnityEngine;
using System.Collections;

public class Samurai_jousting_pole : MonoBehaviour
{
    public int attack_power
    {
        get
        {
            return 10;
        }
    }// attack_power

    //--------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player_character>();
        if (player == null || !Samurai_Attack.get().is_charging)
        {
            return;
        }

        var knockback = transform.forward * attack_power;
        player.receive_hit(attack_power, knockback);
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }// OnTriggerStay
}
