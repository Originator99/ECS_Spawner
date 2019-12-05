using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnerAuthering : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
	[SerializeField] private GameObject prefab;
	[SerializeField] private float spawnRate;
	[SerializeField] private float maxDistanceFromSpawner;
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
		dstManager.AddComponentData(entity, new Spawner {
			prefab = conversionSystem.GetPrimaryEntity(prefab),
			maxDistanceFromSpawner = maxDistanceFromSpawner,
			spawnDelay = 1 / spawnRate,
			nextSpawnTime = 0
		});
		
	}

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
		referencedPrefabs.Add(prefab);
	}
}
