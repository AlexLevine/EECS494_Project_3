using UnityEngine;
using System.Collections;

public class Start_boss_battle_trigger : MonoBehaviour
{

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag != "Player")
        {
            return;
        }

        Boss_fight_controller.get().start_fight();
    }// OnTriggerEnter

    //--------------------------------------------------------------------------

}
