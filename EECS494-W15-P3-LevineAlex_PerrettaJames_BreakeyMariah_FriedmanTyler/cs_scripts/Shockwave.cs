using UnityEngine;
using System.Collections;

public class Shockwave : MonoBehaviour
{
    private Vector3 original_scale;

    public void start_shockwave()
    {
        original_scale = transform.localScale;
        StartCoroutine(expand());
    }// start_shockwave

    //--------------------------------------------------------------------------

    private IEnumerator expand()
    {
        var start_scale = transform.localScale;
        print("start_scale: " + start_scale);
        var end_scale = Vector3.one * 10f;
        print("end_scale: " + end_scale);

        var time_elapsed = 0f;
        var duration = 0.25f;

        var lerp_percent = time_elapsed / duration;
        while (lerp_percent < 1)
        {
            transform.localScale = Vector3.Lerp(
                start_scale, end_scale, lerp_percent);
            time_elapsed += Time.deltaTime;
            lerp_percent = time_elapsed / duration;
            yield return null;
        }

        transform.localScale = Vector3.Lerp(
            start_scale, end_scale, 1);

        yield return null;

        transform.localScale = original_scale;
        gameObject.SetActive(false);
    }// expand

    //--------------------------------------------------------------------------

    void OnTriggerEnter(Collider c)
    {
        var enemy = c.gameObject.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }

        var knockback_dir =
                c.gameObject.transform.position - transform.position;
        knockback_dir.y = 0;

        enemy.receive_hit(
            Ninja.get_sword().attack_power * 1.5f, knockback_dir, gameObject);
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }// OnTriggerStay
}
