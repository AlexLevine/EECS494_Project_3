using UnityEngine;
using System.Collections;

public class Fade_in_object : Switchee
{
    public GameObject to_fade_in;
    public float fade_duration = 1.5f;

    private float time_elapsed = 0;
    private bool activated = false;
    private Renderer renderer;
    private Color start_color;
    private Color end_color;

    //--------------------------------------------------------------------------

    void Start()
    {
        renderer = to_fade_in.GetComponent<Renderer>();
        start_color = renderer.material.color;
        end_color = renderer.material.color;
        start_color.a = 0;

        renderer.material.color = start_color;
        to_fade_in.SetActive(false);
    }// Start

    //--------------------------------------------------------------------------

    public override void activate()
    {
        if (activated)
        {
            return;
        }

        activated = true;
        StartCoroutine(fade_in());
    }// activate

    //--------------------------------------------------------------------------

    IEnumerator fade_in()
    {
        var lerp_percent = time_elapsed / fade_duration;
        to_fade_in.SetActive(true);

        while (lerp_percent < 1)
        {
            lerp_percent = time_elapsed / fade_duration;
            if (lerp_percent >= 1)
            {
                lerp_percent = 1;
            }

            renderer.material.color = Color.Lerp(
                start_color, end_color, lerp_percent);

            time_elapsed += Time.deltaTime;
            yield return null;
        }
    }// fade_in
}
