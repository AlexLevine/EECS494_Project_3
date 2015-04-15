using UnityEngine;
using System.Collections;

public class Off_screen : MonoBehaviour {
	private Llama llama;
	private Ninja ninja;
	public GameObject llama_mesh;
	public GameObject ninja_mesh;
	private Renderer llama_renderer;
	private Renderer ninja_renderer;
	
	private Vector3 center;
	private float upper_right;
	private float upper_left;
	private float lower_left;
	private float lower_right;
	private Vector3 base_vector;
	
	public float icon_ratio = .9f;
	
	private bool active = true;
	
	public Texture llama_texture;
	public Texture ninja_texture;
	
	private enum Character_e{
		LLAMA,
		NINJA
	}
	
	// Use this for initialization
	void Start () {
		llama = Llama.get();
		ninja = Ninja.get();
		llama_renderer = llama_mesh.GetComponent<Renderer>();
		ninja_renderer = ninja_mesh.GetComponent<Renderer>();
		
		center = new Vector3(Camera.main.pixelWidth/2f,Camera.main.pixelHeight/2f,0);
		base_vector = new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight/2f,0) - center;
		upper_right = angle(base_vector,new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight,0) - center);
		upper_left = angle(base_vector, new Vector3(0,Camera.main.pixelHeight,0)-center);
		lower_left = angle(base_vector, new Vector3(0,0,0)-center);
		lower_right = angle(base_vector,new Vector3(Camera.main.pixelWidth,0,0)-center);
		print(upper_right); print (upper_left); print (lower_left); print (lower_right);
	}
	
	void OnGUI(){
		if (llama_mesh==null || ninja_mesh==null) print("need to set llama and ninja meshes correctly");
		if (!active) return;
		
		
		if (!Camera_follow.point_in_viewport(llama.transform.position)){//&& !llama_renderer.isVisible){
			icon(llama.transform.position,Character_e.LLAMA);
		}
		if (!Camera_follow.point_in_viewport(ninja.transform.position)){ //&& !ninja_renderer.isVisible){
			icon (ninja.transform.position,Character_e.NINJA);
		}
	}
	
	void icon(Vector3 world_pos, Character_e c){
		var screen_pos = Camera.main.WorldToScreenPoint(world_pos);
		var ang = angle(base_vector,screen_pos-center);
		if (screen_pos.x-center.x == 0){print("ZERO SLOPE"); return;}
		var slope = (screen_pos.y-center.y)/(screen_pos.x-center.x);
		//print(screen_pos); return;
		
		//print(Camera.main.transform.forward);
		print(llama.transform.position);print(Camera.main.transform.position);
		
		return;
		
		//get location for icon
		Vector2 icon_location = Vector2.zero;
//		if (ang<=upper_right && ang>lower_right){
//			print ("right");
//			icon_location.x = icon_ratio*Camera.main.pixelWidth;
//			icon_location.y = solve(slope,icon_location.x,false);	
//		}
//		else if (ang<=upper_left && ang>upper_right){
//			print ("top");
//			icon_location.y = icon_ratio*Camera.main.pixelHeight;
//			icon_location.x = solve (slope,icon_location.y,true);
//			
//		}
//		else if (ang<=lower_left || ang>upper_left){ //due to atan2 implementation
//			print ("left");
//			icon_location.x = 0;
//			icon_location.y = solve(slope,icon_location.x,false);
//		}
		if (ang<=lower_right && ang>lower_left){
			print ("bottom");
			icon_location.y = 0;
			icon_location.x = solve (slope,icon_location.y,true);
		}
		else{
			print("GUI ICONS NOT WORKING");
		}
		
		//make gui icon
		//Vector2 gui_location = GUIUtility.ScreenToGUIPoint(icon_location);
		Vector2 gui_location = icon_location;
		GUI.DrawTexture(new Rect(gui_location.x,gui_location.y,30,30),ninja_texture);
	}
	
	//point-slope formula solved for x and y
	float solve(float slope, float determinate, bool solve_for_x){
		if (solve_for_x){
			return (determinate - center.y + slope*center.x)/slope;
		}
		else{
			return slope*(determinate-center.x) + center.y;
		}
	}
	
	
	float angle(Vector3 b_v, Vector3 arm){
		var dot_product = b_v.x*arm.x + b_v.y*arm.y;
		var determinant = b_v.x*arm.y - b_v.y*arm.x;
		return Mathf.Atan2(determinant,dot_product);
	}
	
	
}


