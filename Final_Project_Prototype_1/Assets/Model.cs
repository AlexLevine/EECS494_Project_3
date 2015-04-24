using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model : MonoBehaviour
{
    private static Model instance;

    private List<GameObject> players = new List<GameObject>();
    private List<Enemy> enemies = new List<Enemy>();

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

    public void register_enemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void remove_enemy(Enemy enemy)
    {
        var removed = enemies.Remove(enemy);
        print("removed: " + removed);
        broadcast_enemy_gone(enemy);
    }

    public void register_player(GameObject player)
    {
        players.Add(player);
    }

    public List<GameObject> get_players()
    {
        return players;
    }

    public void subscribe_to_checkpoint_load(
        Checkpoint_load_subscriber subscriber)
    {
        checkpoint_subscribers.Add(subscriber);
    }

    public void unsubscribe_from_checkpoint_load(
        Checkpoint_load_subscriber subscriber)
    {
        checkpoint_subscribers.Remove(subscriber);
    }

    public List<Checkpoint_load_subscriber> get_checkpoint_subscribers()
    {
        return checkpoint_subscribers;
    }

    // // Use this for initialization
    // void Start ()
    // {

    // }

    // Update is called once per frame
    // void LateUpdate()
    // {
    //     enemies.RemoveAll((Enemy obj) => obj == null);
    // }

    public void reset()
    {
        players = new List<GameObject>();
        enemies = new List<Enemy>();
        checkpoint_subscribers = new List<Checkpoint_load_subscriber>();
    }

    //--------------------------------------------------------------------------

    public void broadcast_enemy_gone(Enemy enemy)
    {
        foreach (var player in players)
        {
            if (player == null)
            {
                continue;
            }

            player.GetComponent<Player_character>().notify_enemy_gone(
                enemy.gameObject);
        }
    }// broadcast_enemy_gone

    //--------------------------------------------------------------------------

    public List<Enemy> get_potential_lock_on_targets(
        GameObject player)
    {
        return enemies.FindAll(
            (Enemy obj) => Vector3.Angle(
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
        var closest_target = potential_targets[0];
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

        return closest_target.gameObject;
        // return potential_targets.MinBy(
        //     (GameObject obj) => Vector3.Distance(
        //         transform.position, obj.transform.position));
    }// get_closest_potential_lock_on_target
}
