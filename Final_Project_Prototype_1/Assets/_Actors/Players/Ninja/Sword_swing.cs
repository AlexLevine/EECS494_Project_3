using UnityEngine;
using System.Collections;

public enum Sword_state_e
{
    IDLE,
    SWINGING
}

public class Sword_swing : MonoBehaviour
{
    private Vector3 sword_neutral_pos;
    private Quaternion sword_neutral_rotation;

    public GameObject sword_neutral;

    public Sword_state_e state;

    public float angle_distance_swung = 0;

    public GameObject swing_sound_player;
    public GameObject basic_attack_vocals;

    private GameObject sword;
    private float swing_speed = 520f;

    // private Vector3 swing_size;
    // private Vector3 rest_size;

    public bool is_playing
    {
        get
        {
            return state != Sword_state_e.IDLE;
        }
    }

    void Start()
    {
        sword = Ninja.get().sword_obj;

        // swing_size = sword.transform.localScale;
        // rest_size = swing_size;
        // rest_size.z = 2f;


        sword_neutral_pos = sword_neutral.transform.localPosition;
        sword_neutral_rotation = sword_neutral.transform.localRotation;
        // sword.transform.localScale = rest_size;
        sword.transform.localPosition = sword_neutral_pos;
        sword.transform.localRotation = sword_neutral_rotation;
    }

    void Update()
    {
        switch(state)
        {
        case Sword_state_e.IDLE:
            break;
        case Sword_state_e.SWINGING:
            sword.transform.RotateAround(
                transform.position, Vector3.up, -swing_speed * Time.deltaTime);

            var distance = Vector3.Angle(
                sword.transform.localPosition, sword_neutral_pos);
            if (distance < max_swing_angle_distance)
            {
                return;
            }

            sword.transform.localPosition = sword_neutral_pos;
            sword.transform.localRotation = sword_neutral_rotation;
            // sword.transform.localScale = rest_size;
            state = Sword_state_e.IDLE;
            // sword.GetComponent<Ninja_sword>().is_swinging = false;
            break;
        }
    }

    public void swing()
    {
        if (sword.GetComponent<Ninja_sword>().is_attacking)
        {
            return;
        }

        basic_attack_vocals.GetComponent<Sound_effect_randomizer>().play();
        swing_sound_player.GetComponent<Sound_effect_randomizer>().play();

        // HACK: calculate the swing start distance instead of hardcoding
        // sword.transform.localScale = swing_size;
        sword.transform.localPosition = new Vector3(1.27f, 0f, 1.2f);
        sword.transform.localEulerAngles = new Vector3(0, 45f, 0);
        state = Sword_state_e.SWINGING;
        // sword.GetComponent<Ninja_sword>().is_swinging = true;
    }

    public float max_swing_angle_distance
    {
        get
        {
            return 110f;
        }
    }


}
