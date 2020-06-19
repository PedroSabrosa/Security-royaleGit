using UnityEngine;

public class BuildManager : MonoBehaviour
{

	public static BuildManager instance;

	void Awake ()
	{
		if (instance != null)
		{
			Debug.LogError("More than one BuildManager in scene!");
			return;
		}
		instance = this;
	}

	public GameObject buildEffect;
	public GameObject sellEffect;

	public GameObject costTowerPrefab;

	private TurretBlueprint turretToBuild;
	private Node selectedNode;

	public NodeUI nodeUI;

	public bool CanBuild
    {
        get
        {
            return (turretToBuild != null && GameObject.Find("GameMaster").GetComponent<WaveSpawner>().bDefenseTurnActive == true);
        }
    }

	public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

	public void SelectNode (Node node)
	{
		if (selectedNode == node)
		{
			DeselectNode();
			return;
		}

		selectedNode = node;
		turretToBuild = null;

		nodeUI.SetTarget(node);
	}

	public void DeselectNode()
	{
		selectedNode = null;
		nodeUI.Hide();
	}

	public void SelectTurretToBuild (TurretBlueprint turret)
	{
		turretToBuild = turret;
		DeselectNode();
	}

	public TurretBlueprint GetTurretToBuild ()
	{
		return turretToBuild;
	}

}
