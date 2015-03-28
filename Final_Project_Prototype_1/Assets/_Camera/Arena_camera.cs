using UnityEngine;
using System.Collections;
using System;

public class Arena_camera : MonoBehaviour
{
    public GameObject arena_center;

    private float follow_distance = 15f;
    private float hover_height = 15f;

    // private float

    // Use this for initialization
    void Start()
    {
        if (arena_center == null)
        {
            throw new Exception(
                "arena center needs to be set by the script" +
                "that activated this one");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.RotateAround(
            arena_center.transform.position, Vector3.up, 30 * Time.deltaTime);

        var camera_offset = -transform.forward;
        camera_offset.y = 0;
        camera_offset = camera_offset.normalized;
        camera_offset *= follow_distance;

        camera_offset.y += hover_height;

        transform.LookAt(arena_center.transform.position);

    }
}
