using UnityEngine;

public class Balloon : MonoBehaviour
{
    public Vector3[] waypoints;// path waypoints for balloon to follow
    private int currentWaypointIndex = 0; // index of the current waypoint the balloon is moving towards
    public float speed = 2f; // speed of the balloon
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = Manager.ManagerInstance.waypoints; // get the waypoints from the manager instance
        transform.up = waypoints[currentWaypointIndex] - transform.position; // rotate the balloon to face the waypoint
    }

    // Update is called once per frame
    void Update()
    {
        MoveBalloonTowardsWaypoint(); //move the balloon towards the current waypoint
    }

    void MoveBalloonTowardsWaypoint()
    {
        if(currentWaypointIndex < waypoints.Length)
        {
            // move the balloon towards the current waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex], speed * Time.deltaTime);
            
            // check if the balloon has reached the current waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex]) < 0.01f)
            {
                currentWaypointIndex++; // move to the next waypoint
                transform.up = waypoints[currentWaypointIndex] - transform.position; // rotate the balloon to face the waypoint
            }
        }
        //balloon has reached the end of the path, reduce health or end game
        else
        {
            Manager.ManagerInstance.BalloonReachedEnd(); // call the method in the manager to handle the event
            Destroy(gameObject); // destroy the balloon if it has reached the end of the path
        }
    }
}
