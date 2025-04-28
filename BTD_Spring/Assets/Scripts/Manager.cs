using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Manager : MonoBehaviour
{
    public static Manager ManagerInstance; // Singleton reference to this manager object available to all other scripts
    public Vector3[] waypoints;   // path waypoints for balloons to follow
    public Dictionary<string, BalloonData> balloonDatas = new Dictionary<string, BalloonData>(); // balloon data for red balloons
    public List<BalloonData> balloonDataList = new List<BalloonData>(); // list of all balloon data
    public GameObject balloonPrefab; // prefab for the balloon object to be spawned
    public int numberOfBalloonsSpawned = 0;
    public GameObject path;
    private int numWaypoints = 10; // number of waypoints in the path
    public GameObject towerPrefab; // prefab for the tower object to be spawned
    public GameObject cursorTower; // cursor icon object to show where tower will be spawned
    private bool isTowerBeingPlaced = false; // flag to check if a tower is being placed
    private float health = 100f; // health of the player
    private int money = 100; // money of the player
    public TMP_Text moneyText; // text object to display the player's money
    public TMP_Text healthText; // text object to display the player's health
    [SerializeField] public Button buyButton; // button object to buy a tower
    private float balloonSpawnRate = 1f; // rate at which balloons are spawned
    private float balloonSpawnTimer = 0f; // timer to track the time since the last balloon was spawned

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = new Vector3[numWaypoints]; // Initialize the waypoints array with 5 elements
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = path.transform.Find("waypoint" + i).position; // Initialize each waypoint
        }
        LoadBalloonData(); // load the balloon data from the balloons JSON file
    }

    // Update is called once per frame
    void Update()
    {
        TowerPlacement(); // cursor tower icon management and tower placement

        balloonSpawnTimer += Time.deltaTime; // increment the balloon spawn timer
        if(balloonSpawnTimer >= balloonSpawnRate) // check if the balloon spawn timer has reached the spawn rate
        {
            SpawnBalloon(balloonDataList[Random.Range(0,balloonDataList.Count)]); // spawn a new balloon at the start of the path
            balloonSpawnTimer = 0f; // reset the balloon spawn timer
        }
        moneyText.text = "Money: " + money.ToString(); // update the money text display
        healthText.text = "Health: " + ((int)health).ToString(); // update the health text display
    }










    void LoadBalloonData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Data/balloons");

        if (jsonText != null)
        {
            BalloonDataList dataList = JsonUtility.FromJson<BalloonDataList>(jsonText.text);
            foreach (BalloonData balloon in dataList.balloons)
            {
                balloonDatas.Add(balloon.name, balloon); // add the balloon data to the dictionary using the name as the key
                balloonDataList.Add(balloon); // add the balloon data to the list
            }
        }
        else
        {
            Debug.LogError("Could not find balloons.json in Resources/Data/");
        }
    }

    public void BalloonReachedEnd(GameObject balloon)
    {
        health -= balloon.GetComponent<Balloon>().data.damage; // reduce player health when a balloon reaches the end of the path
        Destroy(balloon); // destroy the balloon object
    }

    public GameObject SpawnBalloon(BalloonData balloonData)
    {
        // spawn a new balloon at the start of the path
        GameObject newBalloon = Instantiate(balloonPrefab, waypoints[0], Quaternion.identity);
        newBalloon.GetComponent<Balloon>().InitializeFromData(balloonData); // set the balloon type
        numberOfBalloonsSpawned++; // increment the number of balloons in the queue
        return newBalloon; // return the new balloon object
    }
    public GameObject SpawnBalloon(Transform inputTransform, string name)
    {
        // spawn a new balloon at the start of the path
        GameObject newBalloon = Instantiate(balloonPrefab, inputTransform.position, inputTransform.rotation);
        newBalloon.GetComponent<Balloon>().InitializeFromData(balloonDatas[name]); // set the balloon type
        numberOfBalloonsSpawned++; // increment the number of balloons in the queue
        return newBalloon; // return the new balloon object
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
        if(isTowerBeingPlaced == false) // check if the player is in tower placement mode
        {
            cursorTower.GetComponent<SpriteRenderer>().enabled = false; // stop showing the cursor tower icon
            return; // exit if the player is not placing a tower
        }
        cursorTower.GetComponent<SpriteRenderer>().enabled = true; // show the cursor tower icon
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get the mouse position in world coordinates
        mousePos.z = 0; // set the z coordinate to 0 to keep the tower on the same plane as the path
        cursorTower.transform.position = mousePos; // move the cursor tower icon to the mouse position
        bool isValidTowerPlacement = IsValidTowerPlacement(mousePos); // check if the tower can be placed at the mouse position
        // Check if the mouse is over any UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            isValidTowerPlacement = false; // disable tower placement if the mouse is over any UI element
        }
        if(isValidTowerPlacement)
        {
            cursorTower.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.5f); // set the cursor tower icon to be green if the tower can be placed
        }
        else
        {
            cursorTower.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1f); // set the cursor tower icon to be red if the tower cannot be placed
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

    public void DamagePlayer()
    {
        if(health<0f) // check if the player is already dead
        {
            Debug.Log("game over"); // log game over message
        }
        health -= 1f;
    }

    public void AddMoney(int amount)
    {
        money += amount; // add the specified amount of money to the player's total
    }
    
    public void RemoveMoney(int amount)
    {
        money -= amount; // remove the specified amount of money from the player's total
    }

    public void OnClickBuyButton()
    {
        if(isTowerBeingPlaced) // check if the player is in tower placement mode
        {
            isTowerBeingPlaced = false; // exit tower placement mode
            AddMoney(50); // refund the money spent on the tower
            return;
        }
        else if (money >= 100) // check if the player has enough money to buy a tower
        {
            isTowerBeingPlaced = true; // enter tower placement mode
            RemoveMoney(50); // deduct money for placing the tower
        }
    }
}
