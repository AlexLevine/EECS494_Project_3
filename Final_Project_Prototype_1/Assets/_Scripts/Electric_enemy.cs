using UnityEngine;
using System.Collections;

public class Electric_enemy : Enemy {

	private float speed = 2f;

	public float offset = 5f;

	//Distance where enemy turns around
	public float leftEdge;
	public float rightEdge;
	private float downEdge;
	private float upEdge;

	public GameObject start_location;

	public Vector3 start;
	
	//private Color originalColor;

	//Chance that the enemy will change directions
	//private float chanceToChangeDirections = 0.001f;

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
			return 3;
		}
	}
	
	public override void on_player_hit()
	{
		base.on_player_hit();
		//Destroy(gameObject, 0.2f);
		
	}

	
	// Use this for initialization
	public override void Start () {
		base.Start();
		
		start = (start_location == null ?
		         transform.position : start_location.transform.position);
		
		// originalColor = renderer.material.color;
		transform.position = start;

		leftEdge = start.x - offset;
		rightEdge = start.x + offset;

		this.rigidbody.velocity = new Vector3 (speed, 0, 0);

		//originalColor = renderer.material.color;
		
	}
	
	public override void Update() {

		base.Update();
		//Basic Movement
		Vector3 pos = transform.position;

//		if (renderer.material.color == originalColor) renderer.material.color = Color.white;
//		else renderer.material.color = originalColor;
	
//		if (Random.value < chanceToChangeDirections){
//			peo.vel = new Vector2 (speed * rand_speed, peo.vel.y);
//		}
//		else if (Random.value < (chanceToChangeDirections * 2)) { // the 2 makes up for the fact that this
//			// statement gets called half as much as the one above
//			peo.vel = new Vector2 (peo.vel.x, speed * rand_speed);
//		}
		
		
		
		if (pos.x < leftEdge) {
			//this.rigidbody.velocity = new Vector3 (speed, 0, 0);
			speed *= -1f;
		} else if (pos.x > rightEdge) {
			//this.rigidbody.velocity = new Vector3 (-speed, 0, 0);
			speed *= -1f;
		}

//		if (pos.y > upEdge) {
//			peo.vel = new Vector2 (peo.vel.x, -speed);
//		}
//		if (pos.y < downEdge) {
//			peo.vel = new Vector2 (peo.vel.x, speed);
//		}
		
			this.rigidbody.velocity = new Vector3 (speed, 0, 0);
	}
}
