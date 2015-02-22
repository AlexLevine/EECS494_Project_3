using UnityEngine;
using System.Collections;
using System;

public class Flash_animation : MonoBehaviour
{
    private Color originalColor;

    // Use this for initialization
    void Start()
    {
        originalColor = renderer.material.color;

    }// Start

    //--------------------------------------------------------------------------

    public void start_animation()
    {
        // StartCoroutine (flashSprite ());
    }

    //--------------------------------------------------------------------------

    private IEnumerator flashSprite() 
    {
        foreach(var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            for (int i = 0; i < 5; ++i) 
            {
                renderer.material.color = Color.red;
                yield return new WaitForSeconds(.1f);
                renderer.material.color = originalColor;
                yield return new WaitForSeconds(.1f);
            }
            renderer.material.color = originalColor;    
            // break;
        }
    }
}