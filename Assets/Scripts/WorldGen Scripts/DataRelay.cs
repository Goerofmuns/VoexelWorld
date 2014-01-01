using UnityEngine;
using System.Collections;

public class DataRelay : MonoBehaviour {

    public Block[, ,] data;

    void Start()
    {
        data = new Block[512, 64, 512];
        Object.DontDestroyOnLoad(gameObject);
    }
}
