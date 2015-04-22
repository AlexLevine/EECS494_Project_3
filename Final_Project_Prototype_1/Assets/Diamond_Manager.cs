using UnityEngine;
using System.Collections;

public class Diamond_Manager : Switchee {

    public bool running = false; 
    public float lerp_duration = 5f; 
    public float time_elapsed = 0; 
    public GameObject target_position; 
    private Vector3 start_pos; 

    public GameObject youre_winner_canvas; 
    

    public override void activate(
        Switchee_callback callback, float? callback_delay)
    {
        base.activate(callback, callback_delay); 

        StartCoroutine(lerp_diamond());  
    }

    void Start()
    {
        start_pos = transform.position; 
        youre_winner_canvas.SetActive(false);
        // StartCoroutine(lerp_diamond());  
    }

    void Update()
    {
        rotate_around_self(); 
    }


    IEnumerator lerp_diamond()
    {
        var lerp_percent = time_elapsed / lerp_duration;
        while (time_elapsed < lerp_duration)
        {
            yield return null;
            time_elapsed += Time.deltaTime;
            lerp_percent = time_elapsed / lerp_duration;

            transform.position = Vector3.Lerp(start_pos,
                target_position.transform.position, lerp_percent);
        }

        do_callback(); 
    }


    private void rotate_around_self()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 20f);
    }


    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        youre_winner_canvas.SetActive(true);
    }

}
