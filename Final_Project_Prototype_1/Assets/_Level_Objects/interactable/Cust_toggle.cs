using UnityEngine;
using System.Collections;

public class Cust_toggle : MonoBehaviour {    

    public GameObject toggle_item; 

    public enum toggle_action_e
    {
        TURN_OFF, // disables the toggle_item
        TURN_ON,  // enables the toggle_item object
        INCREMENT_PTS, // Adds points to the player
        CUSTOM_TOGGLE // calls the custom toggle script that should be attached
    }

    toggle_action_e action; 

    void OnTriggerEnter(Collider other){
        print("on trigger enter");
        if(!other.gameObject.name.Contains("Llama") &&
           !other.gameObject.name.Contains("Ninja"))
        {
            return; 
        }

        switch (action)
        {
        case toggle_action_e.TURN_OFF:
            toggle_item.SetActive(false);
            break;

        case toggle_action_e.TURN_ON:
            toggle_item.SetActive(true);
            break;

        case toggle_action_e.INCREMENT_PTS:
            if(other.gameObject.name.Contains("Llama"))
            {
                Timer.num_enemies_killed_by_llama++;
            }
            else
            {
                Timer.num_enemies_killed_by_ninja++;

            }
            break;

        case toggle_action_e.CUSTOM_TOGGLE:
            toggle_item.GetComponent<Toggle_target>().toggle_script();
            break;
        }
        case default:
            break; 
    }
}
