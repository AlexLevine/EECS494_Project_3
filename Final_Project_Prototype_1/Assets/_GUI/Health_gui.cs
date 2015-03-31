using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Health_gui : MonoBehaviour {

    Ninja ninja;
    Llama llama;


    void Start()
    {
        ninja = Ninja.get();
        llama = Llama.get();
    }
    
    public Slider llama_health, ninja_health;     

    // void OnGUI(){
    //     const int windowWidth = 200;
    //     const int windowHeight = 150;
    //     Rect windowRect = new Rect((Screen.width - windowWidth) /2,
    //                                (Screen.height - windowHeight)/2,
    //                                windowWidth,windowHeight);

    //     if (llama.health<=0 || ninja.health<=0){
    //         Time.timeScale=0;
    //         GUILayout.Window (0,windowRect,RestartMenu,"You lost :(");
    //     }
    // }

    // private void RestartMenu(int id){
    //     // var nin = GameObject.Find ("Ninja");
    //     // var ll = GameObject.Find ("Llama");
    //     if (GUILayout.Button ("Restart")){
    //         //Destroy (nin); Destroy (ll);
    //         Application.LoadLevel (Application.loadedLevel);
    //     }
    // }


    // Update is called once per frame
    void Update () {
        while (!ninja || !llama){
            ninja = Ninja.get();
            llama = Llama.get();
            return;
        }

        float cur_llama_health = (float)llama.health / (float)llama.max_health; 
        float cur_ninja_health = (float)ninja.health / (float)ninja.max_health; 

        // print(cur_ninja_health);
        // print(cur_llama_health);

        llama_health.normalizedValue = cur_llama_health; 
        ninja_health.normalizedValue = cur_ninja_health; 


        // GUIText gt = this.GetComponent<GUIText> ();
        // gt.text = "Ninja Health: " + ninja.health + "\nLlama Health: " + llama.health;
    }
}
