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


    private float timer = 0f; 
    private float max_time = 3f; 



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
                //Application.LoadLevel(1);
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
