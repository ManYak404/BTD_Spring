using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject dartPrefab; // prefab for the dart object to be spawned
    private float radius = 4f;  // The radius to search for balloons within
    private float fireRate = 1f; // The rate at which the tower fires darts
    private float nextFireTime = 0f; // The time when the tower can fire again
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nextFireTime += Time.deltaTime; // Increment the next fire time by the time since the last frame
        if (nextFireTime >= fireRate) // Check if the tower can fire again
        {
            FireDart(); // Fire a dart at the next balloon
            nextFireTime = 0f; // Reset the next fire time
        }
    }
    
    GameObject NextBalloon()
    {
        // Find all colliders within the radius around the position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        GameObject largestIndexedBalloon = null; // Variable to store the largest indexed balloon found

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider is a balloon
            if (collider.CompareTag("Balloon"))
            {
                if(largestIndexedBalloon == null || collider.gameObject.GetComponent<Balloon>().index > largestIndexedBalloon.GetComponent<Balloon>().index)
                {
                    largestIndexedBalloon = collider.gameObject; // Update the largest indexed balloon
                }
            }
        }

        return largestIndexedBalloon; // Return the largest indexed balloon found
    }

    void FireDart()
    {
        Debug.Log("Firing Dart!"); // Log the firing of the dart
        GameObject targetBalloon = NextBalloon(); // Get the next balloon to shoot at
        if (targetBalloon != null)
        {
            Debug.Log("Target Balloon Found!"); // Log the target balloon found
            GameObject dart = Instantiate(dartPrefab, transform.position, Quaternion.identity); // Create a new dart object
            dart.GetComponent<Dart>().SetTarget(targetBalloon); // Set the target for the dart to follow
        }
    }
}
