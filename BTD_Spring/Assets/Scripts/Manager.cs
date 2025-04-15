using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager ManagerInstance; // Singleton reference to this manager object available to all other scripts
    public Vector3[] waypoints;   // path waypoints for balloons to follow
    private float cameraHeight = 10f; // height of the camera in world units
    public GameObject balloonPrefab; // prefab for the balloon object to be spawned
    public GameObject path;
    private int numWaypoints = 10; // number of waypoints in the path

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
        waypoints = new Vector3[numWaypoints]; // Initialize the waypoints array with 5 elements
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = path.transform.Find("waypoint" + i).position; // Initialize each waypoint
        }
        SpawnBalloon(); // spawn the first balloon at the start of the path

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

    public void SpawnBalloon()
    {
        // spawn a new balloon at the start of the path
        GameObject newBalloon = Instantiate(balloonPrefab, waypoints[0], Quaternion.identity);
        //newBalloon.transform.SetParent(transform); // set the balloon as a child of the manager object
    }
}
