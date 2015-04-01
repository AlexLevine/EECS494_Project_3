using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_projectile : MonoBehaviour {
    
    private int attack_power = 1; 

    void OnCollisionEnter(Collision c)
    {  
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<Actor>().receive_hit(
                attack_power, Vector3.zero, gameObject);
        }

        Destroy (gameObject);
    }// OnCollisionEnter
    
    void OnCollisionStay(Collision collision)
    {
        OnCollisionEnter(collision);
    } // OnCollisionStay
}
