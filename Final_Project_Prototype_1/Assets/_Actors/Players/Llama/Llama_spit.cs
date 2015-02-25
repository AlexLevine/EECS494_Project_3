using UnityEngine;
using System.Collections;

public class Llama_spit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.receive_hit(9001);
        }

        print(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            return;
        }

        print("bye now");
        Destroy(gameObject);
    }
}
