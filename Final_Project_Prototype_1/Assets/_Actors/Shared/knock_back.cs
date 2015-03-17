using UnityEngine;
using System.Collections;

public class knock_back : MonoBehaviour {

	public bool is_hit;
	private int hit_timer;
	const int full_hit_timer = 20;
	private Vector3 hit_dir;

	private int knock_back_speed = 5;

	// Use this for initialization
	void Start () {
		is_hit = false;
		hit_timer = full_hit_timer;
	}
	
	// Update is called once per frame
	void Update () {
		if (is_hit && hit_timer >= 0) {
			print ("1: " + this.GetComponent<Rigidbody>().velocity);
			this.GetComponent<Rigidbody>().velocity = hit_dir * knock_back_speed;
			print ("2: " + this.GetComponent<Rigidbody>().velocity);
			--hit_timer;
		} else if (is_hit) {
			is_hit = false;
			hit_timer = full_hit_timer;
		} else return;
	}

	public void on_hit(Vector3 dir) {
		is_hit = true;
		hit_dir = -dir;
		hit_dir.Normalize ();
	}

//	public IEnumerator flashSprite() {
//		for (int i = 0; i < 5; ++i) {
//			renderer.material.color = Color.black;
//			yield return new WaitForSeconds(.1f);
//			renderer.material.color = originalColor;
//			yield return new WaitForSeconds(.1f);
//		}
//		renderer.material.color = originalColor;
//	}
}
