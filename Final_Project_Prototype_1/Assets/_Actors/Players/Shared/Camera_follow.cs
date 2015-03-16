using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour
{
    public static float min_camera_distance = 6f;
    // public static float camera_y_distance = 4f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        var llama = Llama.get().gameObject;
        var ninja = Ninja.get().gameObject;

        var distance = llama.transform.position - ninja.transform.position;

        var camera_distance = Mathf.Max(new float[] {
            Mathf.Abs(distance.x), Mathf.Abs(distance.y), Mathf.Abs(distance.z)
        });

        camera_distance = Mathf.Max(min_camera_distance, camera_distance);

        var midpoint = Vector3.Lerp(
            llama.transform.position, ninja.transform.position, 0.5f);

        var new_camera_pos = midpoint;
        new_camera_pos.z -= camera_distance;
        new_camera_pos.y += camera_distance;

        transform.position = new_camera_pos;
    }
}
