using UnityEngine;
using System.Collections;

public class Goal_wall_array : MonoBehaviour {

	public Cut_scene wall1_cut_scene;
	public Cut_scene wall2_cut_scene;
	public Cut_scene wall3_cut_scene;
	
	private Queue walls;

	// Use this for initialization
	void Start () {
		walls = new Queue();
		walls.Enqueue(wall1_cut_scene);
		walls.Enqueue(wall2_cut_scene);
		walls.Enqueue(wall3_cut_scene);
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public Cut_scene get_next_wall() {
		if (walls.Count != 0) {
			return (Cut_scene) walls.Dequeue();
		} else {
			// error
			print("Error with switch - no walls left to disappear");
			return null;
		}
	}
}
