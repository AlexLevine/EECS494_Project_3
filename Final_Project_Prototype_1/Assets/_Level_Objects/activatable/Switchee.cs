using UnityEngine;
using System.Collections;

public class Switchee : MonoBehaviour
{
    public delegate void Switchee_callback();

    public virtual void activate(Switchee_callback callback=null)
    {

    }
}
