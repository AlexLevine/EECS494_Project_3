// MyCharacter.cs - A simple example showing how to get input from Rewired.Player

using UnityEngine;
using System.Collections;
using Rewired;

[RequireComponent(typeof(CharacterController))]
public class Player_character : MonoBehaviour {

    public int playerId = 0; // The Rewired player id of this character

    public float moveSpeed = 10.0f;
    public float bulletSpeed = 15.0f;
    public GameObject bulletPrefab;

    private Player player; // The Rewired Player
    private CharacterController cc;
    private Vector3 moveVector;
    private bool jump;
    private float y_velocity = 0;

    void Awake() {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        // Get the character controller
        cc = GetComponent<CharacterController>();
    }

    void Update () {
        if (cc.isGrounded)
        {
            y_velocity = 0;
        }

        GetInput();
        ProcessInput();
    }

    private void GetInput() {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        moveVector.x = player.GetAxis("move_left_right"); // get input by name or action id
        moveVector.z = player.GetAxis("move_front_back");
        // print(moveVector);
        jump = player.GetButtonDown("jump");
    }

    private void ProcessInput() {
        // Process movement
        // if(moveVector.x != 0.0f || moveVector.z != 0.0f) {
        if(jump && cc.isGrounded)
        {
            y_velocity = 10;
        }
        else
        {
            y_velocity += -40f * Time.deltaTime;
        }

        moveVector.y = y_velocity * Time.deltaTime;

        // var sphere_collider = GetComponent<SphereCollider();
        // if (sphere_collider == null)
        // {
        //     RaycastHit hit_info;
        //     var sphere_hit = Physics.SphereCast(GetComponent<SpereCollider>(),
        //         sphere_collider., Vector3 direction, out hit_info, float maxDistance = Mathf.Infinity
        // }

        cc.Move(moveVector * moveSpeed * Time.deltaTime);

        // }

        // Process jump

    }
}
