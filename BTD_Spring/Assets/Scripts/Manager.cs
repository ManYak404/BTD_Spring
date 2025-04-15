using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager ManagerInstance; // Singleton reference to this manager object available to all other scripts
    public Vector3[] waypoints;   // path waypoints for balloons to follow
    private float cameraHeight = 10f; // height of the camera in world units

    void Awake()
    {
        //ensure there is only one manager instance in the scene
        if (ManagerInstance == null)
        {
            ManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = new Vector3[5]; // Initialize the waypoints array with 5 elements
        waypoints[0] = Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 0.5f, cameraHeight)); // Set the first waypoint at (0.2, 0.5)
        waypoints[1] = Camera.main.ViewportToWorldPoint(new Vector3(0.4f, 0.5f, cameraHeight)); // Set the first waypoint at (0.4, 0.5)
        waypoints[2] = Camera.main.ViewportToWorldPoint(new Vector3(0.4f, 0.8f, cameraHeight)); // Set the first waypoint at (0.4, 0.8)
        waypoints[3] = Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 0.3f, cameraHeight)); // Set the first waypoint at (0.7, 0.3)
        waypoints[4] = Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.5f, cameraHeight)); // Set the first waypoint at (0.8, 0.5)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BalloonReachedEnd()
    {
        // handle the event when a balloon reaches the end of the path
        Debug.Log("Balloon reached the end of the path!");
    }
}
