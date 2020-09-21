using UnityEngine;
using System.Collections;
using System;

public class VerticalGameObject : MonoBehaviour
{

    /// <summary>
    /// Speed of object
    /// </summary>
    public float Speed { set; get; }
    
    /// <summary>
    /// Degree of object path from vertical
    /// </summary>
    public float DegFromVertical { set; get; }

    protected Rigidbody2D rb;
    protected Vector2 screenBounds;
    
    /// <summary>
    /// Set the velocity of the RigidBody2D of the object and set the screen bounds
    /// </summary>
    protected void SetVelocityAndScreenBounds()
    {
        rb = this.GetComponent<Rigidbody2D>();
        float velocityX = (float)Math.Sin(DegFromVertical * Math.PI / 180) * Speed;
        float velocityY = (float)Math.Cos(DegFromVertical * Math.PI / 180) * Speed;
        rb.velocity = new Vector2(velocityX, velocityY);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }


}
