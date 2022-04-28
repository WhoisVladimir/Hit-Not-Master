using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] waypoints;

    private CharController charController;

    private CancellationTokenSource cancellationTokenSource;


    private void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
        if(waypoints != null && waypoints.Length > 0)
        {
            player.transform.position = waypoints[0].position;
        }

        charController = player.GetComponent<CharController>();

        FollowPath();
    }

    private async void FollowPath()
    {
        var cancellationToken = cancellationTokenSource.Token;
        for (int i = 1; i < waypoints.Length; i++)
        {
            await charController.StartMovingAsync(waypoints[i].position, cancellationToken);
            Debug.Log($"Point {i} is reached.");

            await StartFightAsync(cancellationToken);
        }

    }

    private async UniTask StartFightAsync(CancellationToken cancellationToken)
    {
        var layer = LayerMask.GetMask("Enemy");
        var enemies = Physics.OverlapSphere(player.transform.position, 10, layer);
        Debug.Log($"Enemies count: {enemies.Length}");

        var enemiesCount = enemies.Length;

        var enemyLogic = new EnemyController[enemiesCount];
        var observedTasks = new UniTask[enemiesCount];

        for (int i = 0; i < enemiesCount; i++)
        {
            Debug.Log($"Loop {i}");
            enemyLogic[i] = enemies[i].GetComponent<EnemyController>();
            var tempIndex = i;
            observedTasks[tempIndex] = UniTask.WaitWhile(() => enemyLogic[tempIndex].Health > 0, PlayerLoopTiming.Update, cancellationToken);
        }

        charController.SwitchFightStatus(true);

        await UniTask.WhenAll(observedTasks);
        charController.SwitchFightStatus(false);
    }

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.transform.position, 10);
    }
#endif

}
