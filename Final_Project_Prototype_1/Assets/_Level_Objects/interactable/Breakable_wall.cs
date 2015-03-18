using UnityEngine;
using System.Collections;

public class Breakable_wall : MonoBehaviour {

    bool is_dead = false; 
    public float time_since_murdered = 2f;  

    void OnTriggerEnter(Collider other){
        if(Llama.get().is_charging)
        {
            foreach(var child in GetComponentsInChildren<Rigidbody>())
            {
                child.AddForce(Vector3.one * Random.Range(-10.0F, 10.0F) * 150f);
                child.useGravity = true; 
                // child.TriggerDestruction(new Vector3(0,0,0), 11f);
            }
            is_dead = true; 
        }
    }


    void Update()
    {
        if(!is_dead)
        {
            return;
        }
        time_since_murdered -= Time.deltaTime; 
        if(time_since_murdered <= 0)
        {
            Destroy(gameObject);
        }
    }

}
