using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Lock_on_health_bar : MonoBehaviour {
    public GameObject target;

    // Update is called once per frame
    void FixedUpdate () {
        if(target == null)
        {
            gameObject.SetActive(false);
            return; 
        }

        update_slider(); 

        Vector3 screen_point = Camera.main.WorldToViewportPoint(target.transform.position);

        float x_pos = screen_point.x * Screen.width; 
        float y_pos = (screen_point.y * Screen.height) - 20; 

        transform.position = new Vector3(x_pos, y_pos, 0);
    }


    void update_slider() 
    {
        float health_percentage = (float)target.GetComponent<Actor>().health /
                                  (float)target.GetComponent<Actor>().max_health; 

        this.GetComponent<Slider>().normalizedValue = health_percentage; 
    }
}
