using UnityEngine;
using System.Collections;

public class Switchee : MonoBehaviour {
    public bool on;
    public Material switch_on_material;
    public Material switch_off_material;
    // Use this for initialization
    void Awake () {
        on = false;
    }

    public void toggle()
    {
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            print("deactivated");
        }
        else
        {
            gameObject.SetActive(true);
            print("activated");
        }
    }

}
