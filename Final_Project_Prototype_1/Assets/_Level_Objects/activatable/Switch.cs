using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Switch : MonoBehaviour
{
    public GameObject switchee;
    public GameObject cut_scene;
    // Use this for initialization
    void Start ()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    //--------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

		if (cut_scene!=null)
        {
			Cut_scene cs = cut_scene.GetComponent<Cut_scene>();
			cs.activate();
		}
		else
        {
			Switchee sw = switchee.GetComponent<Switchee>();
			sw.activate(null, null);
		}

        Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
}
