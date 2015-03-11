using UnityEngine;
using System.Collections;

public enum Sword_state_e
{
    IDLE,
    SWINGING
}

public class Sword_swing : MonoBehaviour
{
    public GameObject sword;
    public GameObject sword_swing_end;

    public Vector3 sword_neutral_pos;
    public Vector3 sword_neutral_rotation;

    public Sword_state_e state;

    public float angle_distance_swung = 0;

    void Start ()
    {
        sword_neutral_pos = sword.transform.localPosition;
        sword_neutral_rotation = sword.transform.localEulerAngles;
    }

    void Update ()
    {
        switch(state)
        {
        case Sword_state_e.IDLE:
            break;
        case Sword_state_e.SWINGING:
            // sword.transform.localPosition = Vector3.Lerp(
            //     sword.transform.localPosition,
            //     sword_swing_end.transform.localPosition,
            //     0.05f);

            // sword.transform.localEulerAngles = Vector3.RotateTowards(
            //     sword.transform.localEulerAngles,
            //     sword_swing_end.transform.localEulerAngles,
            //     0.5f, 0f);


            // var distance_to_end = Vector3.Distance(
            //     sword.transform.localPosition,
            //     sword_swing_end.transform.localPosition);
            // // print(distance_to_end);
            // if (distance_to_end >= 0.1f)
            // {
            //     return;
            // }

            sword.transform.RotateAround(
                transform.position, Vector3.up, -360f * Time.deltaTime);

            var distance = Vector3.Angle(
                sword.transform.localPosition, sword_neutral_pos);
            if (distance < max_swing_angle_distance)
            {
                return;
            }

            sword.transform.localPosition = sword_neutral_pos;
            sword.transform.localEulerAngles = sword_neutral_rotation;
            state = Sword_state_e.IDLE;
            sword.GetComponent<Ninja_sword>().is_swinging = false;
            break;
        }
    }

    public void swing()
    {
        if (is_swinging)
        {
            return;
        }

        // HACK: calculate the swing start distance instead of hardcoding
        sword.transform.localPosition = new Vector3(1.27f, 0f, 1.2f);
        sword.transform.localEulerAngles = new Vector3(0, 45f, 0);
        state = Sword_state_e.SWINGING;
        sword.GetComponent<Ninja_sword>().is_swinging = true;
    }

    public float max_swing_angle_distance
    {
        get
        {
            return 90f;
        }
    }

    public bool is_swinging
    {
        get
        {
            return state != Sword_state_e.IDLE;
        }
    }
}
