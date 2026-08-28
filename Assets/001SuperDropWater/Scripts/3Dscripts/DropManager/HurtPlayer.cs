using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
	public int value;
	public int damageToGive = 1;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			//Quand tu te fait toucher//
			Vector3 hitDirection = other.transform.position - transform.position;
			hitDirection = hitDirection.normalized;
			LifeManager lifeManager = Object.FindFirstObjectByType<LifeManager>();
			if (lifeManager != null)
			{
				lifeManager.damagePlayer(damageToGive, hitDirection);
			}
		}
	}
}