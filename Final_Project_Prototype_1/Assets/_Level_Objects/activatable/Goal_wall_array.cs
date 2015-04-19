using UnityEngine;
using System.Collections;

public class Goal_wall_array : MonoBehaviour {

	public GameObject wall1;
	public GameObject wall2;
	public GameObject wall3;
	
	private Queue walls;

	// Use this for initialization
	void Start () {
		walls.Enqueue(wall1);
		walls.Enqueue(wall2);
		walls.Enqueue(wall3);
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public GameObject get_next_wall() {
		if (walls.Count != 0) {
			return (GameObject) walls.Dequeue();
		} else {
			// error
			print("Error with switch - no walls left to disappear");
			return null;
		}
	}
}
