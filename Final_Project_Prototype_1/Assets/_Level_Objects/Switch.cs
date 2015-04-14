using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{
    public GameObject switchee;
    public GameObject cut_scene;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    // void FixedUpdate () {
    //     Renderer r = this.GetComponent<Renderer>();
    //     Switchee sw = switchee.GetComponent<Switchee>();
    //     if (sw.on) r.material = sw.switch_on_material;
    //     else r.material = sw.switch_off_material;
    // }

    void OnTriggerEnter(Collider other)
    {
		if (cut_scene!=null)
        {
			Cut_scene cs = cut_scene.GetComponent<Cut_scene>();
			cs.activate();
		}
		else
        {
			Switchee sw = switchee.GetComponent<Switchee>();
			sw.activate();
		}

        Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (cut_scene==null)
        {
			Switchee sw = switchee.GetComponent<Switchee>();
			sw.activate();
        }

        Destroy(gameObject);
    }
}
