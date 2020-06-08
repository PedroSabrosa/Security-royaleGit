using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public static int EnemiesAlive;

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

    public bool bCanStartRound;
    public bool bRoundStarted;
    public bool bDefenseTurnActive;
    public bool bAttackTurnActive;
    public bool bTroopsSent;
    public bool bPassingTurn;
    public bool bEndOfRound;

    public Wave[] waves;

	public Transform spawnPoint;

    private float attackTurnTimer = 60f;  // ADDING TIMERS
    private float defenseTurnTimer = 30f; // ADDING TIMERS
    private float passingTurnTimer = 5f;  // ADDING TIMERS
    private float endOfRoundTimer = 10f;  // ADDING TIMERS

    public Text timerText;                // ADDING TIMERS
    public Text turnText;          

    public GameObject attackPanel;
    public GameObject defensePanel;
    public GameObject shopPanel;
    public GameObject fogPanel;

	public GameManager gameManager;

	private int waveIndex = 0;

    void Awake()
    {
        bDefenseTurnActive = true;
        bAttackTurnActive = false;
        bTroopsSent = false;
        bPassingTurn = false;
        bEndOfRound = false;

        defensePanel.SetActive(true);
        shopPanel.SetActive(true);
        attackPanel.SetActive(false);
        fogPanel.SetActive(false);

        bCanStartRound = true;
        bRoundStarted = false;
    }

	void Update ()
	{

        EnemiesAlive = simpleEnemyCount + fastEnemyCount + toughEnemyCount; //sum of all enemies
        Debug.Log("EnemiesAlive: " + EnemiesAlive);                         //check current number of all enemies

        CheckTurns();

  //      if (EnemiesAlive > 0)
		//{
		//	return;
		//}

		//if (waveIndex == waves.Length)
		//{
		//	gameManager.WinLevel();
		//	this.enabled = false;
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

    //-------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------

    public void EndDefenseTurn()
    {
        bDefenseTurnActive = false;

        defensePanel.SetActive(false);
        shopPanel.SetActive(false);

        bPassingTurn = true;
    }

    public void StartAttackTurn()
    {
        bAttackTurnActive = true;

        attackPanel.SetActive(true);
        //fogPanel.SetActive(true);
    }

    public void SendTroops()
    {
        bTroopsSent = true;
        StartCoroutine(SpawnWave());
    }

    public void EndAttackTurn()
    {
        bEndOfRound = true;
    }

    public void CheckTurns()
    {

        if (bDefenseTurnActive)
        {
            turnText.text = "Defense Turn";

            defenseTurnTimer -= Time.deltaTime;
            defenseTurnTimer = Mathf.Clamp(defenseTurnTimer, 0f, Mathf.Infinity);
            timerText.text = string.Format("{00:00}", defenseTurnTimer);

            if (defenseTurnTimer <= 0)
            {
                bPassingTurn = true;
            }
        }

        if (bPassingTurn)
        {
            turnText.text = "Passing Turns";

            passingTurnTimer -= Time.deltaTime;
            passingTurnTimer = Mathf.Clamp(passingTurnTimer, 0f, Mathf.Infinity);
            timerText.text = string.Format("{00:00}", passingTurnTimer);

            bDefenseTurnActive = false;
            bAttackTurnActive = false;

            defensePanel.SetActive(false);
            shopPanel.SetActive(false);
            attackPanel.SetActive(false);
            //fogPanel.SetActive(true);

            if (passingTurnTimer <= 0)
            {
                StartAttackTurn();
                bPassingTurn = false;
            }
        }

        if (bAttackTurnActive)
        {
            bDefenseTurnActive = false;
            turnText.text = "Attack Turn";

            attackTurnTimer -= Time.deltaTime;
            attackTurnTimer = Mathf.Clamp(attackTurnTimer, 0f, Mathf.Infinity);
            timerText.text = string.Format("{00:00}", attackTurnTimer);

            if (attackTurnTimer <= 0)
            {
                EndAttackTurn();
            }
        }

        if (bEndOfRound)
        {
            turnText.text = "Final Seconds";

            endOfRoundTimer -= Time.deltaTime;
            endOfRoundTimer = Mathf.Clamp(endOfRoundTimer, 0f, Mathf.Infinity);
            timerText.text = string.Format("{00:00}", endOfRoundTimer);

            bDefenseTurnActive = false;
            bAttackTurnActive = false;

            defensePanel.SetActive(false);
            shopPanel.SetActive(false);
            attackPanel.SetActive(false);
            fogPanel.SetActive(false);

            if (endOfRoundTimer <= 0)
            {
                turnText.text = "Round Ended";
                bEndOfRound = false;
            }
        }
    }

    //-------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------

    IEnumerator SpawnWave ()
	{
        Wave wave = waves[waveIndex];

        wave.simpleEnemyCount = simpleEnemyCount;
        wave.fastEnemyCount = fastEnemyCount;
        wave.toughEnemyCount = toughEnemyCount;

        EnemiesAlive = wave.simpleEnemyCount + wave.fastEnemyCount + wave.toughEnemyCount;

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

        ResetWave();
	}

    void ResetWave()
    {
        simpleEnemyCount = 0;
        fastEnemyCount = 0;
        toughEnemyCount = 0;
    }

	void SpawnEnemy (GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}

}
