using UnityEngine;
using System.Collections;

public class Ninja_jousting_pole : MonoBehaviour
{
    public int attack_power
    {
        get
        {
            return (Llama.get().is_charging ? 10 : 0);
        }
    }// attack_power

    //--------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }

        enemy.on_hit_by_jousting_pole(attack_power);
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }// OnTriggerStay
}
