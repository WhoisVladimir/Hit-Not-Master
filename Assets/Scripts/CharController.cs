using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;

//public delegate void InputAction(int num);
public class CharController : MonoBehaviour
{
    private delegate bool InputAction(int num);
    private InputAction inputAction;

    [SerializeField] private GameObject shotPoint;
    [SerializeField] private GameObject projectile;
    private NavMeshAgent agent;
    private Camera cam;
    private bool isFight;

    Vector3 testPoint;

    private void Awake()
    {
#if UNITY_EDITOR
        inputAction = Input.GetMouseButtonDown;
#endif

#if UNITY_ANDROID
        inputAction = Input.GetTouch;
#endif
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    public async UniTask StartMovingAsync(Vector3 waypoint, CancellationToken cancellationToken)
    {
        var isPointReached = agent.SetDestination(waypoint);

        await UniTask.WaitUntil(() => agent.remainingDistance == 0, PlayerLoopTiming.Update, cancellationToken);
    }

    private void Update()
    {
        if (inputAction(0) && isFight) AimShoot();
    }
    public void SwitchFightStatus(bool isFight)
    {
        this.isFight = isFight;
    }

    private void AimShoot()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPosition;
        if (Physics.Raycast(ray, out var hit, 100)) targetPosition = hit.point;
        else
        {
            var mouseX = Input.mousePosition.x;
            var mouseY = Input.mousePosition.y;
            targetPosition = cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.farClipPlane));
        }

        var targetDirection = (targetPosition - shotPoint.transform.position).normalized;
        var instancePos = shotPoint.transform.position;
        var proj = Instantiate(projectile, instancePos, Quaternion.Euler(Vector3.up * -90));
        var projectileCtrl = proj.GetComponent<ProjectileController>();
        projectileCtrl.Shoot(targetDirection);

        testPoint = targetDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(shotPoint.transform.position, testPoint);
    }
}
