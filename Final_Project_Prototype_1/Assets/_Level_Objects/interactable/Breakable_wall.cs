using UnityEngine;
using System.Collections;

public class Breakable_wall : MonoBehaviour
{
    // public GameObject cracked_mesh;

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
    // void OnCollisionEnter(Collision other)
    // {
    //     // print(other.gameObject.tag);
    //     if (other.gameObject.tag != "Player" || !Llama.get().is_charging)
    //     {
    //         return;
    //     }
    //     print("Before Loop");
    //     foreach(var child in GetComponentsInChildren<Rigidbody>())
    //     {
    //         print("In Loop");
    //         child.isKinematic = false;
    //         child.useGravity = true;
    //         child.AddForce(Vector3.one * Random.Range(-10.0F, 10.0F) * 150f);
    //         // child.TriggerDestruction(new Vector3(0,0,0), 11f);
    //     }
    //     is_dead = true;
    // }

    public void break_wall()
    {
        print("boom");

        // print(other.gameObject.tag);
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        // print("Before Loop");
        foreach(var child in GetComponentsInChildren<Rigidbody>())
        {
            // print("In Loop");
            child.isKinematic = false;
            child.useGravity = true;
            child.AddForce(Vector3.one * Random.Range(-10.0F, 10.0F) * 150f);
            // child.TriggerDestruction(new Vector3(0,0,0), 11f);
        }
        is_dead = true;
    }

    // void OnTriggerStay(Collider other)
    // {
    //     OnTriggerEnter(other);
    // }


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
