using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public LifeManager theHealthMan;
	public Renderer theRend;
	public Material cpOff;
	public Material cpOn;

    void Start()
    {
        theHealthMan = FindFirstObjectByType<LifeManager>();
    }

	public void CheckpointOn()
	{
		Checkpoint[] checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
		foreach (Checkpoint cp in checkpoints)
		{
			cp.CheckpointOff();
		}
		theRend.material = cpOn;
	}

	public void CheckpointOff()
	{
		theRend.material = cpOff;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Player"))
		{
			theHealthMan.SetSpawnPoint(transform.position);	
			CheckpointOn();
		}
	}
}
