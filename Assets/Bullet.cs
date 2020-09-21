using UnityEngine;
using System.Collections;
using System;

public class Bullet : VerticalGameObject
{
    public int Damage { get; set; }

    // Use this for initialization
    void Start()
    {
        Damage = 1;
        SetVelocityAndScreenBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(transform.position.y) > screenBounds.y * 1.25)
        {
            Destroy(this.gameObject);
        }
    }
}
