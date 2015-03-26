using UnityEngine;
using System.Collections;

public class Health_gui : MonoBehaviour {

    Ninja ninja;
    Llama llama;
    
    public int num_lives;
    

    void Awake(){
        Time.timeScale=1;
    }

    // Use this for initialization
    void Start () {
        //ninja = GameObject.Find ("Ninja").GetComponent<Ninja> ();
        //llama = GameObject.Find ("Llama").GetComponent<Llama> ();
    }


    void OnGUI(){
        const int windowWidth = 200;
        const int windowHeight = 150;
        Rect windowRect = new Rect((Screen.width - windowWidth) /2,
                                   (Screen.height - windowHeight)/2,
                                   windowWidth,windowHeight);

        if (llama.health<=0 || ninja.health<=0){
            Time.timeScale=0;
            GUILayout.Window (0,windowRect,RestartMenu,"You lost :(");
        }
    }

    private void RestartMenu(int id){
        // var nin = GameObject.Find ("Ninja");
        // var ll = GameObject.Find ("Llama");
        if (GUILayout.Button ("Restart")){
            //Destroy (nin); Destroy (ll);
            Application.LoadLevel (Application.loadedLevel);
        }
    }


    // Update is called once per frame
    void Update () {
        while (!ninja || !llama){
            ninja = Ninja.get();
            llama = Llama.get();
            return;
        }

        GUIText gt = this.GetComponent<GUIText> ();

        gt.text = "Ninja Health: " + ninja.health + "\nLlama Health: " + llama.health;

    }
}
