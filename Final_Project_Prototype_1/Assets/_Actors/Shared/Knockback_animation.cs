using UnityEngine;
using System.Collections;

public class Knockback_animation : MonoBehaviour
{
    public bool is_playing { get { return is_playing_ || die_when_finished_; } }

    private bool is_playing_;
    private float time_elapsed;
    private static float max_knockback_duration = 0.5f;

    private bool die_when_finished_;

    private Vector3 knockback_velocity_;

    private Actor actor;

    // Use this for initialization
    void Start()
    {
        actor = gameObject.GetComponent<Actor>();
    }

    void FixedUpdate()
    {
        if (!is_playing)
        {
            return;
        }

        var pc = GetComponent<Player_character>();
        if (pc != null)
        {
            pc.stop();
        }

        if (knockback_velocity_.magnitude != 0)
        {
            actor.move(knockback_velocity_ * Time.fixedDeltaTime);
        }

        time_elapsed += Time.fixedDeltaTime;

        if (time_elapsed <= max_knockback_duration)
        {
            return;
        }

        is_playing_ = false;
        time_elapsed = 0;

        if (die_when_finished_)
        {
            die_when_finished_ = false;
            actor.on_death();
        }

    }// FixedUpdate

    //--------------------------------------------------------------------------

    public void apply_knockback(
        Vector3 knockback_velocity, bool die_when_finished)
    {
        is_playing_ = true;
        knockback_velocity_ = knockback_velocity;
        die_when_finished_ = die_when_finished;
    }// apply_knockback
}
