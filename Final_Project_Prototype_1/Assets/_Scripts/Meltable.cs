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
        if(transform.localScale == Vector3.zero)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.name.Contains("fire_breath"))
        {
            melting = true; 
            //Destroy(gameObject);
        }
    }
}
