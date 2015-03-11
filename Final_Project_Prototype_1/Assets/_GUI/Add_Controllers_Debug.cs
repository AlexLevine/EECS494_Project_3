using UnityEngine;
using System.Collections;
using InControl;

public class Add_Controllers_Debug : MonoBehaviour {
	public enum players_set_e {
		START, 
		WAITING_FOR_LLAMA,
		WAITING_FOR_NINJA, 
		DONE
	}
	
	public GameObject cont_watch_obj_1, cont_watch_obj_2;
	
	private Controller_Watcher p1, p2; 
	
	players_set_e waiting_for_player; 
	
	void Start()
	{
		var ll = Instantiate (cont_watch_obj_1,this.transform.position,Quaternion.identity) as GameObject;
		ll.name = "Llama";
		var llama = GameObject.Find("Llama");
		llama.transform.position = new Vector3(-3,0,-40);
		p1 = ll.GetComponent<Controller_Watcher>();
		
		var nin = Instantiate (cont_watch_obj_2,this.transform.position,Quaternion.identity) as GameObject;
		nin.name = "Ninja";
		p2 = nin.GetComponent<Controller_Watcher>();
		var ninja = GameObject.Find("Ninja");
		ninja.transform.position = new Vector3(3,0,-40);
		
		waiting_for_player = players_set_e.START; 
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		var status = look_for_input();
		if(status == 0 || status==-1){
			return; 
		}
		
		switch (waiting_for_player){
			case players_set_e.START:
				if (status==1) waiting_for_player = players_set_e.WAITING_FOR_NINJA;
				if (status==2) waiting_for_player = players_set_e.WAITING_FOR_LLAMA;
				break;
			case players_set_e.WAITING_FOR_LLAMA:
				if (status==1) waiting_for_player = players_set_e.DONE;
				break;
			case players_set_e.WAITING_FOR_NINJA:
				if (status==2) waiting_for_player = players_set_e.DONE;
				break;
		}
	}
	
	void OnGUI(){
		if(waiting_for_player == players_set_e.DONE)
		{
			return; 
		}
		GUI.Box(new Rect(0, 0, 220, 25), "Assigning Controllers");    
		
		switch(waiting_for_player){
		case players_set_e.START:
			GUI.Box(new Rect(0, 25, 300, 25), "Press A to be the Llama!  Press B to be the Ninja!");                
			break;
		case players_set_e.WAITING_FOR_LLAMA:
			GUI.Box(new Rect(0, 25, 300, 25), "Waiting for other play to press A");
			break; 
		case players_set_e.WAITING_FOR_NINJA:
			GUI.Box(new Rect(0, 25, 300, 25), "Waiting for other player to press B");
			break; 	
		}
	}
	
	int look_for_input()
	{
		foreach(var device in InputManager.Devices){
			
			if(device.GetControl(InputControlType.Action1)){
				if (p1.device==device || p2.device==device) continue;
				p1.device = device;
				return 1;
			}
			else if(device.GetControl(InputControlType.Action2)){
				if (p1.device==device || p2.device==device) continue;
				p2.device = device;
				return 2;
			}
		}
		return 0; 
	}
}
