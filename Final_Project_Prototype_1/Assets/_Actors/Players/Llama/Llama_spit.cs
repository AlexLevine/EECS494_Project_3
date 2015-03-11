using UnityEngine;
using System.Collections;

public class Llama_spit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.on_hit_spit(1);
            Timer.num_enemies_killed_by_llama++; 
        }

        print(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            return;
        }

        Destroy(gameObject);
    }
}
