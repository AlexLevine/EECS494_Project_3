using UnityEngine;
using System.Collections;

public class Freezable : MonoBehaviour {    

    void OnTriggerEnter(Collider other){
        if(other.gameObject.name.Contains("ice_projectile"))
        {
            turn_off_particles();
        }
    }

    private void turn_off_particles()
    {
        Transform[] allChildren = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren) 
        {
            if(child.gameObject.name.Contains("Particle System"))
            {
                    child.gameObject.SetActive(false);                     
            }
        }
    }
}
