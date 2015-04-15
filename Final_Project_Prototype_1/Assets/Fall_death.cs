using UnityEngine;
using System.Collections;

public class Fall_death : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<Player_character>() == null)
        {
            return;
        }

        Checkpoint.load_last_checkpoint();
    }

    void OnTriggerStay(Collider c)
    {
        OnTriggerEnter(c);
    }
}
