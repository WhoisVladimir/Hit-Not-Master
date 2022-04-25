using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

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

            await Task.Delay(2000, cancellationToken);
        }

    }

    //private async Task

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }
}
