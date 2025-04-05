using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int healthPoints = 100;

    public float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Timer.platformRiding)
        {
            transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
        }
    }
}
