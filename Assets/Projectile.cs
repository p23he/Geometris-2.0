using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour

{
    public float Speed { get; set; }
    public int Damage { get; set; }
    public float DegFromVertical { get; set; }
    private Rigidbody2D rb;
    private Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        float velocityX = (float)Math.Sin(DegFromVertical * Math.PI / 180) * Speed;
        float velocityY = (float)Math.Cos(DegFromVertical * Math.PI / 180) * Speed;
        rb.velocity = new Vector2(velocityX, velocityY);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
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
