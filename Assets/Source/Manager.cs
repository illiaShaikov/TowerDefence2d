using System.Collections;
using System.Collections.Generic;
using TowerDefense.Runtime;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus
{
    next, play, gameover, win
}

public class Manager : Loader<Manager>
{
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] GameObject[] enemies;    
    [SerializeField] int totalEnemies;
    [SerializeField] int enemiesPerSpawn;
    [SerializeField] int totalWaves = 10;
    [SerializeField] Text totalMoneyLabel;
    [SerializeField] Text currentWave;
    [SerializeField] Text playMoney;
    [SerializeField] Text playBtnText;
    [SerializeField] Button playBtn;
    [SerializeField] Text totalEscapedLabel;

    int waveNumber = 0;
    int totalMoney = 10;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKilled = 0;
    int whichEnemyToSpawn = 0;
    GameStatus currentStatus = GameStatus.play;

    public List<Enemy> EnemyList = new List<Enemy>();

    const float spawnDelay = 1f;
    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = TotalMoney.ToString();
        }
    }
    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }
    //private int enemiesOnScreen = 0;
    void Start()
    {       
        playBtn.gameObject.SetActive(false);
        ShowMenu();
    }
    private void Update()
    {
        HandleEscape();
    }
    IEnumerator Spawn()
    {
        if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for(int i = 0; i < enemiesPerSpawn; i++)
            {
                if(EnemyList.Count < totalEnemies)
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
    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }
    public void SubstractMoney(int amount)
    {
        TotalMoney -= amount;
    }
    public void IsWaveOver()
    {
        totalEscapedLabel.text = totalEscaped + " / 10";
        if((roundEscaped + totalKilled) == totalEnemies)
        {
            SetCurrentGameStatus();
            ShowMenu();
        }
    }
    public void SetCurrentGameStatus()
    {
        if(totalEscaped >= 10)
        {
            currentStatus = GameStatus.gameover;
        }
        else if(waveNumber == 0 && (totalEscaped + totalKilled) == 0)
        {
            currentStatus = GameStatus.play;
        }
        else if(waveNumber >= totalWaves)
        {
            currentStatus = GameStatus.win;
        }
        else
        {
            currentStatus = GameStatus.next;
        }
    }
    public void PlayButtonPressed()
    {
        switch(currentStatus)
        {
            case GameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 5;
                TotalEscaped = 0;
                TotalMoney = 10;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagBuildSide();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = TotalEscaped + " / 10";
                break;
        }
        DestroyEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWave.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }
    public void ShowMenu()
    {
        switch(currentStatus)
        {
            case GameStatus.gameover:
                playBtnText.text = "PLAY AGAIN!";
                break;
            case GameStatus.next:
                playBtnText.text = "NEXT";
                break;
            case GameStatus.play:
                playBtnText.text = "PLAY";
                break;
            case GameStatus.win:
                playBtnText.text = "PLAY";
                break;
        }
        playBtn.gameObject.SetActive(true);
    }
    private void HandleEscape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDrag();
            TowerManager.Instance.towerButtonPressed = null;
        }
    }
}
