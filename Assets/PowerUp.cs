using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : VerticalGameObject
{
    public enum BulletPowerUp
    {
        DamageShot, TripleShot, SpeedShot, None
    }

    public enum PlayerPowerUp
    {
        Health, Speed, None
    }

    public BulletPowerUp BulletPower { set; get; }
    public PlayerPowerUp PlayerPower { set; get; }
    public bool AffectsPlayer { set; get; }

    // Start is called before the first frame update
    void Start()
    {
        
        SetVelocityAndScreenBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < screenBounds.y * -1.25)
        {
            Destroy(this.gameObject);
        }
    }
}
