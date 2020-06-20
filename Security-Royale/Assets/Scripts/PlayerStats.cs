using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{

	public static int Money;
    private int startMoney = 400;

    public static int AttackMoney;
    private int startAttackMoney = 330;

    public static int Lives;
    private int startLives = 3;

	public static int Rounds;

	void Start ()
	{
		Money = startMoney;
        Lives = startLives;

        AttackMoney = startAttackMoney;

        Rounds = 0;
	}

    public void AddAttackMoney(int amount)
    {
        AttackMoney += amount;
    }

}
