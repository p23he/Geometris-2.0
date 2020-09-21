using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum BulletType
    {
        PentaShot, HexaShot, Octoshot
    }

    public float Speed { get; set; }
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public Text ScoreText;
    public int Score { get; set; }

    private const float MAX_TILT = 15f;
    Boolean alreadyTiltedLeft = false;
    Boolean alreadyTiltedRight = false;


    public List<GameObject> bulletPrefabs;
    private Dictionary<BulletType, GameObject> bulletMap = new Dictionary<BulletType, GameObject>();
    public BulletType bullet = BulletType.PentaShot;

    public PowerUp.BulletPowerUp BulletPowerUp { set; get; }
    public PowerUp.PlayerPowerUp PlayerPowerUp { set; get; }

    public GameObject healthPrefab;
    public List<GameObject> healthContainers = new List<GameObject>(); 

    private GUIStyle guiStyle = new GUIStyle();
    public Font myFont;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 5;
        Health = 5;
        BulletPowerUp = PowerUp.BulletPowerUp.None;
        PlayerPowerUp = PowerUp.PlayerPowerUp.None;
        InstantiateHealthbar();
        Debug.Log("Game started.");
        transform.position = new Vector2(0, -4);

        Score = 0;

        // Iterate over powerup enum and map each prefab with each powerup
        int i = 0;
        foreach(BulletType bullet in BulletType.GetValues(typeof(PowerUp.BulletPowerUp))) {
            if (i >= bulletPrefabs.Count) break;
            bulletMap.Add(bullet, bulletPrefabs[i]);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            return;
        }

        // Back to no tilt when key up
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (alreadyTiltedLeft)
            {
                transform.Rotate(Vector3.back * MAX_TILT);
            }
            
            
            alreadyTiltedLeft = false;
        } 
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            
            if (alreadyTiltedRight)
            {
                transform.Rotate(Vector3.forward * MAX_TILT);
            }
            
            alreadyTiltedRight = false;
        }

        // Move left or right depending on key pressed
        float translateX = (float)Math.Cos((MAX_TILT / 180) * Math.PI);
        float translateY = (float)Math.Sin((MAX_TILT / 180) * Math.PI);

        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (!alreadyTiltedLeft)
            {
                transform.Rotate(Vector3.forward * MAX_TILT);
            }
            alreadyTiltedLeft = true;
            transform.Translate(new Vector2(-translateX, translateY) * Speed * Time.deltaTime);
           
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!alreadyTiltedRight)
            {
                transform.Rotate(Vector3.back * MAX_TILT);
            }
            alreadyTiltedRight = true;
           
            transform.Translate(new Vector2(translateX, translateY) * Speed * Time.deltaTime);
        }

        // Shooting 
        if (Input.GetKeyDown (KeyCode.Space))
        {
            this.GetComponent<AudioSource>().Play();
            Shoot();
        }
    }
    // TODO: make constants for these numbs
    private void Shoot()
    {
        if (BulletPowerUp == PowerUp.BulletPowerUp.TripleShot)
        {
            CreateBullet(-35, 10, 1);
            CreateBullet(0, 10, 1);
            CreateBullet(35, 10, 1);
        } 
        else if (BulletPowerUp == PowerUp.BulletPowerUp.SpeedShot)
        {
            CreateBullet(0, 50, 1);
        } 
        else if (BulletPowerUp == PowerUp.BulletPowerUp.DamageShot)
        {
            CreateBullet(0, 20, 2);
        }
        else
        {
            CreateBullet(0, 10, 1);
        }
    }

    private void UpdatePlayerStats()
    {
        if (PlayerPowerUp == PowerUp.PlayerPowerUp.Speed)
        {
            Speed = Math.Min(7, Speed) + 1;
        }
    }

    // ADD DAMAGE PARAM
    // Creates a bullet/projectile with specified speed and degree from vertical
    /// <summary>
    /// Creates a bullet/projectile and sets it speed and degree from vertical
    /// </summary>
    /// <param name="degFromVertical">Angle of shot</param>
    /// <param name="speed">Speed of shot</param>
    public void CreateBullet(float degFromVertical, float speed, int damage)
    {
        Vector3 playerPos = this.transform.position;
        GameObject projectileGameObject = Instantiate(bulletMap[bullet]) as GameObject;
        projectileGameObject.transform.position = new Vector3(playerPos.x, playerPos.y + 1, playerPos.z);
        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.DegFromVertical = degFromVertical;
        projectile.Speed = speed;
        projectile.Damage = damage;
    }

    // For when the enemy collides with player ship, 
    private void OnTriggerEnter2D(Collider2D other)
    {
        // When player collides with enemy ship
        if (other.tag == "Enemy")
        {
            Health--;
        }
        // When player collides with enemy shot
        if (other.tag == "EnemyShot")
        {
            Health -= other.GetComponent<Bullet>().Damage;
            Debug.Log("health is now " + Health);
        }
        // When player collides with a power up block
        if (other.tag == "PowerUp")
        {
            PowerUp powerUpObj = other.GetComponent<PowerUp>();
            if (!powerUpObj.AffectsPlayer)
            {
                BulletPowerUp = powerUpObj.BulletPower;
            }
            else
            {
                if (powerUpObj.PlayerPower == PowerUp.PlayerPowerUp.Health)
                {
                    Health++;
                    UpdateHealthBar();
                }
                else
                {
                    PlayerPowerUp = powerUpObj.PlayerPower;
                    UpdatePlayerStats();
                }
            }
            Debug.Log(BulletPowerUp + " " + PlayerPowerUp);
        }
        UpdateHealthBar();
        Destroy(other.gameObject);
    }

    // creates the player's health bar in the beginning of the game
    // called in Start()
    private void InstantiateHealthbar()
    {
        for (int i = 0; i < Health; i++)
        {
            GameObject healthGameObject = Instantiate(healthPrefab) as GameObject;
            healthContainers.Add(healthGameObject);
            healthGameObject.transform.position = new Vector3(-9.5f + (i * 0.5f), 4.3f, 0);
        }

    }

    private void UpdateHealthBar()
    {
        for (int i = 0; i < healthContainers.Count; i++)
        {
            Renderer healthRenderer = healthContainers[i].gameObject.GetComponent<Renderer>();
            healthRenderer.material.SetColor("_Color", Color.red);
        }

        for (int i = healthContainers.Count - 1; i >= Math.Max(Health, 0); i--)
        {
            Renderer healthRenderer = healthContainers[i].gameObject.GetComponent<Renderer>();
            healthRenderer.material.SetColor("_Color", Color.gray);
        }
    }

    private void OnGUI()
    {
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.red;
        GUILayout.Label("Score: " + Score.ToString(), guiStyle);
    }


}
