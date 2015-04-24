using UnityEngine;
using System.Collections;

public enum Aerial_attack_state_e
{
    NOT_ATTACKING,
    WINDING_UP,
    DIVING,
    COOLING_DOWN
}

public class Aerial_attack : MonoBehaviour
{
    public bool is_playing { get {
        return state != Aerial_attack_state_e.NOT_ATTACKING; } }
    public bool is_diving { get {
        return state == Aerial_attack_state_e.DIVING; } }

    public GameObject sword_neutral_position;
    public GameObject sword_attack_position;

    public GameObject attack_vocals;

    public GameObject shockwave;

    private Aerial_attack_state_e state;

    private GameObject sword;

    // private Vector3 sword_neutral_pos;
    // private Vector3 sword_neutral_rotation;

    private Vector3 dive_velocity = new Vector3(0, -40, 0);

    void Start()
    {
        sword = Ninja.get().sword_obj;

        // sword_neutral_pos = sword.transform.localPosition;
        // sword_neutral_rotation = sword.transform.localEulerAngles;
    }// Start

    //--------------------------------------------------------------------------

    void Update()
    {
        switch (state)
        {
        case Aerial_attack_state_e.NOT_ATTACKING:
            break;

        case Aerial_attack_state_e.WINDING_UP:
            var reached_target = step_sword_towards(
                sword_attack_position.transform.position,
                sword_attack_position.transform.rotation);
            if (!reached_target)
            {
                break;
            }

            state = Aerial_attack_state_e.DIVING;
            attack_vocals.GetComponent<Sound_effect_randomizer>().play();
            break;

        case Aerial_attack_state_e.DIVING:
            Ninja.get().apply_momentum(dive_velocity);
            break;

        case Aerial_attack_state_e.COOLING_DOWN:
            var reached_neutral = step_sword_towards(
                sword_neutral_position.transform.position,
                sword_neutral_position.transform.rotation);
            if (reached_neutral)
            {
                state = Aerial_attack_state_e.NOT_ATTACKING;
                // HAAAAACK (applies gravity so ninja stays grounded)
                Ninja.get().update_movement_velocity(Vector3.zero);
            }

            break;
        }
    }// Update

    //--------------------------------------------------------------------------

    public void start_attack()
    {
        if (is_playing)
        {
            return;
        }

        Ninja.get().stop();

        state = Aerial_attack_state_e.WINDING_UP;
    }// start_attack

    //--------------------------------------------------------------------------

    public void notify_dive_landed()
    {
        if (state != Aerial_attack_state_e.DIVING)
        {
            return;
        }

        state = Aerial_attack_state_e.COOLING_DOWN;
        shockwave.SetActive(true);
        shockwave.GetComponent<Shockwave>().start_shockwave();
    }// notify_dive_landed

    //--------------------------------------------------------------------------

    // Returns true if the step reached the destination.
    bool step_sword_towards(Vector3 target_pos, Quaternion target_rotation)
    {
        sword.transform.position = Vector3.MoveTowards(
            sword.transform.position, target_pos,
            8f * Time.deltaTime);
        var reached_pos = Vector3.Distance(
            sword.transform.position, target_pos) < 0.01f;

        // if (!reached_pos)
        // {
        //     return false;
        // }

        sword.transform.rotation = Quaternion.RotateTowards(
                sword.transform.rotation, target_rotation,
                360f * Time.deltaTime);

        var reached_rotation = Quaternion.Angle(
            sword.transform.rotation,
            target_rotation) < 0.1f;

        return reached_rotation && reached_pos;
    }// step_sword_towards
}

// sword.transform.localEulerAngles
// sword.transform.rotation.eulerAngles
