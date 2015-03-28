using UnityEngine;
using System.Collections;

public class Llama_spit : MonoBehaviour
{
    public int attack_power { get { return 5; } }

    public float time_left = 0.7f;

    void Update()
    {
        time_left -= Time.deltaTime;

        if (time_left < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // print("blaaaah");
        var enemy = other.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.on_hit_spit(
                attack_power, Vector3.zero);
            Timer.num_enemies_killed_by_llama++;
        }

        // print(other.gameObject.tag);
        var hit_player = other.gameObject.tag == "Player";
        var hit_weapon = other.gameObject.tag == "weapon";
        var hit_trigger = other.isTrigger;
        if(hit_player || hit_weapon || hit_trigger)
        {
            return;
        }

        print(other);

        Destroy(gameObject);
    }
}
