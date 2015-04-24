using UnityEngine;
using System.Collections;

public class Heart_rotate : MonoBehaviour {
    // Update is called once per frame
    void Update () {
       transform.Rotate(Vector3.up * Time.deltaTime * 20f);
    }
}
