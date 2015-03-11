using UnityEngine;
using System.Collections;

public class Ninja_jousting_pole : MonoBehaviour
{
    public int attack_power
    {
        get
        {
            return 1 * GameObject.Find("Llama").rigidbody.velocity;
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

        enemy.on_hit_sword(attack_power);
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }// OnTriggerStay
}
