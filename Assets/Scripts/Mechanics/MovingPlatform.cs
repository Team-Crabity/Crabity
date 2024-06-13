using UnityEngine;

public class PlatformerMover : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;

    private Vector3 target;

    private bool pointBIsTarget;
    private bool pointAIsTarget;

    private void Start()
    {
        // Initialize the target to pointB
        pointBIsTarget = true;
        pointAIsTarget = false;

        // Log initial points and target to verify they are set correctly
        Debug.Log($"{gameObject.name} Start: pointA={pointA}, pointB={pointB}, target={target}");
    }

    private void Update()
    {
        // Move the object towards the target
        target = pointAIsTarget ? pointA.position : pointB.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Check if the object has reached the target point
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Switch the target point
            pointAIsTarget = !pointAIsTarget;
            pointBIsTarget = !pointBIsTarget;

            // Log target switching to verify behavior
            Debug.Log($"{gameObject.name} Switch Target: new target={target}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    // Optional: Draw the movement path in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
