using UnityEngine;
using System.Collections;
using InControl;


public class Llama : Player_character
{
    public GameObject spit_prefab;
    private bool throw_ninja = false;
    private float rotation_count;
    private const float rotation_wait = .5f;
    private Vector3 rotation_point;
    private float amount_rotated;
    private float rotation_angle = Mathf.PI/2;

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
	
	void FixedUpdate(){
		if (throw_ninja){
		
			//rotate to throw
			var rotate = transform.forward;
			var temp = rotate.z;
			rotate.z = rotate.x;
			rotate.x = -temp;
			if (rotation_count<=rotation_wait){
				transform.RotateAround(transform.position,rotate,-rotation_angle);
				rotation_count+=Time.fixedDeltaTime;
				amount_rotated+=rotation_angle;
	         }
	         else{
				transform.RotateAround(transform.position,rotate,amount_rotated);
				throw_ninja = false;
				var ninja = GameObject.Find("Ninja");
				teamed_up = false;
				
				ninja.GetComponent<Player_character>().teamed_up = false;
				var new_ninja_velocity = transform.forward + transform.up;//Vector3.one;// transform.forward;
				
				print("new vel: " + new_ninja_velocity);
				new_ninja_velocity.x *= 5;
				new_ninja_velocity.y *= 10;
				new_ninja_velocity.z *= 5;
				print("new vel: " + new_ninja_velocity);
				ninja.GetComponent<Rigidbody>().velocity = new_ninja_velocity;    
			}
		}
	}
    //--------------------------------------------------------------------------

    public override void jump()
    {
        if(!on_ground)
        {
            return;
        }

        base.jump();
    }
    
	public override void run(Vector3 tilt, bool sprint)
    {
    	if (throw_ninja) return;
    	base.run(tilt,sprint);
    }
    

    public override void adjust_jousting_pole(float vertical_tilt, float horizontal_tilt)
    {return;}

    public override void toggle_jousting_pole()
    {return;}

    //--------------------------------------------------------------------------

    public override void team_up_engage_or_throw()
    {
        if (!teamed_up)
        {
            return;
        }

        if (!throw_ninja){
	        throw_ninja = true;
			rotation_count = 0;
			//rotation_point = transform.position;
			amount_rotated = 0;
		}
    }// team_up_engage_or_throw

    //--------------------------------------------------------------------------

    public override void projectile_attack()
    {
        var projectile_start_pos = transform.position;
        projectile_start_pos += transform.forward * 1f;
        projectile_start_pos.y += 1f;

        GameObject spit = Instantiate(
            spit_prefab, projectile_start_pos,
            transform.rotation) as GameObject;
        spit.GetComponent<Rigidbody>().velocity = transform.forward * 12;
    }// projectile_attack

    //--------------------------------------------------------------------------

    public override void physical_attack()
    {
    }// physical_attack

    //--------------------------------------------------------------------------

    public override int max_health
    {
        get
        {
            return 100;
        }
    }// max_health

    //--------------------------------------------------------------------------

    public override int run_speed
    {
        get
        {
            return 5;
        }
    }// run_speed

    //--------------------------------------------------------------------------

    public override int sprint_speed
    {
        get
        {
            return 10;
        }
    }// sprint_speed

    //--------------------------------------------------------------------------

    public override int jump_speed
    {
        get
        {
            return 7;
        }
    }// jump_speed
}

