using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Health_gui : MonoBehaviour {

    Ninja ninja;
    Llama llama;
    
    public GameObject llama_fill_bar;
    public GameObject ninja_fill_bar;
    
    Color full_health_col;
    Color half_health_col;
    Color small_health_col;


    void Start()
    {
        ninja = Ninja.get();
        llama = Llama.get();
        
        full_health_col = llama_fill_bar.GetComponent<Image>().color;
        half_health_col = Color.yellow;
        small_health_col = Color.red;
        
        //half_health_col = Color.
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
        
        
		if (cur_llama_health < .25) {
			llama_fill_bar.GetComponent<Image>().color = small_health_col;
		} else if (cur_llama_health < .5) {
        	// when health gets lower than half, turn the bar yellow
        	llama_fill_bar.GetComponent<Image>().color = half_health_col;
        } else {
        	llama_fill_bar.GetComponent<Image>().color = full_health_col;
        }
        
		if (cur_ninja_health < .25) {
			ninja_fill_bar.GetComponent<Image>().color = small_health_col;
		} else if (cur_ninja_health < .5) {
        	ninja_fill_bar.GetComponent<Image>().color = half_health_col;
        } else {
        	ninja_fill_bar.GetComponent<Image>().color = full_health_col;
        }	


        llama_health.normalizedValue = cur_llama_health; 
        ninja_health.normalizedValue = cur_ninja_health; 


        // GUIText gt = this.GetComponent<GUIText> ();
        // gt.text = "Ninja Health: " + ninja.health + "\nLlama Health: " + llama.health;
    }
}
