using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyUI : MonoBehaviour
{

	public Text moneyText;

    public Text attackMoneyText;

    // Update is called once per frame
    void Update ()
    {
        moneyText.text = "$" + PlayerStats.Money.ToString();

        attackMoneyText.text = "Attack Money = $" + PlayerStats.AttackMoney.ToString();
    }
}
