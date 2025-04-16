using UnityEngine;

public class Balloon : MonoBehaviour
{
    public Vector3[] waypoints;// path waypoints for balloon to follow
    private int currentWaypointIndex = 0; // index of the current waypoint the balloon is moving towards
    public float speed = 1f; // speed of the balloon
    public float health = 100f; // health of the balloon
    public int index; // index of the current balloon in the queue
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = Manager.ManagerInstance.waypoints; // get the waypoints from the manager instance
        index = Manager.ManagerInstance.numberOfBalloons; // get the current balloon index from the manager instance
        Manager.ManagerInstance.numberOfBalloons++; // increment manager instance balloon index
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
            transform.up = waypoints[currentWaypointIndex] - transform.position; // rotate the balloon to face the waypoint
            
            // check if the balloon has reached the current waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex]) < 0.01f)
            {
                currentWaypointIndex++; // move to the next waypoint
            }
        }
        //balloon has reached the end of the path, reduce health or end game
        else
        {
            DestroyBalloon(); // destroy the balloon object
        }
    }

    void DestroyBalloon()
    {
        Manager.ManagerInstance.BalloonReachedEnd(gameObject); // call dequeue in the manager to handle destruction
        Destroy(gameObject); // destroy the balloon object
    }

    public void HitByDart(float damage)
    {
        // reduce the health of the balloon by the damage amount
        health -= damage; // reduce the health of the balloon by the damage amount
        if (health <= 0) // check if the balloon is dead
        {
            DestroyBalloon(); // destroy the balloon object
        }
    }
}
