using UnityEngine;

public class Bullet : MonoBehaviour
{

	private Transform target;

	public float speed = 70f;

	public int damage;

	public float explosionRadius = 0f;
	public GameObject impactEffect;

	public void Seek(Transform _target)
	{
		target = _target;
	}

	// Update is called once per frame
	void Update()
	{

		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);

	}

	void HitTarget()
	{
		GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(effectIns, 5f);

		if (explosionRadius > 0f)
		{
			Explode();
		}
		else
		{
			Damage(target);
		}

		Destroy(gameObject);
	}

	void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy" || collider.tag == "Turret")
			{
				Damage(collider.transform);
			}
		}
	}

	void Damage(Transform entity)
	{
		if (entity.tag == "Enemy")
		{
			Enemy e = entity.GetComponent<Enemy>();

			if (e != null)
			{
				e.TakeDamage(damage);
			}
		}
		else if (entity.tag == "Turret")
		{
			Turret t = entity.GetComponent<Turret>();

			if (t != null)
			{
				t.TakeDamage(damage);
			}
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
