using UnityEngine;
using System.Collections;

public class Ninja_sword : MonoBehaviour
{
    public static string global_name = "ninja_sword";

    public GameObject hit_sound_player;

    public bool is_swinging = false;

    public int attack_power
    {
        get
        {
            return 5;
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
        if (!is_swinging)
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }

        var knockback_velocity = Ninja.get().transform.forward * attack_power;
        enemy.receive_hit(attack_power, knockback_velocity, gameObject);

        // hit_sound_player.GetComponent<AudioSource>().Play();
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }// OnTriggerStay

    //--------------------------------------------------------------------------

    // // Use this for initialization
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
