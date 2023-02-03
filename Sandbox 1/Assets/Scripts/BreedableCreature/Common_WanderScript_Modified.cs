using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
#if UNITY_EDITOR
#endif

namespace Polyperfect.Common
{
    [RequireComponent(typeof(CharacterController))]
    public class Common_WanderScript_Modified : MonoBehaviour
    {
        private const float contingencyDistance = 1f;

        [SerializeField, Tooltip("How far away from it's origin this animal will wander by itself.")]
        private float wanderZone = 10f;

        [SerializeField, Tooltip("How far this animal can sense a predator.")]
        private float awareness = 30f;

        [SerializeField, Tooltip("If true, this animal will rotate to match the terrain. Ensure you have set the layer of the terrain as 'Terrain'.")]
        private bool matchSurfaceRotation = false;

        [SerializeField, Tooltip("How fast the animnal rotates to match the surface rotation.")]
        private float surfaceRotationSpeed = 2f;

        private CharacterController characterController;
        private NavMeshAgent navMeshAgent;
        private float turnSpeed = 0f;
        public bool wanderState;
        public bool idleState;
        private float walkMoveSpeed = 10;
        private float walkTurnSpeed = 100;

        [SerializeField]
        float moveSpeed = 0f;
        bool forceUpdate = false;
        Vector3 startPosition;
        Vector3 wanderTarget;
        float idleUpdateTime;


        private void Awake()
        {
            idleState = true;
            wanderState = false;
            characterController = GetComponent<CharacterController>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            if (navMeshAgent) { navMeshAgent.stoppingDistance = contingencyDistance; }

            if (matchSurfaceRotation && transform.childCount > 0)
            {
                transform.GetChild(0).gameObject.AddComponent<Common_SurfaceRotation>().SetRotationSpeed(surfaceRotationSpeed);
            }
        }

        private void Start()
        {
            startPosition = transform.position;
            StartCoroutine(RandomStartingDelay());
        }

        void Update()
        {
            if (forceUpdate)
            {
                forceUpdate = false;
            }

            var position = transform.position;
            var targetPosition = position;

            if (wanderState)
            {
                targetPosition = wanderTarget;
                Debug.DrawLine(position, targetPosition, Color.yellow);
                FaceDirection((targetPosition - position).normalized);
                var displacementFromTarget = Vector3.ProjectOnPlane(targetPosition - transform.position, Vector3.up);
                if (displacementFromTarget.magnitude < contingencyDistance)
                {
                    wanderState = false;
                    idleState = true;
                }
            }
            else if (idleState)
            {
                if (Time.time >= idleUpdateTime)
                {
                    HandleBeginWander();
                    wanderState = true;
                    idleState = false;
                }
            }

            if (navMeshAgent)
            {
                navMeshAgent.destination = targetPosition;
                navMeshAgent.speed = moveSpeed;
                navMeshAgent.angularSpeed = turnSpeed;
            }
            else
                characterController.SimpleMove(moveSpeed * UnityEngine.Vector3.ProjectOnPlane(targetPosition - position, Vector3.up).normalized);
        }

        void FaceDirection(Vector3 facePosition)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Vector3.RotateTowards(transform.forward,
                facePosition, turnSpeed * Time.deltaTime * Mathf.Deg2Rad, 0f), Vector3.up), Vector3.up);
        }

        void SetMoveSlow()
        {
            var minSpeed = float.MaxValue;

            var stateSpeed = walkMoveSpeed;
            if (stateSpeed < minSpeed)
            {
                minSpeed = stateSpeed;
            }

            turnSpeed = walkTurnSpeed;
            moveSpeed = minSpeed;
        }

        void HandleBeginWander()
        {
            var rand = Random.insideUnitSphere * wanderZone;
            var targetPos = startPosition + rand;
            ValidatePosition(ref targetPos);

            wanderTarget = targetPos;
            SetMoveSlow();
        }

        void ValidatePosition(ref Vector3 targetPos)
        {
            if (navMeshAgent)
            {
                NavMeshHit hit;
                if (!NavMesh.SamplePosition(targetPos, out hit, Mathf.Infinity, 1 << NavMesh.GetAreaFromName("Walkable")))
                {
                    Debug.LogError("Unable to sample nav mesh. Please ensure there's a Nav Mesh layer with the name Walkable");
                    enabled = false;
                    return;
                }

                targetPos = hit.position;
            }
        }

        IEnumerator RandomStartingDelay()
        {
            yield return new WaitForSeconds(Random.Range(0f, 2f));
            StartCoroutine(ConstantTicking(Random.Range(.7f, 1f)));
        }

        IEnumerator ConstantTicking(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}