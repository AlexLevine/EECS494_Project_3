using UnityEngine;
using System.Collections;

public class Ice_Projectile : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.receive_hit(9001);
        }
        if(other.tag == "Player")
        {
            return; 
        }
        
        Destroy(gameObject);
    }
}
