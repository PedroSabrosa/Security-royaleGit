using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{

	private Transform target;
	private int wavepointIndex = 0;

	public bool move;

	private Enemy enemy;

    //-----------------------
    private GameObject goal;
    private NavMeshAgent agent;

    void Start()
	{
		move = true;

		enemy = GetComponent<Enemy>();

		//target = Waypoints.points[0];

        //-----------------------------
        goal = GameObject.FindGameObjectWithTag("End");
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.transform.position;
    }

	void Update()
	{
		if (move)
		{
			//Vector3 dir = target.position - transform.position;
			//transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

			//if (Vector3.Distance(transform.position, target.position) <= 0.4f)
			//{
			//	GetNextWaypoint();
			//}
			if (agent.isStopped)
				agent.isStopped = false;
			enemy.speed = enemy.startSpeed;
		}
		else
        {
			agent.isStopped = true;
        }

        //-----------------------------------------------------------------------
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    EndPath();
                    return;
                }
            }
        }
    }

	//void GetNextWaypoint()
	//{
	//	if (wavepointIndex >= Waypoints.points.Length - 1)
	//	{
	//		EndPath();
	//		return;
	//	}

	//	wavepointIndex++;
	//	target = Waypoints.points[wavepointIndex];
	//}

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
