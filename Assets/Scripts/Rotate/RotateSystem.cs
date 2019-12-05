using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;

public class RotateSystem : JobComponentSystem {
	//protected override void OnUpdate() {
	//	Entities.ForEach((ref Rotate rotate, ref RotationEulerXYZ euler) => {
	//		euler.Value.y += rotate.radiansPerSecond * Time.DeltaTime;
	//	});
	//}

	[BurstCompile]
	private struct RotateJob : IJobForEach<RotationEulerXYZ, Rotate> {
		public float deltaTime;

		public void Execute(ref RotationEulerXYZ euler, ref Rotate rotate) {
			euler.Value.y += rotate.radiansPerSecond * deltaTime;
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps) {
		var rotate = new RotateJob { deltaTime = Time.DeltaTime };
		return rotate.Schedule(this, inputDeps);
	}
}
