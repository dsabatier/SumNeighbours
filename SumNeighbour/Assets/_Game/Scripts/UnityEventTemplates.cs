using System;
using UnityEngine;
using UnityEngine.Events;
using SumNeighbours;
using Noodlepop;

// Templates for UnityEvents.

#region Game specific types:
/// <summary>
/// A Node
/// </summary>
[Serializable] public class UnityEventNode : UnityEvent<Node> { }

/// <summary>
/// Old Value, New Value
/// </summary>
[Serializable] public class UnityEventMoveMade : UnityEvent<Node, int, int> { }

/// <summary>
/// UnityEvent: Old GameState, New GameState
/// </summary>
[Serializable] public class UnityEventGameState : UnityEvent<GameState, GameState> { }
#endregion

#region Primitives and Unity types:
[Serializable] public class UnityEventVector3 : UnityEvent<Vector3> { }
[Serializable] public class UnityEventBool : UnityEvent<bool> { }
[Serializable] public class UnityEventString : UnityEvent<string> { }
[Serializable] public class UnityEventInt : UnityEvent<int> { }
[Serializable] public class UnityEventRay : UnityEvent<Ray> { }
#endregion