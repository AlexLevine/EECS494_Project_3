using UnityEngine;
using System.Collections;

public class Switchee : MonoBehaviour {
    public bool on;

    // Use this for initialization
    void Awake () {
        on = false;
    }

    public virtual void activate()
    {
        on = true; 
    }
        

}
