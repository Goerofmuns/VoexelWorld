using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * 5 / 10);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(-Vector3.forward * 5 / 10);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * 5 / 10);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * 5 / 10);
        }
    }
}
