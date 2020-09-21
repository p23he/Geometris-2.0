using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public List<GameObject> bulletPowerUpPrefabs = new List<GameObject>();
    public List<GameObject> playerPowerUpPrefabs = new List<GameObject>();

    private Dictionary<Enemy.EnemyType, GameObject> enemyMap = new Dictionary<Enemy.EnemyType, GameObject>();
    private Dictionary<PowerUp.BulletPowerUp, GameObject> bulletPowerUpMap = new Dictionary<PowerUp.BulletPowerUp, GameObject>();
    private Dictionary<PowerUp.PlayerPowerUp, GameObject> playerPowerUpMap = new Dictionary<PowerUp.PlayerPowerUp, GameObject>();

    private Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach(Enemy.EnemyType enemyType in Enemy.EnemyType.GetValues(typeof(Enemy.EnemyType)))
        {
            enemyMap.Add(enemyType, enemyPrefabs[i]);
            i++;
        }
        i = 0;
        foreach (PowerUp.BulletPowerUp powerUpType in PowerUp.BulletPowerUp.GetValues(typeof(PowerUp.BulletPowerUp)))
        {
            if (i >= bulletPowerUpPrefabs.Count) break;
            bulletPowerUpMap.Add(powerUpType, bulletPowerUpPrefabs[i]);
            i++;
        }
        i = 0;
        foreach (PowerUp.PlayerPowerUp powerUpType in PowerUp.PlayerPowerUp.GetValues(typeof(PowerUp.PlayerPowerUp)))
        {
            if (i >= playerPowerUpPrefabs.Count) break;
            playerPowerUpMap.Add(powerUpType, playerPowerUpPrefabs[i]);
            i++;
        }
    }

    void SpawnEnemy(Enemy.EnemyType enemyType)
    {
        GameObject enemyGameObject = SpawnVerticalObject(enemyMap[enemyType]); 
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();
        enemy.Type = enemyType;
    }

    void SpawnBulletPowerUp(PowerUp.BulletPowerUp powerUpType)
    {
        GameObject powerUpGameObject = SpawnVerticalObject(bulletPowerUpMap[powerUpType]);
        PowerUp powerUp = powerUpGameObject.GetComponent<PowerUp>();
        powerUp.Speed = -1;
        powerUp.BulletPower = powerUpType;
    }

    void SpawnPlayerPowerUp(PowerUp.PlayerPowerUp powerUpType)
    {
        GameObject powerUpGameObject = SpawnVerticalObject(playerPowerUpMap[powerUpType]);
        PowerUp powerUp = powerUpGameObject.GetComponent<PowerUp>();
        powerUp.Speed = -1;
        powerUp.PlayerPower = powerUpType;
        powerUp.AffectsPlayer = true;
    }

    /// <summary>
    /// Spawn a vertical moving game object using specified prefab
    /// </summary>
    /// <param name="prefab"></param>
    private GameObject SpawnVerticalObject(GameObject prefab)
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        GameObject gameObj = Instantiate(prefab);
        System.Random rnd = new System.Random();
        gameObj.transform.position = new Vector3(rnd.Next(0, 2 * (int)screenBounds.x) - (int)screenBounds.x, screenBounds.y, 0);
        return gameObj;
    }

    private PowerUp.BulletPowerUp GetRandomBulletPowerUp()
    {
        return (PowerUp.BulletPowerUp)UnityEngine.Random.Range(0, 3);
    }

    private PowerUp.PlayerPowerUp GetRandomPlayerPowerUp()
    {
        return (PowerUp.PlayerPowerUp)UnityEngine.Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        int t = Time.frameCount;
        int score = GameObject.FindWithTag("Player").GetComponent<Player>().Score;
        int oneOrTwo = UnityEngine.Random.Range(0, 1);

        if (t % (300 + 5 * score) == 0)
        {
            SpawnEnemy(Enemy.EnemyType.Triangle);
        }
        if (t % (502 + 5 * score) == 0)
        {
            SpawnEnemy(Enemy.EnemyType.Square);
        }
        if (t % 3005 == 0 && t % 2 == oneOrTwo)
        {
            SpawnBulletPowerUp(GetRandomBulletPowerUp());
        }
        if (t % (Math.Max(504, 1005 - 25 * score)) == 0)
        {
            SpawnEnemy(Enemy.EnemyType.Pentagon);
        
        }
        if (t % 3005 == 0 && t % 2 == 1 - oneOrTwo)
        {
            SpawnPlayerPowerUp(GetRandomPlayerPowerUp());
        }
    }
}
