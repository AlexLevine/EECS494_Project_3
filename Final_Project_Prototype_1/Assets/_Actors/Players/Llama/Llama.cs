using UnityEngine;
using System.Collections;

public class Llama : Player_character
{
    public GameObject spit_prefab;
    public GameObject spit_spawn_point;

    private static Llama instance;

    //--------------------------------------------------------------------------

    public static Llama get()
    {
        return instance;
    }// get

    //--------------------------------------------------------------------------

    public override void Awake()
    {
        base.Awake();

        instance = this;
    }// Awake

    //--------------------------------------------------------------------------

    // // Use this for initialization
    // public override void Start()
    // {
    //     base.Start();
    // }// Start

    //--------------------------------------------------------------------------

    // // Update is called once per frame
//    public override void Update()
//    {
//		base.Update();
//	}// Update

	// void FixedUpdate(){
	// 	if (throw_ninja){

	// 		//rotate to throw
	// 		var rotate = transform.forward;
	// 		var temp = rotate.z;
	// 		rotate.z = rotate.x;
	// 		rotate.x = -temp;
	// 		if (rotation_count<=rotation_wait){
	// 			transform.RotateAround(transform.position,rotate,-rotation_angle);
	// 			rotation_count+=Time.fixedDeltaTime;
	// 			amount_rotated+=rotation_angle;
	//          }
	//          else{
	// 			transform.RotateAround(transform.position,rotate,amount_rotated);
	// 			throw_ninja = false;
	// 			var ninja = Ninja.get();

 //                team_up_disengage();

	// 			var new_ninja_velocity = transform.forward + transform.up;//Vector3.one;// transform.forward;

	// 			print("new vel: " + new_ninja_velocity);
	// 			new_ninja_velocity.x *= 5;
	// 			new_ninja_velocity.y *= 10;
	// 			new_ninja_velocity.z *= 5;
	// 			print("new vel: " + new_ninja_velocity);
	// 			ninja.velocity = new_ninja_velocity;
	// 		}
	// 	}
	// }
    //--------------------------------------------------------------------------

    // public override void jump()
    // {
    //     base.jump();
    // }

	public override void move(Vector3 delta_position)
    {
    	if (gameObject.GetComponent<Throw_animation>().is_playing)
        {
            return;
        }

    	base.move(delta_position);
    }// move

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        // if (!is_teamed_up)
        // {
        //     return;
        // }

        gameObject.GetComponent<Throw_animation>().start_animation();
  //       if (!throw_ninja){
	 //        throw_ninja = true;
		// 	rotation_count = 0;
		// 	//rotation_point = transform.position;
		// 	amount_rotated = 0;
		// }
    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    public override void attack()
    {
        GameObject spit = Instantiate(
            spit_prefab, spit_spawn_point.transform.position,
            transform.rotation) as GameObject;
        var direction = (is_locked_on ?
            (lock_on_target_pos - spit.transform.position).normalized :
            transform.forward);
        print(direction);

        spit.GetComponent<Rigidbody>().velocity = direction * 14f;
    }// projectile_attack

    //--------------------------------------------------------------------------

    // public override void physical_attack()
    // {
    // }// physical_attack

    //--------------------------------------------------------------------------

    public override int max_health
    {
        get
        {
            return 100;
        }
    }// max_health

    //--------------------------------------------------------------------------

    public override float run_speed
    {
        get
        {
            return 5;
        }
    }// run_speed

    //--------------------------------------------------------------------------

    public override float sprint_speed
    {
        get
        {
            return 10;
        }
    }// sprint_speed

    //--------------------------------------------------------------------------

    public override float jump_speed
    {
        get
        {
            return 15;
        }
    }// jump_speed
}

