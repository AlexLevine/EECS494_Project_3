using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_projectile : MonoBehaviour {

    private int attack_power = 1;

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            var actor = c.gameObject.GetComponent<Actor>();
            if (actor == null)
            {
                actor = c.gameObject.GetComponentInParent<Actor>();
            }
            actor.receive_hit(
                attack_power, Vector3.zero, gameObject);
        }

        Destroy (gameObject);
    }// OnCollisionEnter

    void OnCollisionStay(Collision collision)
    {
        OnCollisionEnter(collision);
    } // OnCollisionStay
}
