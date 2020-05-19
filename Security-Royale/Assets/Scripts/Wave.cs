using UnityEngine;

[System.Serializable]
public class Wave
{

	public GameObject simpleEnemy;
	public GameObject fastEnemy;
	public GameObject toughEnemy;

    [HideInInspector]
	public int simpleEnemyCount;
    [HideInInspector]
    public int fastEnemyCount;
    [HideInInspector]
    public int toughEnemyCount;

	public float rate;

}
