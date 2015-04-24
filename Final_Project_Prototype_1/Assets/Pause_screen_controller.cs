using UnityEngine;
using System;
using InControl;

public class Pause_screen_controller : MonoBehaviour {

    private static Pause_screen_controller instance; 

    public static Pause_screen_controller get()
    {
        return instance;
    }// get

    void Awake()
    {
        instance = this; 
    }

    // Use this for initialization
    void Start () 
    {
        gameObject.SetActive(false);
    }
}
