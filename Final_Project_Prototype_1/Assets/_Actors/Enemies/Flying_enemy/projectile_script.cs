using UnityEngine;
using System.Collections;

public class projectile_script : Enemy {

// 	private float speed = 4f;
// 	private int attack_power = 1;
//
// 	//the 2 players that the projectile targets
// 	public string player_name1 = "Llama";
// 	public string player_name2 = "Ninja";
//
//
// 	public GameObject start_location;
//
// 	public Vector3 start;
// 	private Vector3 dest;
//
// 	public Vector3 angle;
//
//
// 	// Use this for initialization
// 	public void Start()
// 	{
//
// 		start = (start_location == null ?
// 		         transform.position : start_location.transform.position);
//
// 		// originalColor = renderer.material.color;
// 		dest = new Vector3 (0, 0, 0);
// 		transform.position = start;
//
// 	}// Start
//
// 	//--------------------------------------------------------------------------
//
// 	public void Awake() {
//
// 		//Basic Movement
// 		Vector3 pos = this.transform.position;
//
// 		Vector3 player1_pos = pos;
// 		Vector3 player2_pos = pos;
//
// 		// if the objects exist, find their postions
// 		if (GameObject.Find(player_name1))
// 		{
// 			player1_pos = GameObject.Find(player_name1).transform.position;
// 		}
// 		if (GameObject.Find(player_name2))
// 		{
// 			player2_pos = GameObject.Find(player_name2).transform.position;
// 		}
//
// 		// set destination point
// 		if (player1_pos == pos && player2_pos != pos) {
// 			dest = player2_pos;
// 		} else if (player2_pos == pos && player1_pos != pos) {
// 			dest = player1_pos;
// 		} else if (player2_pos == pos && player1_pos == pos) {
// 			dest = pos;
// 		} else if (Vector3.Distance (player1_pos, pos) < Vector3.Distance (player2_pos, pos)) {
// 			dest = player1_pos;
// 		} else {
// 			dest = player2_pos;
// 		}
//
// 		angle = dest - pos;
// 		angle.Normalize ();
//
// 	}
//
// 	public void Update()
// 	{
// 		this.GetComponent<Rigidbody>().velocity = angle * speed;
//
// //		if (this.GetComponent<Rigidbody>().velocity != Vector3.zero)
// //		{
// //			transform.rotation = Quaternion.Slerp(
// //				transform.rotation,
// //				Quaternion.LookRotation(this.GetComponent<Rigidbody>().velocity),
// //				Time.deltaTime * speed);
// //		}
// 	}
//
// 	void OnCollisionEnter(Collision c)
// 	{
//
// 		if (c.gameObject.tag == "Player")
// 		{
// 			c.gameObject.GetComponent<Actor>().receive_hit(attack_power);
// 		}
// 		Destroy (gameObject);
// 	}// OnCollisionEnter

}
