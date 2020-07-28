using System.Collections;
using System.Collections.Generic;
using TowerDefense.Runtime;
using UnityEngine;

public class Manager : Loader<Manager>
{
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int maxEnemiesOnScreen;
    [SerializeField] int totalEnemies;
    [SerializeField] int enemiesPerSpawn;

    public List<Enemy> EnemyList = new List<Enemy>();

    const float spawnDelay = 1f;
    //private int enemiesOnScreen = 0;
    void Start()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for(int i = 0; i < enemiesPerSpawn; i++)
            {
                if(EnemyList.Count < maxEnemiesOnScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[0]) as GameObject;
                    newEnemy.transform.position = SpawnPoint.transform.position;
                    //enemiesOnScreen +=1;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroyEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }
    /*public void RemoveEnemyFromScreen()
    {
        if(enemiesOnScreen > 0)
        {
            enemiesOnScreen -= 1;
        }
    }*/
}
