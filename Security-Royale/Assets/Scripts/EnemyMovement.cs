using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

	private Transform target;
	private int wavepointIndex = 0;

	public bool move;

	private Enemy enemy;

	void Start()
	{
		move = true;

		enemy = GetComponent<Enemy>();

		target = Waypoints.points[0];
	}

	void Update()
	{
		if (move)
		{
			Vector3 dir = target.position - transform.position;
			transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

			if (Vector3.Distance(transform.position, target.position) <= 0.4f)
			{
				GetNextWaypoint();
			}

			enemy.speed = enemy.startSpeed;
		}
	}

	void GetNextWaypoint()
	{
		if (wavepointIndex >= Waypoints.points.Length - 1)
		{
			EndPath();
			return;
		}

		wavepointIndex++;
		target = Waypoints.points[wavepointIndex];
	}

	void EndPath()
	{
		PlayerStats.Lives--;
		WaveSpawner.EnemiesAlive--;
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Turret")
		{
			move = false;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Turret")
		{
			move = true;
		}
	}

}
