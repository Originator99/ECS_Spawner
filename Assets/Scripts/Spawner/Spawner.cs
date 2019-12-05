using Unity.Entities;

public struct Spawner : IComponentData {
	public Entity prefab;
	public float maxDistanceFromSpawner;
	public float spawnDelay;
	public float nextSpawnTime;
}
