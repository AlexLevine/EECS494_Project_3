using UnityEngine;
using System.Collections;

public class Fire_wall : Switchee {
    
    public override void activate()
    {
        gameObject.SetActive(false);
    }    
 
}
