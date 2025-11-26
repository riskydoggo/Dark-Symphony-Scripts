using JetBrains.Annotations;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyAction
{
    Standing,
    MovingToCabin,
    MovingToGramophone,
    Chasing,
    ListeningToMusic,
    Enraged,
    Jumpscare
}

public class MyEnemyAI : MonoBehaviour
{
    public EnemyAction currentAction;

    public Animator animator;
    public Transform player, cabinDoor;
    public Transform[] gramophoneStops, gramophones;
    public PlayerInteract playerInteract;

    public float chaseDistance = 10f, loseInterestDistance = 30f;
    public bool musicPlaying = false, hasJumpscared = false;

    public EnemyDetector detector;

    public GameObject Meshes, Eyes, JumpCam, MainCam, GameOverCam, recordPickupText, speedrunTimerText;
    public FirstPersonController FPController;
    public TMP_Text descriptionText;

    private NavMeshAgent agent;
    private Transform currentGramophoneStop;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on " + gameObject.name);
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator not assigned in the Inspector");
            return;
        }

        animator = GetComponent<Animator>();
        currentAction = EnemyAction.MovingToCabin;
    }

    void Update()
    {
        // ---- State machine ----
        switch (currentAction)
        {
            case EnemyAction.Enraged:
                Debug.Log("[STATE] Enraged");
                SetAnimatorState(isChasing: true);
                MoveTo(player.position);
                break;

            case EnemyAction.Chasing:
                Debug.Log("[STATE] Chasing");
                SetAnimatorState(isChasing: true);
                MoveTo(player.position);
                break;

            case EnemyAction.MovingToGramophone:
                Debug.Log("[STATE] MovingToGramophone");

                if (currentGramophoneStop != null)
                {
                    MoveTo(currentGramophoneStop.position);

                    Debug.Log("[INFO] Destination: " + currentGramophoneStop.position +
                              " | Remaining: " + agent.remainingDistance +
                              " | StoppingDist: " + agent.stoppingDistance);

                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.5f)
                    {
                        Debug.Log("[TRANSITION] Arrived at gramophone stop → ListeningToMusic");
                        currentAction = EnemyAction.ListeningToMusic;
                    }
                }
                break;

            case EnemyAction.ListeningToMusic:
                Debug.Log("[STATE] ListeningToMusic (dancing)");
                SetAnimatorState(isListeningToMusic: true);
                FaceGramophone();

                if (!musicPlaying)
                {
                    Debug.Log("[MUSIC] Music stopped → Switching to MovingToCabin");
                    currentAction = EnemyAction.MovingToCabin;
                    currentGramophoneStop = null;
                }
                break;

            case EnemyAction.MovingToCabin:
                Debug.Log("[STATE] MovingToCabin");
                SetAnimatorState(isWalking: true);
                MoveTo(cabinDoor.position);
                break;

            case EnemyAction.Jumpscare:
                if (!hasJumpscared) //Stops AI from going to another state after jumpscare
                {
                    hasJumpscared = true; //Stops AI from going to another state after jumpscare
                    Debug.Log("[STATE] Jumpscare");

                    StartCoroutine(JumpscareDelay());
                    SetAnimatorState(isIdle: true);
                    StopMovement();

                    JumpCam.SetActive(true);
                    MainCam.SetActive(false); //Prevents multiple audio listeners from being active

                    Meshes.SetActive(false); //Turns AI meshes off
                    Eyes.SetActive(false);
                    FPController.enabled = false;
                }

                break;

            case EnemyAction.Standing:
            default:
                Debug.Log("[STATE] Idle/Default");
                SetAnimatorState(isIdle: true);
                StopMovement();
                break;
        }

        // ---- Decision tree ----
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (hasJumpscared) return; //State will remain jumpscare for the remainder of the scene, until the player plays again
        else if (detector.isEnemyEnraged)
        {
            Debug.Log("[DECISION] Enemy is enraged → Enraged state");
            currentAction = EnemyAction.Enraged;
        }
        else if (distToPlayer < chaseDistance)
        {
            Debug.Log("[DECISION] Player within chaseDistance → Chasing");
            currentAction = EnemyAction.Chasing;
        }
        else if (musicPlaying && distToPlayer > loseInterestDistance)
        {
            if (currentAction != EnemyAction.ListeningToMusic && currentAction != EnemyAction.MovingToGramophone)
            {
                int idx = playerInteract.currentGramophoneIndexPlaying;

                if (gramophoneStops.Length > idx)
                {
                    currentGramophoneStop = gramophoneStops[idx];
                    currentAction = EnemyAction.MovingToGramophone;
                    Debug.Log("[DECISION] Music playing → MovingToGramophone: " + currentGramophoneStop.name);
                }
                else
                {
                    Debug.LogWarning("Invalid gramophone index!");
                    currentAction = EnemyAction.MovingToCabin;
                }
            }
        }
        else if (!musicPlaying)
        {
            if (currentAction != EnemyAction.MovingToCabin)
            {
                Debug.Log("[DECISION] No music playing → MovingToCabin");
                currentAction = EnemyAction.MovingToCabin;
                currentGramophoneStop = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            recordPickupText.SetActive(false);
            speedrunTimerText.SetActive(false);
            currentAction = EnemyAction.Jumpscare;
            Debug.Log("YOU GOT JUMPSCARED");

            switch (playerInteract.numRecordsFound)
            {
                case 0: descriptionText.text = "Did you even try?"; break;
                case 1: descriptionText.text = "Your wife is next."; break;
                case 2: descriptionText.text = "His rough hands grasp your throat. You lay lifeless."; break;
                case 3: descriptionText.text = "Only 3? Your wife would be ashamed."; break;
                case 4: descriptionText.text = "You were no match for that thing."; break;
                case 5: descriptionText.text = "5 vinyls. Not good enough."; break;
                case 6: descriptionText.text = "So close. He doesn't care."; break;
            }
        }

        if (other.CompareTag("CabinDoor"))
        {
            recordPickupText.SetActive(false);
            speedrunTimerText.SetActive(false);
            currentAction = EnemyAction.Jumpscare;
            Debug.Log("Enemy reached the cabin door");
            StopMovement();

            switch (playerInteract.numRecordsFound)
            {
                case 0: descriptionText.text = "Your wife was murdered in cold blood. Did you even try?"; break;
                case 1: descriptionText.text = "You wife's screams echo through the forest."; break;
                case 2: descriptionText.text = "She knows it was your fault. Use your flashlight."; break;
                case 3: descriptionText.text = "You're too late. You drop to your knees in agony."; break;
                case 4: descriptionText.text = "There's no one else to blame."; break;
                case 5: descriptionText.text = "You chose your life over hers."; break;
                case 6: descriptionText.text = "Your wife was slaughtered on your way to the fire. Too slow."; break;
            }
        }
    }

    void SetAnimatorState(bool isIdle = false, bool isWalking = false, bool isListeningToMusic = false, bool isChasing = false)
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isListeningToMusic", isListeningToMusic);
        animator.SetBool("isChasing", isChasing);
    }

    void FaceGramophone()
    {
        if (playerInteract.currentGramophoneIndexPlaying < gramophones.Length)
        {
            Vector3 direction = gramophones[playerInteract.currentGramophoneIndexPlaying].position - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }

    private void MoveTo(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPosition);
        }
    }

    private void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    private IEnumerator JumpscareDelay()
    {
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Game Over Screen");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        JumpCam.SetActive(false); //Prevents multiple audio sources from being active
        GameOverCam.SetActive(true); 
        
    }
}
