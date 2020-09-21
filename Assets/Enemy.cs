using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : VerticalGameObject
{
    public enum EnemyType { Triangle, Square, Pentagon, Hexagon };

    public int Health { get; set; }
    public int FireRate { get; set; }
    public EnemyType Type { get; set; }

    public GameObject bulletPrefab;


    // Start is called before the first frame update
    void Start()
    {
        SetEnemyStats();
        SetVelocityAndScreenBounds();
        InvokeRepeating("Shoot", 0.5f, FireRate);
    }

    // Update is called once per frame
    void Update()
    {
        // Change the below if statement
        if (transform.position.y < screenBounds.y * -1.25)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Set the stats of the enemy depending on its type
    /// </summary>
    private void SetEnemyStats()
    {
        if (Type == EnemyType.Triangle)
        {
            Health = 1;
            FireRate = 4;
            Speed = -2;
        } 
        else if (Type == EnemyType.Square)
        {
            Health = 2;
            FireRate = 4;
            Speed = -0.5f;
        } 
        else if (Type == EnemyType.Pentagon)
        {
            Health = 3;
            FireRate = 3;
            Speed = -0.75f;
        }
        else if (Type == EnemyType.Hexagon)
        {
            Health = 4;
            FireRate = 4;
            Speed = -0.5f;
        }
    }

    /// <summary>
    /// Shoot a bullet according to the type of enemy
    /// </summary>
    private void Shoot()
    {
        if (Type == EnemyType.Triangle)
        {
            CreateBullet(0, -10, 1);
        }
        else if (Type == EnemyType.Square)
        {
            CreateBullet(-35, -5, 1);
            CreateBullet(0, -5, 1);
            CreateBullet(35, -5, 1);
        }
        else if (Type == EnemyType.Pentagon)
        {
            CreateBullet(0, -50, 1);
        }
        else if (Type == EnemyType.Hexagon)
        {
            CreateBullet(0, -1, 2);
        }
    }

    /// <summary>
    /// Creates a bullet/projectile and sets it speed and degree from vertical
    /// </summary>
    /// <param name="degFromVertical">Angle of shot</param>
    /// <param name="speed">Speed of shot</param>
    public void CreateBullet(float degFromVertical, float speed, int damage)
    {
        Vector3 playerPos = this.transform.position;
        GameObject projectileGameObject = Instantiate(bulletPrefab) as GameObject;
        projectileGameObject.transform.position = new Vector3(playerPos.x, playerPos.y + (0.5f * speed / Mathf.Abs(speed)), playerPos.z);


        // Get the colour of the enemy
        SpriteRenderer enemyRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        Color enemyColor = enemyRenderer.color;
        // Change the colour of the bullet to same colour as enemy
        Renderer bulletRenderer = projectileGameObject.GetComponent<Renderer>();
        bulletRenderer.material.SetColor("_Color", enemyColor);

        // Set the speed and degree of the projectile
        Bullet bullet = projectileGameObject.GetComponent<Bullet>();
        bullet.DegFromVertical = degFromVertical;
        bullet.Speed = speed;
        bullet.Damage = damage;

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerShot")
        {
            Health--;
            Destroy(other.gameObject);
            if (Health <= 0)
            {
                Destroy(this.gameObject);
                Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.Score++;
            }
        }
    }
}
