using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(CapsuleCollider))]
public class EnemyBaheviour : MonoBehaviour
{
	#region declarations
	public float Health = 100f;
	[Range(0, 10)]
	public int SoulCost = 1;
	public GameObject SoulPrefab;
	[Space]
	[Range(0, 100)]
	public int DropVerity = 9;
	public DropWeapon[] DropWeapons;
	[Space]
	public Weapon WeaponType;
	public enum Weapon { ColdWeapon, Laser, Granate};
	public float Damage = 1f, Range = 3f;
	

	float timeToFindHero = 1f;
	NavMeshAgent agent;
    #endregion

    private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.stoppingDistance = Range - 1f;
		StartCoroutine(FindHero());

		WaveManager.EnemiesLeft++;
	}

	public void TakeDamage(float damage)
	{
		Health -= damage;
		if (Health < 0f) Destroy(gameObject);
	}

	IEnumerator FindHero()
	{
		while(true)
		{
			agent.SetDestination(Camera.main.transform.position);
			yield return new WaitForSeconds(timeToFindHero);
		}
	}

	private void OnDestroy()
	{
		if (Health <= 0)
		{
			WaveManager.EnemiesLeft--;
			ProgressManager.Killed++;

			if (SoulCost > 0)
			{
				GameObject soul = Instantiate(SoulPrefab, transform.position, transform.rotation);
				soul.GetComponent<Soul>().SoulCost = SoulCost;
				soul.transform.localScale = Vector3.one * (1 + SoulCost / 10);
			}
			DropWeapon();
		}
	}

	void DropWeapon()
	{
		int currentVerity = UnityEngine.Random.Range(0, 100);
		if (currentVerity <= DropVerity)
		{
			// sum verity
			int weaponSum = 0;
			foreach (var item in DropWeapons) weaponSum += item.DropVerityCoef;

			// choose weapon to drop
			int weaponRandom = UnityEngine.Random.Range(0, weaponSum);
			int weaponCheckSum = 0;
			for (int i = 0; i < DropWeapons.Length; i++)
			{
				if (weaponCheckSum <= weaponRandom && weaponRandom < weaponCheckSum + DropWeapons[i].DropVerityCoef)
				{
					Instantiate(DropWeapons[i].WeaponPrefab, transform.position, transform.rotation);
				}
				weaponCheckSum += DropWeapons[i].DropVerityCoef;
			}
		}
	}

	public void Shoot() // on animation
	{
		if (WeaponType.Equals("Laser"))
		{ 
		}
		else if (WeaponType.Equals("Granate"))
		{
		}
		else
		{
			Transform cam = Camera.main.transform;
			Vector3 toTarget = cam.position - transform.position;
			if (Vector3.Distance(transform.position, cam.position) < Range &&
				Vector3.Angle(toTarget, transform.forward) < 90f) 
				FindObjectOfType<PlayerParameters>().TakeDamage(Damage);
		}
	}
}

[Serializable]
public class DropWeapon
{
	public GameObject WeaponPrefab;
	public int DropVerityCoef = 10;
}
