using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {
    void OnTriggerEnter(Collider other){
    
        if (other.GetComponent<Player_character>() == null)
        {
            return; 
        }
        
        Player_character p = other.GetComponent<Player_character>();
        p.add_health(2);
        Destroy(gameObject);
    }
}
