using UnityEngine;
using System.Collections;

public class EnemyDetector : MonoBehaviour
{
    public GameObject enemy; // Reference to the enemy object
    public Light flashlightLight; // Reference to the flashlight's Light component
    public LayerMask obstacleLayer; // Layer mask for obstacles
    public bool isEnemyEnraged = false;
    public float enragementDuration = 5f; // Duration for which the enemy remains enraged
    private Coroutine enragementCoroutine;
    public GameObject Eyes;

    private void Start()
    {
        Eyes.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == enemy && flashlightLight.enabled && HasLineOfSight())
        {
            EnrageEnemy();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == enemy && flashlightLight.enabled && HasLineOfSight())
        {
            EnrageEnemy();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == enemy)
        {
            if (enragementCoroutine != null)
            {
                StopCoroutine(enragementCoroutine);
            }
            enragementCoroutine = StartCoroutine(ResetEnragedStateAfterDelay(enragementDuration));
            Debug.Log("Enragement coroutine started on trigger exit.");


        }
    }

    private void EnrageEnemy()
    {
        if (!isEnemyEnraged)
        {
            isEnemyEnraged = true;
            //Debug.Log("Enemy enraged.");
        }

        if (enragementCoroutine != null)
        {
            StopCoroutine(enragementCoroutine);
            enragementCoroutine = null;
            Debug.Log("Existing enragement coroutine stopped.");
        }
    }

    private IEnumerator ResetEnragedStateAfterDelay(float delay)
    {
        Debug.Log("ResetEnragedStateAfterDelay coroutine started.");
        yield return new WaitForSeconds(delay);
        isEnemyEnraged = false;
        enragementCoroutine = null;
        Debug.Log("Enemy no longer enraged after delay.");
        StartCoroutine(EyesDeactivate());
    }

    private IEnumerator EyesDeactivate()
    {
        yield return new WaitForSeconds(5);
        Eyes.SetActive(false);
    }

    private bool HasLineOfSight()
    {
        Vector3 flashlightPosition = flashlightLight.transform.position;
        Vector3 enemyPosition = enemy.transform.position + Vector3.up; // Adjust for enemy's height
        Vector3 directionToEnemy = (enemyPosition - flashlightPosition).normalized;
        float distanceToEnemy = Vector3.Distance(flashlightPosition, enemyPosition);

        // Perform a raycast to check for obstacles between the flashlight and the enemy
        if (Physics.Raycast(flashlightPosition, directionToEnemy, out RaycastHit hit, distanceToEnemy, obstacleLayer))
        {
            //Debug.Log($"No line of sight to enemy. Obstacle: {hit.collider.gameObject.name} at distance: {hit.distance}");
            Debug.DrawLine(flashlightPosition, hit.point, Color.red, 1.0f); // Visualize the raycast
            return false;
        }
        Debug.DrawLine(flashlightPosition, enemyPosition, Color.green, 1.0f); // Visualize the raycast
        //Debug.Log("Clear line of sight to enemy.");
        return true;
    }

    void Update()
    {
        if (isEnemyEnraged)
        {
            // Behavior for when the enemy is enraged by the flashlight
            //Debug.Log("Enemy is enraged by flashlight!");
            Eyes.SetActive(true);
        }
        //else
        //{
        //    // Behavior for when the enemy is not enraged by the flashlight
        //    //Debug.Log("Enemy is not enraged by flashlight.");
        //    Eyes.SetActive(false);
        //}
    }
}