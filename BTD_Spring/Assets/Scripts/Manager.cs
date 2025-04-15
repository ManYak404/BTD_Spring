using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager ManagerInstance; // Singleton reference to this manager object available to all other scripts
    public Transform[] waypoints;   // path waypoints for balloons to follow

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
