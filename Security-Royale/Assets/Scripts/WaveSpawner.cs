using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {


	public static int EnemiesAlive = 0;

    [HideInInspector]
    public int simpleEnemiesAlive = 0; //simple enemies that are alive
    [HideInInspector]
    public int fastEnemiesAlive = 0;   //fast enemies that are alive
    [HideInInspector]
    public int toughEnemiesAlive = 0;  //tough enemies that are alive

    [HideInInspector]
    public int simpleEnemyCount = 0;
    [HideInInspector]
    public int fastEnemyCount = 0;
    [HideInInspector]
    public int toughEnemyCount = 0;

    public bool bCanStartRound = false;
    public bool bRoundStarted = false;

    public Wave[] waves;

	public Transform spawnPoint;

	public float timeBetweenWaves = 5f;
	private float countdown = 2f;

	public Text waveCountdownText;

	public GameManager gameManager;

	private int waveIndex = 0;

	void Update ()
	{

        EnemiesAlive = simpleEnemyCount + fastEnemyCount + toughEnemyCount; //sum of all enemies
        Debug.Log(EnemiesAlive);                                                  //check current number of all enemies


        if (EnemiesAlive > 0)
		{
			return;
		}

		if (waveIndex == waves.Length)
		{
			gameManager.WinLevel();
			this.enabled = false;
		}

		if (countdown <= 0f)
		{
            bCanStartRound = true;
			//StartCoroutine(SpawnWave());
			countdown = timeBetweenWaves;
			return;
		}

        //if (bRoundStarted)
        //{
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
            waveCountdownText.text = string.Format("{0:00.00}", countdown);
        //}
	}

    //-------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------

    public void AddSimpleEnemy()
    {
        if (PlayerStats.AttackMoney >= Enemy.CostSimpleEnemy)
        {
            PlayerStats.AttackMoney -= Enemy.CostSimpleEnemy;
            simpleEnemyCount++; //something similar
        }

        if (PlayerStats.AttackMoney < 0)
        {
            PlayerStats.AttackMoney = 0;
            simpleEnemyCount--;
        }
    }

    public void AddFastEnemy()
    {
        if (PlayerStats.AttackMoney >= Enemy.CostFastEnemy)
        {
            PlayerStats.AttackMoney -= Enemy.CostFastEnemy;
            fastEnemyCount++; //something similar
        }

        if (PlayerStats.AttackMoney < 0)
        {
            PlayerStats.AttackMoney = 0;
            fastEnemyCount--;
        }
    }

    public void AddToughEnemy()
    {
        if (PlayerStats.AttackMoney >= Enemy.CostToughEnemy)
        {
            PlayerStats.AttackMoney -= Enemy.CostToughEnemy;
            toughEnemyCount++; //something similar
        }

        if (PlayerStats.AttackMoney < 0)
        {
            PlayerStats.AttackMoney = 0;
            toughEnemyCount--;
        }
    }

    public void StartSpawnWave()
    {
        if (bCanStartRound)
        {
            StartCoroutine(SpawnWave());
            bRoundStarted = true;
        }
    }

    //-------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------

    IEnumerator SpawnWave ()
	{
		PlayerStats.Rounds++;

		Wave wave = waves[waveIndex];

        wave.simpleEnemyCount = simpleEnemyCount;
        wave.fastEnemyCount = fastEnemyCount;
        wave.toughEnemyCount = toughEnemyCount;

        EnemiesAlive = wave.simpleEnemyCount + wave.fastEnemyCount + wave.toughEnemyCount;

        //for (int i = 0; i < wave.count; i++)
        //{
        //    SpawnEnemy(wave.enemy);
        //    yield return new WaitForSeconds(1f / wave.rate);
        //}

        //---------------------------------------------------------------------------

        for (int i = 0; i < wave.simpleEnemyCount; i++)
        {
            SpawnEnemy(wave.simpleEnemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        for (int i = 0; i < wave.fastEnemyCount; i++)
        {
            SpawnEnemy(wave.fastEnemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        for (int i = 0; i < wave.toughEnemyCount; i++)
        {
            SpawnEnemy(wave.toughEnemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;
	}

	void SpawnEnemy (GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}

}
