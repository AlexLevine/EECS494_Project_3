using UnityEngine;
using System.Collections;

public class Diamond_Manager : Switchee {

    public bool running = false; 
    public float speed = 2f; 
    public GameObject target_position; 
    
    // Update is called once per frame
    void Update () 
    {
        if(!running)
        {
            return; 
        }

        lerp_downwards(); 
        rotate_around_self(); 

    }

    public override void activate(
        Switchee_callback callback, float? callback_delay)
    {
        base.activate(callback, callback_delay); 

        running = true; 
    }

    private void lerp_downwards()
    {
        transform.position = Vector3.Lerp(transform.position,
            target_position.transform.position, Time.deltaTime);
    }

    private void rotate_around_self()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 20f);
    }

    IEnumerator
}
