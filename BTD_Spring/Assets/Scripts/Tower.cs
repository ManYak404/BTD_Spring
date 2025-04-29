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
        GameObject longestDistanceTravelledBalloon = null; // Variable to store the largest indexed balloon found

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider is a balloon
            if (collider.CompareTag("Balloon"))
            {
                if( collider.gameObject.GetComponent<Balloon>().IsSoonToPop()) // Check if the balloon is soon to pop
                {
                    continue; // Skip this balloon if it is soon to pop
                }
                if(longestDistanceTravelledBalloon == null ||
                 collider.gameObject.GetComponent<Balloon>().distanceTravelled > longestDistanceTravelledBalloon.GetComponent<Balloon>().distanceTravelled)
                {
                    longestDistanceTravelledBalloon = collider.gameObject; // Update the largest indexed balloon
                }
            }
        }

        return longestDistanceTravelledBalloon; // Return the largest indexed balloon found
    }

    void FireDart()
    {
        GameObject targetBalloon = NextBalloon(); // Get the next balloon to shoot at
        if (targetBalloon != null)
        {
            GameObject dart = Instantiate(dartPrefab, transform.position, Quaternion.identity); // Create a new dart object
            dart.GetComponent<Dart>().SetTarget(targetBalloon); // Set the target for the dart to follow
            if(targetBalloon.GetComponent<Balloon>().IsBalloonEmptyOnPop()) // Check if the balloon is dead on pop
            {
                targetBalloon.GetComponent<Balloon>().SetBalloonToDieSoon(); // Set the soonToDie flag to true for the balloon
            }
        }
    }
}
