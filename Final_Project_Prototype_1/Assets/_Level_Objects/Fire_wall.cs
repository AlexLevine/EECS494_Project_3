using UnityEngine;
using System.Collections;

public class Fire_wall : MonoBehaviour, Switchee
{
    public void activate()
    {
        gameObject.SetActive(false);
    }
}
