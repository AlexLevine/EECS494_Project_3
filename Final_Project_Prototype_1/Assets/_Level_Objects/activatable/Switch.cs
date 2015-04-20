using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Switch : MonoBehaviour
{
    public GameObject switchee;
    public bool cut_scene;
    public GameObject goal_walls;
    
    private bool triggered = false;
    // Use this for initialization
    void Start ()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    //--------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
    	// if it's not the player or the switch has already been triggered by the player
    	// then do nothing:
        if (other.gameObject.tag != "Player" || triggered)
        {
            return;
        }

		if (cut_scene)
        {
			Cut_scene cs = goal_walls.GetComponent<Goal_wall_array>().get_next_wall();
			cs.activate();
			triggered = true;
		}
		else
        {
			Switchee sw = switchee.GetComponent<Switchee>();
			sw.activate(null, null);
		}

        Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
}
