using UnityEngine;

public class Balloon : MonoBehaviour
{
    public Vector3[] waypoints;// path waypoints for balloon to follow
    private BalloonData data; // balloon data for the current balloon
    private int currentWaypointIndex = 0; // index of the current waypoint the balloon is moving towards
    public float distanceTravelled = 0f; // distance traveled by the balloon
    public float health; // health of the balloon
    public bool soonToPop = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = Manager.ManagerInstance.waypoints; // get the waypoints from the manager instance
        transform.up = waypoints[currentWaypointIndex] - transform.position; // rotate the balloon to face the waypoint\
    }

    // Update is called once per frame
    void Update()
    {
        MoveBalloonTowardsWaypoint(); //move the balloon towards the current waypoint
    }

    public void InitializeFromData(BalloonData inputData)
    {
        data = inputData; // set the balloon data for the current balloon
        health = data.health; // set the health of the balloon
        VisualizeBalloon(); // visualize the balloon with the current type
    }

    void MoveBalloonTowardsWaypoint()
    {
        if(currentWaypointIndex < waypoints.Length)
        {
            // old position of balloon for distance calculation
            Vector3 oldPosition = transform.position;

            // move the balloon towards the current waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex], data.speed * Time.deltaTime);
            transform.up = waypoints[currentWaypointIndex] - transform.position; // rotate the balloon to face the waypoint

            // add distance travelled to the total distance travelled by the balloon
            float delta = Vector3.Distance(transform.position, oldPosition);
            distanceTravelled += delta;
            
            // check if the balloon has reached the current waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex]) < 0.01f)
            {
                currentWaypointIndex++; // move to the next waypoint
            }
        }

        //balloon has reached the end of the path, reduce health or end game
        else
        {
            Manager.ManagerInstance.BalloonReachedEnd(gameObject); // call dequeue in the manager to handle destruction
        }
    }

    void DestroyBalloon()
    {
        Destroy(gameObject); // destroy the balloon object
    }

    public void HitByDart()
    {
        if(health > 1)
        {
            health -= 1; // reduce the health of the balloon by 1
        }
        else
        {
            health = 0; // set the health to 0 if it is less than 1
            PopBalloon(); // call the pop balloon method to handle popping the balloon
        }
    }
    public void VisualizeBalloon()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/BalloonSprites/balloon-" + data.name); // load the sprite for the balloon type
    }

    public bool IsBalloonEmptyOnPop()
    {
        return data.nextBalloonsOnDestroy.Length <= 0; // return true if the balloon has no next balloons on destroy
    }

    public bool IsSoonToPop()
    {
        return soonToPop; // return the soonToPop status of the balloon
    }

    public void SetBalloonToDieSoon()
    {
        soonToPop = true; // set the soonToPop to true
    }

    public void PopBalloon()
    {
        // balloon popped, reduce health or spawn other balloons
        if (IsBalloonEmptyOnPop())
        {
            DestroyBalloon(); // destroy the balloon object if it has no next balloons on destroy
        }
        else
        {
            // spawn other balloons on pop
            foreach (string nextBalloon in data.nextBalloonsOnDestroy)
            {
                GameObject newBalloon = Manager.ManagerInstance.SpawnBalloon(transform, nextBalloon); // spawn the next balloon type
                newBalloon.GetComponent<Balloon>().distanceTravelled = distanceTravelled; // set the distance travelled for the new balloon
                newBalloon.GetComponent<Balloon>().currentWaypointIndex = currentWaypointIndex; // set the current waypoint index for the new balloon
            }
            DestroyBalloon(); // destroy the current balloon object
        }
        soonToPop = false; // reset the soonToPop status of the balloon
    }
}
