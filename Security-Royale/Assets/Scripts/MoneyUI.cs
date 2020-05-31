using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyUI : MonoBehaviour
{

	public Text defenseMoneyText;

    public Text attackMoneyText;

    // Update is called once per frame
    void Update ()
    {
        defenseMoneyText.text = "Defense Money:$" + PlayerStats.Money.ToString();

        attackMoneyText.text = "Attack Money: $" + PlayerStats.AttackMoney.ToString();
    }
}
