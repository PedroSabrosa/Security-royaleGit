using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	public GameObject bulletPrefab;
	public float startSpeed = 10f;
	private GameObject gameManager;

	private bool isFastEnemy;
	private bool isDestructionEnemy;

	public bool preferredMissile;
	public bool preferredLaser;

	public string enemyTag = "Turret";

	public float range = 30f;

	public float fireRate = 0.5f;
	private float fireCountdown = 0f;

	private Transform target;
	private Turret targetTurret;

	public int damage;

	[HideInInspector]
	public float speed;

	public float startHealth = 100;
	private float health;

	public int worth = 50;

	public static int CostSimpleEnemy = 50; //per simple enemy
    public static int CostFastEnemy = 60; //per fast enemy
    public static int CostToughEnemy = 70; //per tough enemy
	public static int CostDestructionEnemy = 70; //per tough enemy
	public static int CostReturnOnSniffingSuccess = 30;

	public GameObject deathEffect;

	[Header("Unity Stuff")]
	public Image healthBar;

	private bool isDead = false;

    void Start()
    {
		gameManager = GameObject.FindGameObjectWithTag("GameMaster");
		isFastEnemy = GetComponent<CustomTag>().HasTag("FastEnemy");
		isDestructionEnemy = GetComponent<CustomTag>().HasTag("DataDestruction");
		speed = startSpeed;
        health = startHealth;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update()
    {
		if(!isFastEnemy && !isDestructionEnemy)
        {
			if (fireCountdown <= 0f)
			{
				Shoot();
				fireCountdown = 1f / fireRate;
			}

			fireCountdown -= Time.deltaTime;
		}
    }

    void OnTriggerEnter(Collider other)
    {	
		if (gameObject.GetComponent<CustomTag>().HasTag("FastEnemy"))
        {
			if (other.tag == "Plane1")
			{
				DestroyFog(other.gameObject);
			}
			else if (other.tag == "Plane2")
			{
				DestroyFog(other.gameObject);
			}
			else if (other.tag == "Plane3")
			{
				DestroyFog(other.gameObject);
			}
			else if (other.tag == "Plane4")
			{
				DestroyFog(other.gameObject);
			}
		}
    }

	void DestroyFog(GameObject other)
    {
		if (gameManager != null)
			gameManager.GetComponent<PlayerStats>().AddAttackMoney(CostReturnOnSniffingSuccess);

		other.gameObject.SetActive(false);
		Destroy(gameObject);
		GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 5f);
	}

	void UpdateTarget()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
			targetTurret = nearestEnemy.GetComponent<Turret>();
		}
		else
		{
			target = null;
		}

	}

	void Shoot()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if(target != null)
        {
			if(preferredMissile && target.GetComponent<CustomTag>().HasTag("Missile"))
			{
				bullet.damage = (int) (damage * 1.5);
				Debug.Log("Bonus Damage");
			}
			else if(preferredLaser && target.GetComponent<CustomTag>().HasTag("Laser"))
			{
				bullet.damage = (int)(damage * 1.5);
			}
			else
			{
				bullet.damage = damage;
				Debug.Log("Normal Damage");
			}
        }
		

		if (bullet != null)
		{
			bullet.Seek(target);
		}
	}

	public void TakeDamage (float amount)
	{
		health -= amount;

		healthBar.fillAmount = health / startHealth;

		if (health <= 0 && !isDead)
		{
			Die();
		}
	}

	public void Slow (float pct)
	{
		speed = startSpeed * (1f - pct);
	}

	void Die ()
	{
		isDead = true;

		PlayerStats.Money += worth;

		GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 5f);

		WaveSpawner.EnemiesAlive--;

		Destroy(gameObject);
	}

}
