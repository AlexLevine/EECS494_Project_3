using UnityEngine;
using System.Collections;

public class Boss_fight_controller : MonoBehaviour, Checkpoint_load_subscriber
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

    private static float pause_duration = 2f;
    private float time_paused = 0;

    //--------------------------------------------------------------------------

    public void start_fight()
    {
        if (state != Boss_fight_state_e.NOT_STARTED)
        {
            return;
        }

        // start cutscene
        state = Boss_fight_state_e.OPENING_CUTSCENE;

        Ninja.get().team_up_engage_or_throw();
        Player_character.force_team_up = true;

        // for now just skip to the charging, eventually give a callback to the
        // cutscene
        state = Boss_fight_state_e.FIGHTERS_CHARGING;

        boss_retreat_point = second_retreat_point.transform.position;
        player_retreat_point = first_retreat_point.transform.position;
        // Llama.get().move(Vector3.down, false); // snap llama to ground
        player_retreat_point.y = Llama.get().transform.position.y;
    }// start_fight

    //--------------------------------------------------------------------------

    public void notify_fighters_passed()
    {
        state = Boss_fight_state_e.FIGHTERS_RETREATING;

        Llama.get().stop();
        Ninja.get().stop();

        Input_reader.toggle_player_controls();

        Samurai_Attack.get().retreat(boss_retreat_point);
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
            Samurai_Attack.get().start_charge();
            break;

        case Boss_fight_state_e.FIGHTERS_RETREATING:
            Llama.get().look_toward(player_retreat_point);
            var pos_step = Llama.get().run_speed * 1.5f * Time.deltaTime;

            var desired_position = Vector3.MoveTowards(
                Llama.get().transform.position, player_retreat_point, pos_step);
            var adjusted_step = desired_position - Llama.get().transform.position;
            Llama.get().move(adjusted_step, false);

            var reached_retreat_point = Vector3.Distance(
                Llama.get().transform.position, player_retreat_point) < 0.1f;
            if (reached_retreat_point)
            {
                state = Boss_fight_state_e.PREPARING_FOR_NEXT_CHARGE;
            }
            break;

        case Boss_fight_state_e.PREPARING_FOR_NEXT_CHARGE:
            Llama.get().look_toward(Samurai_Attack.get().gameObject);
            time_paused += Time.deltaTime;
            if (time_paused < pause_duration)
            {
                return;
            }

            Input_reader.toggle_player_controls();
            swap_retreat_points();
            state = Boss_fight_state_e.FIGHTERS_CHARGING;

            break;

        }
    }

    //--------------------------------------------------------------------------

    public void notify_checkpoint_load()
    {
        state = Boss_fight_state_e.NOT_STARTED;
    }// notify_checkpoint_load

    //--------------------------------------------------------------------------

    void swap_retreat_points()
    {
        var temp = player_retreat_point;
        player_retreat_point = boss_retreat_point;
        boss_retreat_point = temp;

        player_retreat_point.y = Llama.get().transform.position.y;
    }// swap_retreat_points

    //--------------------------------------------------------------------------

}
