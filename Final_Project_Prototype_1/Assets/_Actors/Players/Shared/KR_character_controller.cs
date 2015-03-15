using UnityEngine;
using System.Collections;
using Rewired;

[RequireComponent(typeof(Rigidbody))]
public class KR_character_controller : MonoBehaviour {

    public int playerId = 0; // The Rewired player id of this character

    public float moveSpeed = 5.0f;
    public float jump_speed = 15f;
    public float min_move_distance = 0.001f;
    public float skin_width = 0.01f;
    public float gravity = -25f;

    private Player player; // The Rewired Player
    private Rigidbody kr;
    // private bool is_jumping = false;
    private float y_velocity = 0;

    private bool is_grounded;

    // Contains bit fields indicating which sides of the character were last collided with.
    // public int collisionFlags;

    void Awake() {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        // Get the character controller
        kr = GetComponent<Rigidbody>();
    }

    void Update () {
        if (is_grounded)
        {
            y_velocity = 0;
        }

        // GetInput();
        ProcessInput();
        // move();
    }

    // void FixedUpdate()
    // {
    //     move();
    // }

    // private Vector3 GetInput() {
    //     // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
    //     // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.
    // }// GetInput

    //--------------------------------------------------------------------------

    private void ProcessInput() {
        Vector3 tilt = Vector3.zero;
        tilt.x = player.GetAxis("move_left_right"); // get input by name or action id
        tilt.z = player.GetAxis("move_front_back");

        if(player.GetButtonDown("jump") && is_grounded)
        {
            print("jump!");
            // is_jumping = true;
            y_velocity = jump_speed;
            is_grounded = false;
        }
        else
        {
            print("current y vel: " + y_velocity);
            y_velocity += gravity * Time.deltaTime;
        }

        var delta_position = tilt * moveSpeed;
        delta_position.y = y_velocity;
        delta_position *= Time.deltaTime;

        move(delta_position);

        // var sphere_collider = GetComponent<SphereCollider();
        // if (sphere_collider == null)
        // {
            // RaycastHit hit_info;
            // var sphere_hit = Physics.SphereCast(GetComponent<SpereCollider>(),
            //     sphere_collider., Vector3 direction, out hit_info, float maxDistance = Mathf.Infinity
        // }

        // kr.MovePosition(moveVector * moveSpeed * Time.deltaTime);

        // }

        // Process jump

    }// ProcessInput

    //--------------------------------------------------------------------------

    private void move(Vector3 amount)
    {
        // print(amount);
        step_axis_direction(Vector3.right, amount.x);
        print("amount.y: " + amount.y);
        var y_collision = step_axis_direction(Vector3.up, amount.y);
        step_axis_direction(Vector3.forward, amount.z);

        if (amount.y < 0)
        {
            // is_jumping = false;
            is_grounded = y_collision;
        }
    }// move

    //--------------------------------------------------------------------------

    private bool step_axis_direction(Vector3 direction, float step_amount)
    {
        if (Mathf.Abs(step_amount) < min_move_distance)
        {
            print("too slow! " + Mathf.Abs(step_amount));
            return false;
        }

        var move_increment = direction * step_amount;
        RaycastHit hit_info;
        var hit = kr.SweepTest(
            move_increment, out hit_info, move_increment.magnitude + skin_width);
        if (hit)
        {
            move_increment = move_increment.normalized * Mathf.Max(hit_info.distance - skin_width, 0);
        }

        print("move_increment.y: " + move_increment.y);
        transform.position += move_increment;

        return hit;
    }// step_axis_direction

}

