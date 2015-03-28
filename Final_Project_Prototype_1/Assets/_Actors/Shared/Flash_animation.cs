using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
        // yield return new WaitForSeconds(0.5f);
        // is_playing = false;

        var renderers = new List<MeshRenderer>(
            GetComponentsInChildren<MeshRenderer>());
        renderers.AddRange(
            new List<MeshRenderer>(GetComponents<MeshRenderer>()));

        var main_renderer = GetComponent<MeshRenderer>();
        if (main_renderer != null)
        {
            renderers.Add(main_renderer);
        }

        for (int i = 0; i < 10; ++i)
        {
            foreach(var renderer in renderers)
            {
                renderer.enabled = !renderer.enabled;
                // var original_color = renderer.material.color;
                // renderer.material.color = Color.red;
                // yield return new WaitForSeconds(.05f);
                // renderer.material.color = original_color;
                // yield return new WaitForSeconds(.05f);
            }
            yield return new WaitForSeconds(0.05f);
            // GetComponent<Renderer>().enabled = true;
            // yield return new WaitForSeconds(0.1f);
            // renderer.material.color = original_color;
            // break;
        }

        is_playing = false;

    }
}
