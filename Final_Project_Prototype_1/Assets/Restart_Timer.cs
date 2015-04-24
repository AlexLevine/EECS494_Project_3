using UnityEngine;
using System.Collections;

public class Restart_Timer : MonoBehaviour {

    private float time_to_restart; 
    private float cur_time;   

    // Use this for initialization
    void Start () {
        cur_time = 0f; 
        time_to_restart = 10f; 
    }
    
    // Update is called once per frame
    void Update () {
        cur_time += Time.deltaTime; 
        print("Time To Restart: "  + (time_to_restart - cur_time));

        if(cur_time > time_to_restart)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
