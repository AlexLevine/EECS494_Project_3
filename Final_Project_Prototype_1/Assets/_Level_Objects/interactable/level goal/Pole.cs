using UnityEngine;
using System.Collections;

public class Pole : MonoBehaviour {
    public bool llama;
    public bool ninja;
    public bool level_over;

    // Use this for initialization
    void Start () {
        llama = false;
        ninja = false;
        level_over = false;
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        if (llama || ninja){
            GetComponent<Renderer>().material.color = Color.black;
            level_over = true;
        }
    }
    
    void OnTriggerEnter(Collider other){
        print("ahhh");
        print (other.name);
        if (other.name.Contains ("Ninja")){
            ninja = true;
        }
        if (other.name.Contains("Llama")){
            llama = true;
        }
    }

    void OnGUI()
    {
        if(level_over)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "You're Winner!");                
        }
    }
}
