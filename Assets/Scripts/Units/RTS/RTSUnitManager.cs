using UnityEngine;
using System.Collections.Generic;
using Pathfinding.Util;
using System.Linq;
using Unity.Mathematics;

public class RTSUnitManager
{
	public readonly List<RTSUnit> units = new List<RTSUnit>();

	public void OnDestroy()
	{
	}

	public void AddUnit(RTSUnit unit)
	{
		units.Add(unit);
	}

	public void RemoveUnit(RTSUnit unit)
	{
		units.Remove(unit);
	}

	public void MoveGroupTo(List<RTSUnit> group, Vector3 destination, bool userOrder, MovementMode mode)
	{
		if (group.Count == 0) return;

		var positions = group.Select(u => u.transform.position).ToList();

		var previousMean = Vector3.zero;
		for (int i = 0; i < positions.Count; i++) previousMean += positions[i];
		previousMean /= positions.Count;

		var standardDeviation = Mathf.Sqrt(group.Average(u => Vector3.SqrMagnitude(u.transform.position - previousMean)));
		var thresholdDistance = standardDeviation * 1.0f;

		if (Vector3.Distance(destination, previousMean) > thresholdDistance)
		{
			//Pathfinding.PathUtilities.GetPointsAroundPointWorldFlexible(destination, Quaternion.identity, positions);
			Pathfinding.PathUtilities.FormationPacked(positions, destination, group[0].Radius * 1.1f, new NativeMovementPlane(quaternion.identity));
		}
		else
		{
			//Pathfinding.PathUtilities.GetPointsAroundPointWorld(destination, AstarPath.active.data.recastGraph, positions, 0, 0.5f * 2);
			for (int i = 0; i < positions.Count; i++) positions[i] = destination;
		}

		for (int i = 0; i < group.Count; i++)
		{
			group[i].SetDestination(positions[i], mode);
		}
	}
}

public enum MovementMode
{
	Move,
	AttackMove
}
