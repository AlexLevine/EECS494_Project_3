using UnityEngine;
using System.Collections;

public class Breakable_wall : MonoBehaviour
{
    public GameObject fragment_cube_prefab;

    bool is_dead = false;
    private float time_until_despawn = 2f;

    void Start()
    {
        var main_collider = GetComponent<Collider>();
        var x_begin = main_collider.bounds.min.x;
        var x_end = main_collider.bounds.max.x;
        var x_step = main_collider.bounds.extents.x / 2f;

        var y_begin = main_collider.bounds.min.y;
        var y_end = main_collider.bounds.max.y;
        var y_step = main_collider.bounds.extents.y / 2f;

        var z_begin = main_collider.bounds.min.z;
        var z_end = main_collider.bounds.max.z;
        var z_step = main_collider.bounds.extents.z / 2f;

        // print(x_step);
        // print(y_step);
        // print(z_step);

        for (var x = x_begin; x <= x_end; x += x_step)
        {
            for (var y = y_begin; y <= y_end; y += y_step)
            {
                for (var z = z_begin; z <= z_end; z += z_step)
                {
                    var block = Instantiate(
                        fragment_cube_prefab,
                        new Vector3(x, y, z),
                        Quaternion.identity) as GameObject;
                    // block.transform.localScale =
                    //         fragment_cube_prefab.transform.localScale;
                    block.transform.parent = transform.parent;
                    var rb = block.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    block.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        // foreach(var child in GetComponentsInChildren<Rigidbody>())
        // {
        //     child.isKinematic = true;
        //     child.useGravity = false;
        //     // child.TriggerDestruction(new Vector3(0,0,0), 11f);
        // }
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
        // print("boom");

        // print(other.gameObject.tag);
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        // GetComponent<Rigidbody>().enabled = false;

        // print("Before Loop");
        foreach(var child in transform.parent.GetComponentsInChildren<Rigidbody>())
        {
            print("In Loop");
            child.gameObject.GetComponent<MeshRenderer>().enabled = true;
            child.isKinematic = false;
            child.useGravity = true;
            // child.velocity = (Vector3.one * Random.Range(-1.0F, 1.0F) * 1f);
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
            Destroy(transform.parent.gameObject);
        }
    }

}
