using MoreMountains.TopDownEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : TimedSpawner
{
	[MinMaxSlider(-7, 7)]
	public Vector2 xMinMaxPosition;
	[MinMaxSlider(-3, 5)]
	public Vector2 zMinMaxPosition;

	protected override GameObject Spawn()
	{
		GameObject spawnedPowerUp =  base.Spawn();
		if (spawnedPowerUp != null)
			spawnedPowerUp.transform.position = new Vector3(Random.Range(xMinMaxPosition.x, xMinMaxPosition.y), 0, Random.Range(zMinMaxPosition.x, zMinMaxPosition.y));

		return spawnedPowerUp;
	}
}
