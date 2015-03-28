using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float cur_time;
    static public int num_enemies_killed_by_llama;
    static public int num_enemies_killed_by_ninja;

    public Text llama_txt, ninja_txt; 

    void Start()
    {
        cur_time = 0;
        num_enemies_killed_by_ninja = 0;
        num_enemies_killed_by_llama = 0;
    }

    void Update()
    {

        llama_txt.text = "Score: " + num_enemies_killed_by_llama.ToString();
        ninja_txt.text = "Score: " + num_enemies_killed_by_ninja.ToString();
        cur_time += Time.deltaTime;
    }


    void OnGUI()
    {
        // GUI.Box(new Rect(0, Screen.height - 25, 120, 25), "Time: " + cur_time.ToString().Substring(0,4));
        // GUI.Box(new Rect(0, Screen.height - 50, 120, 25), );
        // GUI.Box(new Rect(0, Screen.height - 75, 120, 25), 
    }
}
