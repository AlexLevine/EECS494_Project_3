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
