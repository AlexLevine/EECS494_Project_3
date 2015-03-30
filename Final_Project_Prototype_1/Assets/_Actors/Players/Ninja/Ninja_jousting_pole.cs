using UnityEngine;
using System.Collections;

public class Ninja_jousting_pole : MonoBehaviour
{
    public static string global_name = "ninja_jousting_pole";

    public int attack_power
    {
        get
        {
            return 10;
        }
    }// attack_power

    //--------------------------------------------------------------------------

    void Awake()
    {
        name = global_name;
    }

    //--------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        // print("thing");
        if (!Llama.get().is_charging)
        {
            return;
        }

        // print("spam");
        // print(other.gameObject);
        // var breakable_wall = other.gameObject.GetComponent<Breakable_wall>();
        // if (breakable_wall != null)
        // {
        //     breakable_wall.break_wall();
        //     return;
        // }

        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }

        var knockback = transform.forward * attack_power;
        knockback.y = 0;

        enemy.receive_hit(attack_power, knockback, gameObject);
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }// OnTriggerStay
}
