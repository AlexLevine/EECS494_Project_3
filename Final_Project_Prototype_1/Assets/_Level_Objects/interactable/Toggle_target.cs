using UnityEngine;
using System.Collections;
using System;

public class Toggle_target : MonoBehaviour 
{    
    public virtual void toggle_script()
    {
        throw new Exception("Derived classes must override this property");
    }
}
