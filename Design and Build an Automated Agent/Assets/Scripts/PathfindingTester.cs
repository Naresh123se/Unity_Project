using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTester : MonoBehaviour
{
    // The A* manager. 
    private AStarManager AStarManager = new AStarManager();

    // List of possible waypoints. 
    private List<GameObject> Waypoints = new List<GameObject>();

    // List of waypoint map connections. Represents a path. 
    private List<Connection> ConnectionArray = new List<Connection>();

    // The start and end nodes. 
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject end;

    // Debug line offset. 
    Vector3 OffSet = new Vector3(0, 0.3f, 0);

    // Movement variables. 
    private float currentSpeed = 28;
    private int currentTarget = 0;
    private Vector3 currentTargetPos;
    private int moveDirection = 1;
    private bool agentMove = true;

    // Boolean variable to track if the agent has completed a full round trip.
    private bool hasCompletedRoundTrip = false;

    // Boolean variable to track if the parcel has been dropped.
    private bool hasDroppedParcel = false;

    // Reference to the parcel GameObject.
    public GameObject parcelPrefab;

    // Start is called before the first frame update 
    void Start()
    {
        if (start == null || end == null)
        {
            Debug.Log("No start or end waypoints.");
            return;
        }

        // Find all the waypoints in the level. 
        GameObject[] GameObjectsWithWaypointTag = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach (GameObject waypoint in GameObjectsWithWaypointTag)
        {
            Waypoints.Add(waypoint);
        }

        // Go through the waypoints and create connections. 
        foreach (GameObject waypoint in Waypoints)
        {
            VisGraphWaypointManager tmpWaypointMan = waypoint.GetComponent<VisGraphWaypointManager>();
            // Loop through a waypoint's connections. 
            foreach (VisGraphConnection aVisGraphConnection in tmpWaypointMan.Connections)
            {
                if (aVisGraphConnection.ToNode != null)
                {
                    Connection aConnection = new Connection();
                    aConnection.FromNode = waypoint;
                    aConnection.ToNode = aVisGraphConnection.ToNode;

                    AStarManager.AddConnection(aConnection);
                }
                else
                {
                    Debug.Log("Warning, " + waypoint.name + " has a missing to node for a connection!");
                }
            }
        }

        // Run A Star... 
        // ConnectionArray stores all the connections in the route to the goal / end node. 
        ConnectionArray = AStarManager.PathfindAStar(start, end);
        if (ConnectionArray.Count == 0)
        {
            Debug.Log("Warning, A* did not return a path between the start and end node.");
        }
    }

    // Draws debug objects in the editor and during editor play (if option set). 
    void OnDrawGizmos()
    {
        // Draw path. 
        foreach (Connection aConnection in ConnectionArray)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine((aConnection.FromNode.transform.position + OffSet),
                            (aConnection.ToNode.transform.position + OffSet));
        }
    }

    // Update is called once per frame 
    void Update()
    {
        if (agentMove && !hasCompletedRoundTrip)
        {
            // Determine the direction to the current target node in the array. 
            if (moveDirection > 0)
            {
                currentTargetPos = ConnectionArray[currentTarget].ToNode.transform.position;
            }
            else
            {
                currentTargetPos = ConnectionArray[currentTarget].FromNode.transform.position;
            }

            // Clear y to avoid up/down movement. Assumes a flat surface. 
            currentTargetPos.y = transform.position.y;

            Vector3 direction = currentTargetPos - transform.position;

            // Calculate the length of the relative position vector  
            float distance = direction.magnitude;

            // Face in the right direction. 
            direction.y = 0;
            if (direction.magnitude > 0)
            {
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = rotation;
            }

            // Calculate the normalized direction to the target from a game object. 
            Vector3 normDirection = direction / distance;

            // Move the game object. 
            transform.position = transform.position + normDirection * currentSpeed * Time.deltaTime;

            // Check if close to the current target. 
            if (distance < 1)
            {
                

                // Move to the next target in the list (if there is one).
                currentTarget += moveDirection;

                // Check if the car reached the end or start of the path.
                if (currentTarget >= ConnectionArray.Count || currentTarget < 0)
                {
                    // Reverse the movement direction.
                    moveDirection *= -1;

                    // Update the current target to the next node in the reversed direction.
                    currentTarget += moveDirection;

                    // Check if the agent has completed a full round trip.
                    if (currentTarget == 0 && moveDirection == 1)
                    {
                        hasCompletedRoundTrip = true;
                        // Optionally, you can add logic to stop the agent or perform other actions.
                        // For example, you might set agentMove to false to stop further movement.
                    }
                }
            }
        }
        else
        {
            // ... additional logic for when the agent is not moving ...
        }
    }

    // ... (Previous code remains unchanged)

    // Function to drop the parcel at a position offset from the endpoint.
    // Function to drop the parcel at a position offset from the endpoint.
   





}