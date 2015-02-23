using UnityEngine;
using System.Collections;

public class Llama_fire_breath : MonoBehaviour {

    public float duration{get{return 1f;}}
    private float time_elapsed = 0; 


    void Update()
    {
        time_elapsed += Time.deltaTime;

        if(duration <= time_elapsed)
        {
            gameObject.SetActive(false);
            time_elapsed = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.receive_hit(9001);
        }
    }
}
