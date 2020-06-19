using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{

	public Color hoverColor;
	public Color notEnoughMoneyColor;
    public Vector3 positionOffset;

	[HideInInspector]
	public GameObject turret;
	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	[HideInInspector]
	public bool isUpgraded = false;

	private Renderer rend;
	private Color startColor;

	BuildManager buildManager;

    //---------------------------------------------
    public Vector3 addXValue;
    public Vector3 subtractXValue;
    public Vector3 addZValue;
    public Vector3 subtractZValue;

    void Start ()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;

		buildManager = BuildManager.instance;

        //----------------------------------------
        addXValue      = new Vector3(5f, 1f, 0f);
        subtractXValue = new Vector3(-5f, 1f, 0f);
        addZValue      = new Vector3(0f, 1f, 5f);
        subtractZValue = new Vector3(0f, 1f, -5f);
    }

	public Vector3 GetBuildPosition ()
	{
		return transform.position + positionOffset;
	}

    //------------------------------------------------
    public Vector3 GetCostTowerBuildPosition1()
    {
        return transform.position + positionOffset + addXValue;
    }

    public Vector3 GetCostTowerBuildPosition2()
    {
        return transform.position + positionOffset + subtractXValue;
    }

    public Vector3 GetCostTowerBuildPosition3()
    {
        return transform.position + positionOffset + addZValue;
    }

    public Vector3 GetCostTowerBuildPosition4()
    {
        return transform.position + positionOffset + subtractZValue;
    }
    //------------------------------------------------

    void OnMouseDown ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (turret != null)
		{
			buildManager.SelectNode(this);
			return;
		}

		if (!buildManager.CanBuild)
			return;

		BuildTurret(buildManager.GetTurretToBuild());
        //-------------------------------------------
        BuildCostTower();
    }

	void BuildTurret (TurretBlueprint blueprint)
	{
		if (PlayerStats.Money < blueprint.cost)
		{
			Debug.Log("Not enough money to build that!");
			return;
		}

		PlayerStats.Money -= blueprint.cost;

		GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;

		turretBlueprint = blueprint;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Debug.Log("Turret build!");
	}

    //-------------------------------------------------------------------------------------
    void BuildCostTower()
    {
        Instantiate(buildManager.costTowerPrefab, GetCostTowerBuildPosition1(), Quaternion.identity);
        Instantiate(buildManager.costTowerPrefab, GetCostTowerBuildPosition2(), Quaternion.identity);
        Instantiate(buildManager.costTowerPrefab, GetCostTowerBuildPosition3(), Quaternion.identity);
        Instantiate(buildManager.costTowerPrefab, GetCostTowerBuildPosition4(), Quaternion.identity);

        Debug.Log("Cost Tower build!");
    }
    //-------------------------------------------------------------------------------------

    public void UpgradeTurret ()
	{
		if (PlayerStats.Money < turretBlueprint.upgradeCost)
		{
			Debug.Log("Not enough money to upgrade that!");
			return;
		}

		PlayerStats.Money -= turretBlueprint.upgradeCost;

		//Get rid of the old turret
		Destroy(turret);

		//Build a new one
		GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		isUpgraded = true;

		Debug.Log("Turret upgraded!");
	}

	public void SellTurret ()
	{
		PlayerStats.Money += turretBlueprint.GetSellAmount();

		GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Destroy(turret);
		turretBlueprint = null;
	}

	void OnMouseEnter ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (!buildManager.CanBuild)
			return;

		if (buildManager.HasMoney)
		{
			rend.material.color = hoverColor;
		} else
		{
			rend.material.color = notEnoughMoneyColor;
		}

	}

	void OnMouseExit ()
	{
		rend.material.color = startColor;
    }

}
