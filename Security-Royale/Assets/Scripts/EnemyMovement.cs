using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{ 
	public bool move;

	private bool isFastEnemy;
	private bool isDestructionEnemy;

	private Enemy enemy;

    private GameObject goal;
    private NavMeshAgent agent;

    void Start()
	{
		isFastEnemy = GetComponent<CustomTag>().HasTag("FastEnemy");
		isDestructionEnemy = GetComponent<CustomTag>().HasTag("DataDestruction");
		move = true;

		enemy = GetComponent<Enemy>();

        goal = GameObject.FindGameObjectWithTag("End");
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.transform.position;
    }

	void Update()
	{
		if(!isFastEnemy && !isDestructionEnemy)
        {
			if (move)
			{
				if (agent.isStopped)
					agent.isStopped = false;
				enemy.speed = enemy.startSpeed;
			}
			else
			{
				agent.isStopped = true;
			}
		}
		else
        {
			enemy.speed = enemy.startSpeed;
		}

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

	void EndPath()
	{
		WaveSpawner.EnemiesAlive--;
		Destroy(gameObject);
		if (GetComponent<CustomTag>().HasTag("DataDestruction"))
        {
			PlayerStats.Lives--;
		}
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
