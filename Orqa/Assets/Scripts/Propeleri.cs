using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeleri : MonoBehaviour
{
    private float speed = 5000f;
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
