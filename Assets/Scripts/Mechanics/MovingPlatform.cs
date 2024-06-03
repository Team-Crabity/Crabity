using UnityEngine;

public class PlatformerMover : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 2.0f;

    private Vector3 target;

    private void Start()
    {
        // Initialize the target to pointB
        target = pointB;

        // Log initial points and target to verify they are set correctly
        Debug.Log($"{gameObject.name} Start: pointA={pointA}, pointB={pointB}, target={target}");
    }

    private void Update()
    {
        // Move the object towards the target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Check if the object has reached the target point
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Switch the target point
            target = (target == pointA) ? pointB : pointA;

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
        Gizmos.DrawLine(pointA, pointB);
    }
}
