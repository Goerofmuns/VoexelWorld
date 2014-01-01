using UnityEngine;
using System.Collections;

public class sunScript : MonoBehaviour
{

    GameObject playerGO;

    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("world");
    }

    void Update()
    {
        transform.LookAt(playerGO.transform);
        transform.RotateAround(playerGO.transform.position, new Vector3(1, 0, 0), 1 * Time.deltaTime);
    }
}
