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
    private float max_time = 3f; 

    void Start()
    {
        HUD_gameobj.SetActive(false);
        Input_reader.toggle_player_controls(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(llama_ready && ninja_ready)
        {
            if(timer < max_time)
            {
                timer += Time.deltaTime;
            }
            else
            {
                Destroy(title_screen_parent);  
                HUD_gameobj.SetActive(true);
                Input_reader.toggle_player_controls(true); 
            }
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
