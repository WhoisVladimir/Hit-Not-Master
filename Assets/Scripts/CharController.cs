using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CharController : MonoBehaviour
{
    [SerializeField] private GameObject moveTarget;

    private NavMeshAgent agent;
    private CharacterController character;
    private List<Rigidbody> rigidbodies;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<CharacterController>();
        rigidbodies = new List<Rigidbody>();
        rigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());
        Debug.Log(rigidbodies.Count);
        SwitchRbKinematic(true);

    }

    private void SwitchRbKinematic(bool isActive)
    {
        foreach (var item in rigidbodies)
        {
            item.isKinematic = isActive;
        }
    }

    public async UniTask StartMovingAsync(Vector3 waypoint, CancellationToken cancellationToken)
    {
        Debug.Log("Start new Move");
        var isPointReached = agent.SetDestination(waypoint);

        await UniTask.WaitUntil(() => agent.remainingDistance == 0, PlayerLoopTiming.Update, cancellationToken);
        Debug.Log(agent.remainingDistance);
        Debug.Log("Stop moving");
        
    }

}
