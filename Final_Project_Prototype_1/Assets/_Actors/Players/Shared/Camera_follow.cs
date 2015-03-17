using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour
{
    public static float min_camera_distance = 5f;
    public static float max_camera_distance = 5f;
    // public static float camera_y_distance = 4f;

    // Use this for initialization
    void Start()
    {

    }

    //--------------------------------------------------------------------------

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
        camera_distance = Mathf.Min(max_camera_distance, camera_distance);

        var midpoint = Vector3.Lerp(
            llama.transform.position, ninja.transform.position, 0.5f);

        var new_camera_pos = midpoint;
        new_camera_pos.z -= camera_distance;
        new_camera_pos.y += camera_distance;

        transform.position = new_camera_pos;
    }

    //--------------------------------------------------------------------------

    // public bool both_points_in_viewport(Vector3 first, Vector3 second)
    // {
    //     return false;
    // }// both_points_in_viewport

    //--------------------------------------------------------------------------

    public static bool point_step_would_leave_viewport(
        Vector3 point, Vector3 direction, float step)
    {
        if (direction.y != 0)
        {
            return false;
        }

        var desired_position = point + (step * direction);
        var viewport_pos = Camera.main.WorldToViewportPoint(desired_position);

        print("direction: " + direction);
        print("step: " + step);
        print("desired_position: " + desired_position);
        print("viewport_pos: " + viewport_pos);

        var leave_z_far_edge = direction.z > 0 && viewport_pos.y > 0.9f;
        var leave_z_near_edge = direction.z < 0 && viewport_pos.y < 0.1f;
        var leave_x_left_edge = direction.x < 0 && viewport_pos.x < 0.1f;
        var leave_x_right_edge = direction.x > 0 && viewport_pos.x > 0.9f;

        return leave_z_far_edge || leave_z_near_edge ||
               leave_x_left_edge || leave_x_right_edge;


        // if (!point_inside_viewport(viewport_pos))
        // {
        //     return true;
        // }
        // return point.x < 0.9f && point.x > 0.1f &&
        //        point.y < 0.9f && point.y > 0.1f;
    }// point_step_would_leave_viewport
}
