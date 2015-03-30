using UnityEngine;
using System.Collections;

public class projectile2_script : Enemy {

	public float speed = 7f;
	public Vector3 angle = new Vector3(1, 0, 0);
	private Vector3 knockback = new Vector3(0, 0, 0);
	
	public override int attack_power
	{
		get
		{
			return 1;
		}
	}
	
	public override int max_health
	{
		get
		{
			return 1;
		}
	}

	// Use this for initialization
	void Start () {
		var llama_pos = Llama.get().transform.position;
		var ninja_pos = Ninja.get().transform.position;
		
		var llama_distance = Vector3.Distance(llama_pos, transform.position);
		var ninja_distance = Vector3.Distance(ninja_pos, transform.position);
		
		if (llama_distance < ninja_distance) {
			// attack llama:
			angle = llama_pos - this.transform.position;
		} else {
			// attack ninja
			angle = ninja_pos - this.transform.position;
		}
		
		angle.Normalize ();
		
	
		this.GetComponent<Rigidbody>().velocity = angle * speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision c)
	{
		
		if (c.gameObject.tag == "Player")
		{
			c.gameObject.GetComponent<Actor>().receive_hit(attack_power, knockback, this.gameObject);
		}
		Destroy (gameObject);
	}// OnCollisionEnter
	
	
}
