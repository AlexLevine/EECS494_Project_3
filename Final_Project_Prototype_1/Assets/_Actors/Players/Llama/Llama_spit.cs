using UnityEngine;
using System.Collections;

public class Llama_spit : MonoBehaviour
{
    public int attack_power { get { return 5; } }

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
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "weapon")
        {
            return;
        }

        Destroy(gameObject);
    }
}
