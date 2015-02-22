using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour
{
    public static float camera_z_distance = -4f;
    public static float camera_y_distance = 4f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var llama = GameObject.Find("Llama");
        var ninja = GameObject.Find("Ninja");

        var midpoint = Vector3.Lerp(
            llama.transform.position, ninja.transform.position, 0.5f);

        var new_camera_pos = midpoint;
        new_camera_pos.z += camera_z_distance;
        new_camera_pos.y += camera_y_distance;

        transform.position = new_camera_pos;
    }
}
