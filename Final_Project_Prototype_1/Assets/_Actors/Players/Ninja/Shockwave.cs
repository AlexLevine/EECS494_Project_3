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
        while (transform.localScale.x < 6)
        {
            transform.localScale += Vector3.one * 0.7f;
            yield return new WaitForSeconds(0.001f);
        }

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
            Ninja.get_sword().attack_power, knockback_dir, gameObject);
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }// OnTriggerStay
}
