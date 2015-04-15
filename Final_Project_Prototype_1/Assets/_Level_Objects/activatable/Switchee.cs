using UnityEngine;
using System.Collections;

public class Switchee : MonoBehaviour
{
    public delegate void Switchee_callback();

    //--------------------------------------------------------------------------

    private Switchee_callback callback_;
    private float? callback_delay_;

    public virtual void activate(
        Switchee_callback callback, float? callback_delay)
    {
        callback_ = callback;
        callback_delay_ = callback_delay;
    }

    //--------------------------------------------------------------------------

    protected void do_callback()
    {
        if (callback_ == null)
        {
            return;
        }

        if (callback_delay_ == null)
        {
            callback_();
            return;
        }

        StartCoroutine(delayed_callback(callback_, (float) callback_delay_));

    }// do_callback

    //--------------------------------------------------------------------------

    IEnumerator delayed_callback(Switchee_callback cb, float delay)
    {
        yield return new WaitForSeconds(delay);
        cb();
        yield break;
    }// delayed_callback

    //--------------------------------------------------------------------------

}
