using UnityEngine;
using System.Collections;

public class Ninja_projectile : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.receive_hit(1);
            Timer.num_enemies_killed_by_ninja++;
        }

        if(other.gameObject.tag == "Player")
        {
            return;
        }

        Destroy(gameObject);
    }
}
