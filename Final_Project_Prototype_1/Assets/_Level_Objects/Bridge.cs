using UnityEngine;
using System.Collections;

public class Bridge : Switchee {
    public GameObject rotation_axis;

    private static float max_rotation = 90f;
    private static float rotation_speed = 90f;

    private float amount_rotated = 0;
    // private Vector3 rotation_origin;
    // private Vector3 rotation_axis;

    // private Vector3 rotate;
    
    // Use this for initialization
    void Start () 
    {
        // on = true;
    }
    
    // Update is called once per frame
    void FixedUpdate () 
    {
        if (!on || amount_rotated >= max_rotation)
        {
            return; 
        }

        var step = rotation_speed * Time.fixedDeltaTime;
        transform.RotateAround(
            rotation_axis.transform.position, transform.right, step);

        amount_rotated += step;
        // float temp = Quaternion.Angle (transform.rotation,Quaternion.Euler(0,0,0));
        // if (temp>=0 && temp<=90){
        //     transform.RotateAround(rotate, Vector3.right, 20 * Time.fixedDeltaTime);
        // }
    }
}
