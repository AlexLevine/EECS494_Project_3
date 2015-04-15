using UnityEngine;
using System.Collections;

public class Cut_scene : MonoBehaviour
{
    public delegate void Cut_scene_callback();

    //--------------------------------------------------------------------------

	public virtual void activate(Cut_scene_callback callback=null)
	{

	}

    //--------------------------------------------------------------------------

	public void pause_all()
	{
		Camera_follow.stop_following_player();
		Input_reader.toggle_player_controls();
		Actor.actors_paused = true;
		Player_character.drop_lock_on_targets();
	}// pause_all

    //--------------------------------------------------------------------------

	public void unpause_all()
	{
		Camera_follow.start_following_player();
		Input_reader.toggle_player_controls();
		Actor.actors_paused = false;
	}// unpause_all
}
