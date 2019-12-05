using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;
using Time = UnityEngine.Time;

public class SpawnerSystem : JobComponentSystem {
	private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem; //stall a buffer until its done

	protected override void OnCreate() {
		endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>(); // if there is no system we create or if there is , we get.
	}

	private struct SpawnerJob : IJobForEachWithEntity<Spawner, LocalToWorld> { //cant use burst compiler here for now
		private EntityCommandBuffer.Concurrent entityCommandBuffer;
		private Random random;
		private readonly float deltaTime;
		public SpawnerJob(EntityCommandBuffer.Concurrent commandBuffer, Random random , float time) {
			entityCommandBuffer = commandBuffer;
			this.random = random;
			deltaTime = time;
		}
		public void Execute(Entity entity, int index, ref Spawner spawner, [ReadOnly] ref LocalToWorld localToWorld) {
			spawner.nextSpawnTime -= deltaTime;
			if (spawner.nextSpawnTime <= 0) {
				spawner.nextSpawnTime += spawner.spawnDelay;
				Entity instance = entityCommandBuffer.Instantiate(index, spawner.prefab);
				entityCommandBuffer.SetComponent(index, instance, new Translation {
					Value = localToWorld.Position + random.NextFloat3Direction() * random.NextFloat() * spawner.maxDistanceFromSpawner
				});
			}
		}
	}
	protected override JobHandle OnUpdate(JobHandle inputDeps) {
		var spawnerJob = new SpawnerJob(endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(), 
			new Random((uint)UnityEngine.Random.Range(0, int.MaxValue)), 
			Time.DeltaTime);
		JobHandle jobhandle = spawnerJob.Schedule(this, inputDeps);
		endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobhandle);
		return jobhandle;
	}
}
