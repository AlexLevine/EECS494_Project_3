using UnityEngine;
using System.Collections;

public class Sword_script : MonoBehaviour {

    private int health = 3;
    
    private float speed = 4f;

    //the 2 players that the sword targets
    const string player_name1 = "player1";
    const string player_name2 = "player2";

    private float rand_speed;
    
    public GameObject start_location; 

    public float center_x = 0f;
    public float center_y = 0f;
    
    private float leftEdge;
    private float rightEdge;
    private float downEdge;
    private float upEdge;

    public Vector3 start; 
    private Vector3 dest;
    
    private Color originalColor;

    
    // Use this for initialization
    void Start () 
    {
        start = start_location.transform.position; 

        originalColor = renderer.material.color;
        dest = new Vector3 (0, 0, 0);
        transform.position = start;
    }
    
    void Update () 
    {
        //Basic Movement
        Vector3 pos = this.transform.position;

        Vector3 player1_pos = pos;
        Vector3 player2_pos = pos;

        // if the objects exist, find their postions
        if (GameObject.Find(player_name1))
        {
            player1_pos = GameObject.Find(player_name1).transform.position;
        } 
        if (GameObject.Find(player_name2)) 
        {
            player2_pos = GameObject.Find(player_name2).transform.position;
        }

        // set destination point
        if (player1_pos == pos && player2_pos != pos) 
        {
            dest = player2_pos;
        }
        else if (player2_pos == pos && player1_pos != pos) 
        {
            dest = player1_pos;
        } 
        else if (player2_pos == pos && player1_pos == pos) 
        {
            dest = pos;
        }
        // else if (Mathf.Sqrt(Mathf.Pow((player1_pos.y - pos.y), 2) + Mathf.Pow((player1_pos.x - pos.x), 2)) <
        //     Mathf.Sqrt(Mathf.Pow((player2_pos.y - pos.y), 2) + Mathf.Pow((player2_pos.x - pos.x), 2)))  
        else if(Vector3.Distance(player1_pos, pos) < Vector3.Distance(player2_pos, pos))
        {
            dest = player1_pos;
        } 
        else 
        {
            dest = player2_pos;
        }

        Vector3 angle = dest - pos;
        angle.Normalize ();
        
        this.rigidbody.velocity = angle * speed;

        if (this.rigidbody.velocity != Vector3.zero) 
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(this.rigidbody.velocity),
                Time.deltaTime * speed);
        }
    }
    
    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0) {
            // dead
            Destroy(gameObject);

        }
        StartCoroutine (flashSprite ());
    }

    public IEnumerator flashSprite() 
    {
        for (int i = 0; i < 5; ++i) {
            renderer.material.color = Color.white;
            yield return new WaitForSeconds(.1f);
            renderer.material.color = originalColor;
            yield return new WaitForSeconds(.1f);
        }
        renderer.material.color = originalColor;
    }
}
