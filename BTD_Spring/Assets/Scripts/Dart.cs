using Unity.VisualScripting;
using UnityEngine;

public class Dart : MonoBehaviour
{
    float speed = 20f; // Speed of the dart
    GameObject targetBalloon; // Target position for the dart to move towards
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget(); // Move the dart towards the target position
    }

    public void SetTarget(GameObject inputTargetBalloon)
    {
        // Set the target for the dart to follow
        targetBalloon = inputTargetBalloon; // Get the position of the target balloon
    }

    void MoveTowardsTarget()
    {
        // Move the dart towards the target position
        transform.up = targetBalloon.transform.position - transform.position; // Rotate the dart to face the target balloon
        transform.position = Vector3.MoveTowards(transform.position, targetBalloon.transform.position, speed * Time.deltaTime); // Move the dart towards the target balloon
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the dart has collided with a balloon
        if (other.gameObject.CompareTag("Balloon"))
        {
            Debug.Log("Dart hit the balloon!"); // Log the collision
            other.gameObject.GetComponent<Balloon>().HitByDart(25f); // Call the HitByDart method on the balloon to reduce its health
            Destroy(gameObject); // Destroy the dart object
        }
    }
}
