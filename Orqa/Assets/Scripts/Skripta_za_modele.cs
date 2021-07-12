using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skripta_za_modele : MonoBehaviour
{
    private float speed = 50f;
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
    }
}
