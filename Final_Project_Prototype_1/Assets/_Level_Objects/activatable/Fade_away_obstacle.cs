using UnityEngine;
using System.Collections;

public class Fade_away_obstacle : Switchee
{
    public float fade_duration = 1.5f;

    private float time_elapsed = 0;
    private bool activated = false;

    private Switchee_callback callback_;

    //--------------------------------------------------------------------------

    public override void activate(Switchee_callback callback=null)
    {
        if (activated)
        {
            return;
        }

        activated = true;
        callback_ = callback;
        StartCoroutine(fade_away());
    }// activate

    //--------------------------------------------------------------------------

    IEnumerator fade_away()
    {
        var renderer = GetComponent<Renderer>();
        var lerp_percent = time_elapsed / fade_duration;
        var start_color = renderer.material.color;
        var end_color = renderer.material.color;
        end_color.a = 0;

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

        if (callback_ != null)
        {
            callback_();
        }
        gameObject.SetActive(false);

    }// fade_away

    //--------------------------------------------------------------------------

}
