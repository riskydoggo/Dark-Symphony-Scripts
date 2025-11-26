using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EnemyState { Idle, MoveToCabin, ListeningToMusic, ChasingPlayer }

public class EnemyAI : MonoBehaviour
{
    public EnemyState currentState;
    public Transform cabin;
    public Transform[] gramophones;
    public Transform player;
    public float chaseDistance = 10f;
    public float loseInterestDistance = 15f;
    public float acornDetectionRadius = 10f;
    public AudioClip chaseClip;
    public Animator animator;
    public float gramophoneRange = 2f;

    private NavMeshAgent agent;
    private int currentGramophoneIndex = 0;
    private AudioSource currentGramophoneAudioSource;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null)
        {
            Debug.LogError("Animator component not assigned.");
        }
        else
        {
            Debug.Log("Animator component assigned.");
        }

        // Initialize state and move to the first gramophone
        currentState = EnemyState.ListeningToMusic;
        MoveToGramophone(currentGramophoneIndex);
    }

    private void Update()
    {
        // Handle state-specific behavior
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;

            case EnemyState.MoveToCabin:
                HandleMoveToCabinState();
                break;

            case EnemyState.ListeningToMusic:
                HandleListeningToMusicState();
                break;

            case EnemyState.ChasingPlayer:
                HandleChasingPlayerState();
                break;
        }
    }

    private void MoveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
        animator.SetBool("isMoving", true);
    }

    private void MoveToGramophone(int index)
    {
        Transform targetGramophone = gramophones[index];
        currentGramophoneAudioSource = targetGramophone.GetComponent<AudioSource>();
        MoveTo(targetGramophone.position);
    }

    private void HandleIdleState()
    {
        animator.SetBool("isMoving", false);
        // Additional logic for Idle state can be added here
    }

    private void HandleMoveToCabinState()
    {
        MoveTo(cabin.position);
        if (ReachedDestination())
        {
            currentState = EnemyState.Idle;
            animator.SetBool("isMoving", false);
        }
    }

    private void HandleListeningToMusicState()
    {
        if (currentGramophoneAudioSource != null && !currentGramophoneAudioSource.isPlaying)
        {
            currentState = EnemyState.MoveToCabin;
            MoveTo(cabin.position);
            animator.SetBool("isListening", false);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isListening", true);
            animator.SetBool("isMoving", false);
        }
    }

    private void HandleChasingPlayerState()
    {
        if (Vector3.Distance(transform.position, player.position) <= chaseDistance)
        {
            MoveTo(player.position);
            animator.SetBool("isChasing", true);
        }
        else if (Vector3.Distance(transform.position, player.position) > loseInterestDistance)
        {
            currentState = EnemyState.MoveToCabin;
            MoveTo(cabin.position);
            animator.SetBool("isChasing", false);
            animator.SetBool("isMoving", true);
        }
    }

    private bool ReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void OnAcornHit(Vector3 acornPosition)
    {
        if (currentState != EnemyState.ListeningToMusic && Vector3.Distance(transform.position, acornPosition) <= acornDetectionRadius)
        {
            currentState = EnemyState.ChasingPlayer;
            //audioSource.clip = chaseClip;
            //audioSource.Play();
            animator.SetBool("isChasing", true);
            animator.SetBool("isMoving", true);
        }
    }

    public void OnDiscPlaced(int gramophoneIndex)
    {
        currentGramophoneIndex = gramophoneIndex;
        currentState = EnemyState.ListeningToMusic;
        MoveToGramophone(gramophoneIndex);
        animator.SetBool("isListening", true);
        animator.SetBool("isMoving", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = EnemyState.ChasingPlayer;
            animator.SetBool("isChasing", true);
        }
        else if (other.CompareTag("Gramophone") && Vector3.Distance(transform.position, other.transform.position) <= gramophoneRange)
        {
            currentState = EnemyState.ListeningToMusic;
            animator.SetBool("isListening", true);
        }
    }
}
