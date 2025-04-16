using System.Collections;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public static Manager ManagerInstance; // Singleton reference to this manager object available to all other scripts
    public Vector3[] waypoints;   // path waypoints for balloons to follow
    public GameObject balloonPrefab; // prefab for the balloon object to be spawned
    public int numberOfBalloons = 0;
    public GameObject path;
    private int numWaypoints = 10; // number of waypoints in the path
    public GameObject towerPrefab; // prefab for the tower object to be spawned
    public GameObject cursorTower; // cursor icon object to show where tower will be spawned
    private bool isTowerBeingPlaced = false; // flag to check if a tower is being placed

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
        cursorTower.GetComponent<SpriteRenderer>().enabled = false; // hide the cursor tower icon at the start
        cursorTower.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f); // set the cursor tower icon to be transparent
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
        if(Input.GetKeyDown(KeyCode.T)) // check if the space key is pressed
        {
            isTowerBeingPlaced = !isTowerBeingPlaced; // toggle the tower placement mode
        }

        if(isTowerBeingPlaced)
        {
            TowerPlacement(); // cursor tower icon management and tower placement
        }
    }

    public void BalloonReachedEnd(GameObject balloon)
    {
        // handle the event when a balloon reaches the end of the path
        Debug.Log("Balloon reached the end of the path!");
    }

    public void SpawnBalloon()
    {
        // spawn a new balloon at the start of the path
        GameObject newBalloon = Instantiate(balloonPrefab, waypoints[0], Quaternion.identity);
    }

    public bool IsValidTowerPlacement(Vector3 position)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(position);
        // check if the tower is in bounds
        if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > Screen.height)
        {
            return false; // tower cannot be placed outside the screen bounds
        }

        // check if the tower is colliding with the path or any of its children
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            Transform current = hit.collider.transform;
            while (current != null)
            {
                if (current.gameObject == path)
                {
                    return false; // tower cannot be placed on the path or any of its children
                }
                current = current.parent;
            }
        }
        return true;
    }

    public void TowerPlacement()
    {
        cursorTower.GetComponent<SpriteRenderer>().enabled = true; // show the cursor tower icon
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get the mouse position in world coordinates
        mousePos.z = 0; // set the z coordinate to 0 to keep the tower on the same plane as the path
        cursorTower.transform.position = mousePos; // move the cursor tower icon to the mouse position
        bool isValidTowerPlacement = IsValidTowerPlacement(mousePos); // check if the tower can be placed at the mouse position
        if(isValidTowerPlacement)
        {
            cursorTower.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1f); // set the cursor tower icon to be green if the tower can be placed
        }
        else
        {
            cursorTower.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.2f); // set the cursor tower icon to be red if the tower cannot be placed
        }
        if(Input.GetMouseButtonDown(0)) // check if the left mouse button is clicked
        {
            if(isValidTowerPlacement) // check if the tower can be placed at the mouse position
            {
                GameObject newTower = Instantiate(towerPrefab, mousePos, Quaternion.identity); // spawn a new tower at the mouse position
                isTowerBeingPlaced = false; // exit tower placement mode
            }
        }
    }
}
