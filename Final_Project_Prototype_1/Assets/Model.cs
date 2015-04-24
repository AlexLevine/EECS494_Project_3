using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model : MonoBehaviour
{
    private static Model instance;

    private List<GameObject> players = new List<Player_character>();
    private List<GameObject> enemies = new List<Enemy>();
    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    private List<Checkpoint_load_subscriber> checkpoint_subscribers =
            new List<Checkpoint_load_subscriber>();

    public static Model get()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void register_enemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    // // Use this for initialization
    // void Start ()
    // {

    // }

    // // Update is called once per frame
    // void Update ()
    // {

    // }

    public void reset()
    {

    }

    public List<GameObject> get_potential_lock_on_targets(
        GameObject player)
    {
        return enemies.FindAll(
            (GameObject obj) => Vector3.Angle(
                player.transform.position, obj.transform.position) <= 90f);
    }// get_potential_lock_on_targets

    //--------------------------------------------------------------------------

    public GameObject get_closest_potential_lock_on_target(
        GameObject player)
    {
        var potential_targets = get_potential_lock_on_targets(player);

        if (potential_targets.Count == 0)
        {
            return null;
        }

        // I am currently very angry at C#'s apparent lack of a min() function
        // that does this.
        GameObject closest_target = potential_targets[0];
        var closest_distance = Vector3.Distance(
            player.transform.position, closest_target.transform.position);
        foreach (var obj in potential_targets)
        {
            var distance = Vector3.Distance(
                player.transform.position, obj.transform.position);
            if (distance < closest_distance)
            {
                closest_target = obj;
                closest_distance = distance;
            }
        }

        return closest_target;
        // return potential_targets.MinBy(
        //     (GameObject obj) => Vector3.Distance(
        //         transform.position, obj.transform.position));
    }// get_closest_potential_lock_on_target
}
