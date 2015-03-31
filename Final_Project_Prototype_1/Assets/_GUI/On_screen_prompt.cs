using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class On_screen_prompt : MonoBehaviour {
    public  GameObject prompt_prefab;
    private GameObject instantiated_prompt;  


    public float max_time_on_screen = 5; 

    private bool  instantiated; 
    private float cur_time; 
    private float prompt_location_x, 
                  prompt_location_y; 

    void Start()
    {
        cur_time = 0; 
        instantiated = false; 
        // top 25% of the screen, in the middle
        prompt_location_x = (float)(Screen.width * .5);
        prompt_location_y = (float)(Screen.height * .9);
    }

    // Update is called once per frame
    void Update ()
    {
        if(!instantiated)
        {
            return; 
        }

        update_time(); 
    }

    void OnTriggerEnter(Collider other){
        if(!other.name.Contains("Llama") && !other.name.Contains("Ninja"))
        {
            return; 
        }
        if(instantiated)
        {
            return; 
        }

        instantiate_object();
        instantiated = true; 
    }

    void instantiate_object()
    {
        if(instantiated)
        {
            return;
        }

        instantiated_prompt = Instantiate(prompt_prefab,
                           new Vector3(prompt_location_x, prompt_location_y, 0),
                           Quaternion.identity) as GameObject;

        var canvas_obj = GameObject.Find("Canvas"); 
        
        if(canvas_obj == null)
        {
            Destroy(instantiated_prompt);
            Destroy(gameObject);

            print("what is even happen");
        }
        instantiated_prompt.transform.SetParent(canvas_obj.gameObject.transform, 
                                                true);
        
    }

    void update_time()
    {
        cur_time += Time.deltaTime; 
        if(cur_time >= max_time_on_screen)
        {
            Destroy(instantiated_prompt);
            Destroy(gameObject);
        }
    }
}
