using UnityEngine;
using System.Collections;

public class Llama_spit : MonoBehaviour
{
    public static string global_name = "llama_spit";

    public Vector3 velocity = Vector3.zero;
    public int attack_power { get { return 5; } }

    private float time_left = 0.50f;

    void Awake()
    {
        name = global_name;
    }

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
        time_left -= Time.deltaTime;

        // print(time_left);
        if (time_left <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // print("blaaaah");
        var enemy = other.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.receive_hit(attack_power, Vector3.zero, gameObject);
        }

        // print(other.gameObject.tag);
        var hit_player = other.gameObject.tag == "Player";
        var hit_weapon = other.gameObject.tag == "weapon";
        var hit_trigger = other.isTrigger;
        if(hit_player || hit_weapon || hit_trigger)
        {
            return;
        }

        // print(other);

        Destroy(gameObject);
    }
}
