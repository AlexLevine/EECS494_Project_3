using UnityEngine;
using System.Collections;

public class Meltable : MonoBehaviour {

    bool melting; 

    // Use this for initialization
    void Start () 
    {
        melting = false; 
    }
    
    // Update is called once per frame
    void Update () 
    {
        if(melting)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other){
        print("HELLO!");
        if(other.gameObject.name.Contains("fire_breath"))
        {
            print("Setting Melting to true");
            melting = true; 
            //Destroy(gameObject);
        }
    }
}
