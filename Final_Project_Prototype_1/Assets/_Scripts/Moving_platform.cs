using UnityEngine;
using System.Collections;
using System;

public class Moving_platform : MonoBehaviour 
{
    private Vector3 start_location; 
    private Vector3 next_location, end_location; 
    public GameObject end_location_obj;  // Empty Gameobject representing where the block should end up 


    public bool active; 
    public bool does_loop; 
    public float speed = 0.2f; 

    void Start()
    {
        start_location = this.transform.position; 

        if(end_location == null)
        {
            throw new Exception("You must attach an end location to " + this.gameObject.name);
            active = false; 
        }
        end_location = end_location_obj.transform.position; 
    }

    void Update()
    {
        if(!active)
        {
            return; 
        }

        if(Mathf.Abs(transform.position.x - next_location.x) < 0.2f && 
           Mathf.Abs(transform.position.y - next_location.y) < 0.2f && 
           Mathf.Abs(transform.position.z - next_location.z) < 0.2f)
        {
            if(next_location == start_location)
            {
                next_location = end_location; 
            }
            else
            {
                next_location = start_location; 
            }
        }
        transform.position = Vector3.Lerp(transform.position,
                                         next_location, Time.deltaTime * speed);


    }
}
