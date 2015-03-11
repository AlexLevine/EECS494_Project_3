using UnityEngine;
using System.Collections;
using System;

public class Flash_animation : MonoBehaviour
{
    // private Color originalColor;

    public bool is_playing = false;

    // Use this for initialization
    void Start()
    {
        // originalColor = GetComponent<Renderer>().material.color;

    }// Start

    //--------------------------------------------------------------------------

    public void start_animation()
    {
        is_playing = true;
        StartCoroutine (flashSprite ());
    }

    //--------------------------------------------------------------------------

    private IEnumerator flashSprite()
    {
        yield return new WaitForSeconds(1f);
        is_playing = false;
        // foreach(var renderer in GetComponentsInChildren<MeshRenderer>())
        // {
        //     for (int i = 0; i < 5; ++i)
        //     {
        //         renderer.material.color = Color.red;
        //         yield return new WaitForSeconds(.1f);
        //         renderer.material.color = originalColor;
        //         yield return new WaitForSeconds(.1f);
        //     }
        //     renderer.material.color = originalColor;
        //     // break;
        // }
    }
}
