using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_projectile : MonoBehaviour
{

    public Vector3 velocity = Vector3.zero;

    private int attack_power = 1;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    void OnTriggerEnter(Collider c)
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

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    } // OnCollisionStay
}
