using UnityEngine;
using System.Collections;

public class Breakable_wall : MonoBehaviour {

    bool is_dead = false;
    private float time_until_despawn = 2f;

    void Start()
    {
        foreach(var child in GetComponentsInChildren<Rigidbody>())
        {
            child.isKinematic = true;
            child.useGravity = false;
            // child.TriggerDestruction(new Vector3(0,0,0), 11f);
        }
    }

    // public void break_wall()
    void OnTriggerEnter(Collider other)
    {
        print("boom");
        if (other.gameObject.tag != "Player" || !Llama.get().is_charging)
        {
            return;
        }
        foreach(var child in GetComponentsInChildren<Rigidbody>())
        {
            child.isKinematic = false;
            child.useGravity = true;
            child.AddForce(Vector3.one * Random.Range(-10.0F, 10.0F) * 150f);
            // child.TriggerDestruction(new Vector3(0,0,0), 11f);
        }
        is_dead = true;
    }


    void Update()
    {
        if(!is_dead)
        {
            return;
        }
        time_until_despawn -= Time.deltaTime;
        if(time_until_despawn <= 0)
        {
            Destroy(gameObject);
        }
    }

}