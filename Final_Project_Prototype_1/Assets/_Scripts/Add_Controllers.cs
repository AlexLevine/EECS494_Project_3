using UnityEngine;
using System.Collections;
using InControl;

public class Add_Controllers : MonoBehaviour {
    public enum players_set_e {
        WAITING_FOR_P1, 
        WAITING_FOR_P2, 
        DONE
    }

    public GameObject cont_watch_obj_1, cont_watch_obj_2;

    private Controller_Watcher p1, p2; 

    players_set_e waiting_for_player; 

    void Start()
    {
        p1 = cont_watch_obj_1.GetComponent<Controller_Watcher>();
        p2 = cont_watch_obj_2.GetComponent<Controller_Watcher>();

        waiting_for_player = players_set_e.WAITING_FOR_P1; 
    }

    // Update is called once per frame
    void Update () 
    {
        var next_device = look_for_input();
        if(next_device == null){
            return; 
        }

        switch(waiting_for_player){
            case players_set_e.WAITING_FOR_P1:
                p1.device = next_device; 
                waiting_for_player = players_set_e.WAITING_FOR_P2;
                break;

            case players_set_e.WAITING_FOR_P2:
                p2.device = next_device; 
                waiting_for_player = players_set_e.DONE;
                break; 

            case players_set_e.DONE:
                // Load the Next Scene
                print("loading the next scene");
                break;

            default:
                print("OH NO!");
                break; 
        }

    }

    void OnGUI(){
        GUI.Box(new Rect(0, 0, 220, 25), "Assigning Controllers");    
        
        switch(waiting_for_player){
            case players_set_e.WAITING_FOR_P1:
                GUI.Box(new Rect(0, 25, 220, 25), "Player 1 Please Press A");                
                break;
            case players_set_e.WAITING_FOR_P2:
                GUI.Box(new Rect(0, 25, 220, 25), "Player 2 Please Press A");                   
                break; 

        }
    }

    InputDevice look_for_input()
    {
        foreach(var device in InputManager.Devices){
            if(device.GetControl(InputControlType.Action1)){
                if(p1.device == device || p2.device == device){
                    continue;
                }

                return device; 
            }
        }
        return null; 
    }
}
