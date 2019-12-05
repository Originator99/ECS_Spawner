using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class RotateAuthering : MonoBehaviour, IConvertGameObjectToEntity {
	[SerializeField] private float degressPerSecond;
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
		dstManager.AddComponentData(entity, new Rotate { radiansPerSecond = math.radians(degressPerSecond) });
		dstManager.AddComponentData(entity, new RotationEulerXYZ());
	}
}
