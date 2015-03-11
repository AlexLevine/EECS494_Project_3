using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Enemy : Actor
{
    public static List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        enemies.Add(gameObject);
    }

    //--------------------------------------------------------------------------

    // // Use this for initialization
    // public override void Start()
    // {
    //     base.Start();
    // }// Start

    //--------------------------------------------------------------------------

    // // Update is called once per frame
    // public override void Update ()
    // {
    //     base.Update();
    // }// Update

    //--------------------------------------------------------------------------

    public static List<GameObject> get_potential_lock_on_targets(
        GameObject player)
    {
        return enemies.FindAll(
            (GameObject obj) => Vector3.Angle(
                player.transform.position, obj.transform.position) <= 90f);
    }// get_potential_lock_on_targets

    //--------------------------------------------------------------------------

    public static GameObject get_closest_potential_lock_on_target(
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

    //--------------------------------------------------------------------------

    public virtual void on_player_hit(){}

    //--------------------------------------------------------------------------

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<Actor>().receive_hit(attack_power);
            on_player_hit();
        }
    }// OnCollisionEnter

    //--------------------------------------------------------------------------

    public virtual int attack_power
    {
        get
        {
            throw new Exception("Derived classes must override this property");
        }
    }// attack_power

    //--------------------------------------------------------------------------

	public virtual void on_hit_spit(int damage)
    {
		// default behavior
		receive_hit (damage);
	}// on_hit_spit

    //--------------------------------------------------------------------------

	public virtual void on_hit_sword(int damage)
    {
		// default behavior
		receive_hit (damage);
	}// on_hit_sword

    //--------------------------------------------------------------------------

	public virtual void on_hit_charge(int damage)
	{
		// default behavior
		receive_hit (damage);
	}// on_hit_charge

    //--------------------------------------------------------------------------

    public virtual void on_hit_by_jousting_pole(int damage)
    {
        receive_hit(damage);
    }// on_hit_by_jousting_pole

	//--------------------------------------------------------------------------

	public override void on_death()
    {
        enemies.Remove(gameObject);
        foreach (var player in Player_character.players)
        {
            player.GetComponent<Player_character>().notify_enemy_killed(
                gameObject);
        }

        Destroy(gameObject);
    }// on_death
}
