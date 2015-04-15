using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Activate_deactivate_group : MonoBehaviour
{
    public GameObject[] to_activate;
    public GameObject[] to_deactivate;

	// Use this for initialization
	void Start()
    {
        GetComponent<Collider>().isTrigger = true;

        foreach (var obj in to_activate)
        {
            obj.SetActive(false);
        }

        foreach (var obj in to_deactivate)
        {
            obj.SetActive(true);
        }
	}

	// Update is called once per frame
	void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag != "Player")
        {
            return;
        }

        foreach (var obj in to_activate)
        {
            obj.SetActive(true);
        }

        foreach (var obj in to_deactivate)
        {
            obj.SetActive(false);
        }

        Destroy(gameObject);
	}

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }
}
