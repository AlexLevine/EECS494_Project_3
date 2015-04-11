using UnityEngine;
using System.Collections;

public class Boss_fight_controller : MonoBehaviour
{
    public enum Boss_fight_state_e
    {
        NOT_STARTED,
        OPENING_CUTSCENE,
        FIGHTERS_CHARGING,
        FIGHTERS_RETREATING,
        PREPARING_FOR_NEXT_CHARGE
    }

    //--------------------------------------------------------------------------

    public static Boss_fight_controller get()
    {
        return instance;
    }// get

    //--------------------------------------------------------------------------

    public GameObject first_retreat_point;
    public GameObject second_retreat_point;

    //--------------------------------------------------------------------------

    private static Boss_fight_controller instance;

    private Boss_fight_state_e state;
    private Vector3 player_retreat_point;
    private Vector3 boss_retreat_point;

    //--------------------------------------------------------------------------

    public void start_fight()
    {
        // start cutscene
        state = Boss_fight_state_e.OPENING_CUTSCENE;

        Ninja.get().team_up_engage_or_throw();
        Player_character.force_team_up = true;

        // for now just skip to the charging, eventually give a callback to the
        // cutscene
        state = Boss_fight_state_e.FIGHTERS_CHARGING;
    }// start_fight

    //--------------------------------------------------------------------------

    public void notify_fighters_passed()
    {
        state = Boss_fight_state_e.FIGHTERS_RETREATING;
    }// notify_fighters_passed

    //--------------------------------------------------------------------------

    void Awake()
    {
        instance = this;
    }// Awake

    //--------------------------------------------------------------------------

    // Update is called once per frame
    void Update ()
    {
        switch (state)
        {
        case Boss_fight_state_e.NOT_STARTED:
            break;

        case Boss_fight_state_e.OPENING_CUTSCENE:
            break;

        case Boss_fight_state_e.FIGHTERS_CHARGING:
            break;

        case Boss_fight_state_e.FIGHTERS_RETREATING:
            // move players toward retreat points
            // if both have reached retreat points, swap retreat points
            // and set to prep charge state
            break;

        case Boss_fight_state_e.PREPARING_FOR_NEXT_CHARGE:
            // rotate camera
            break;

        }

    }
}
