using UnityEngine;
using System.Collections;

public class Change_to_arena_cam : MonoBehaviour {

    void OnTriggerEnter(Collider other){
        if(!other.gameObject.name.Contains("Llama") && 
           !other.gameObject.name.Contains("Ninja"))
        {
            return;
        }

        Camera_follow.in_boss_arena = true;
    }

    void OnTriggerStay(Collider other){
        OnTriggerEnter(other);
    }
}
