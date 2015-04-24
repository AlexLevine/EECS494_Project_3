using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;

public class Title_screen_controller : MonoBehaviour
{
    bool llama_ready = false;
    bool ninja_ready = false;

    public Sprite ready_image;

    public GameObject llama_prompt;
    public GameObject ninja_prompt;

    public GameObject title_screen_parent;
    public GameObject HUD_gameobj;

    private float timer = 0f;
    private float max_time = 1.5f;

    void Start()
    {
        HUD_gameobj.SetActive(false);
        Actor.actors_paused = true;
        // Input_reader.toggle_player_controls(false);
    }

    // Update is called once per frame
    void Update()
    {
        var llama_device = Llama.get().GetComponent<Input_reader>().input_device;
        if (llama_device != null &&
            llama_device.GetControl(InputControlType.Action1).WasPressed)
        {
            setLlama();
        }

        var ninja_device = Ninja.get().GetComponent<Input_reader>().input_device;
        if (ninja_device != null &&
            ninja_device.GetControl(InputControlType.Action1).WasPressed)
        {
            setNinja();
        }

        if(!(llama_ready && ninja_ready))
        {
            return;
        }
        if(timer < max_time)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(title_screen_parent);
            HUD_gameobj.SetActive(true);
            // Input_reader.toggle_player_controls(true);
            Actor.actors_paused = false;
        }
    }

    public void setLlama()
    {
        llama_ready = true;
        llama_prompt.GetComponent<Image>().sprite = ready_image;
    }

    public void setNinja()
    {
        ninja_prompt.GetComponent<Image>().sprite = ready_image;
        ninja_ready = true;
    }
}
