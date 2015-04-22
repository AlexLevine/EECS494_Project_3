using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade_screen : MonoBehaviour
{
    public delegate void Fade_screen_callback();

    public GameObject black_panel;

    // private

    private static Fade_screen instance;
    private Image image;

    public static Fade_screen get()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        image = black_panel.GetComponent<Image>();
        var start_color = Color.black;
        start_color.a = 0;
        image.color = start_color;
    }

    public void fade_to_black(Fade_screen_callback callback=null)
    {
        print("fade_to_black");
        StartCoroutine(fade_screen(true, callback));
    }

    public void fade_from_black(Fade_screen_callback callback=null)
    {
        StartCoroutine(fade_screen(false, callback));
    }

    IEnumerator fade_screen(
        bool fade_to_black, Fade_screen_callback callback)
    {
        print("fade_screen");
        var start_color = image.color;
        start_color.a = (fade_to_black ? 0 : 1);
        var end_color = image.color;
        end_color.a = (fade_to_black ? 1 : 0);

        var fade_duration = 0.5f;
        var time_elapsed = 0f;

        var lerp_percent = time_elapsed / fade_duration;
        while (lerp_percent < 1)
        {
            image.color = Color.Lerp(
                start_color, end_color, lerp_percent);

            yield return null;
            time_elapsed += Time.deltaTime;
            lerp_percent = time_elapsed / fade_duration;

            if (lerp_percent > 1)
            {
                lerp_percent = 1;
            }
        }

        image.color = Color.Lerp(
            start_color, end_color, lerp_percent);

        if (callback != null)
        {
            callback();
        }
    }
}
