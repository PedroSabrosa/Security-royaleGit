using UnityEngine;

[System.Serializable]
public class Wave
{

	public GameObject simpleEnemy;
	public GameObject fastEnemy;
	public GameObject toughEnemy;
    public GameObject destructionEnemy;

    [HideInInspector]
	public int simpleEnemyCount;
    [HideInInspector]
    public int fastEnemyCount;
    [HideInInspector]
    public int toughEnemyCount;
    [HideInInspector]
    public int destructionEnemyCount;

    public float rate;

}
