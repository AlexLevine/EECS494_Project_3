using UnityEngine;
using System.Collections;

public class Freezable : MonoBehaviour {    

    // void OnCollisionEnter(Collision collision)
    // {
    //     print("on collision enter");
    //     if(collision.gameObject.name.Contains("ice_projectile(Clone)"))
    //     {
    //         turn_off_particles();
    //     }
    // }

    void OnTriggerEnter(Collider other){
        print("on trigger enter");
        if(other.gameObject.name.Contains("ice_projectile(Clone)"))
        {
            GetComponent<BoxCollider>().enabled = false;
            turn_off_particles();
        }
    }

    private void turn_off_particles()
    {
        Transform[] allChildren = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren) 
        {
            if(child.gameObject.name.Contains("Particle System") ||
               child.gameObject.name.Contains("Fire_Fence"))
            {
                child.gameObject.SetActive(false);                     
            }
        }
    }
}
