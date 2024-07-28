
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathfindingTester : MonoBehaviour


{
    public float speed;
    private int count = 0;
    private bool reverse = false;
    private int stop = 0;
    int current;
    private int pickupcount;

    public Text PackageReceived;
    public Text Speed;

    public int Items;


    // The A* manager.
    private AStarManager AStarManager = new AStarManager();
    // Array of possible waypoints.
    List<GameObject> Waypoints = new List<GameObject>();
    // Array of waypoint map connections. Represents a path.
    List<Connection> ConnectionArray = new List<Connection>();
    // The start and end target point.
    public GameObject start;
    public GameObject end;
    // Debug line offset. 
    Vector3 OffSet = new Vector3(0, 0.3f, 0);

    // Start is called before the first frame update
    void Start()
    {
        // set count value to 0
        pickupcount = 0;
        data();
        

        


        if (start == null || end == null)
        {
            Debug.Log("No start or end waypoints.");
            return;
        }
        // Find all the waypoints in the level.
        GameObject[] GameObjectsWithWaypointTag;
        GameObjectsWithWaypointTag = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in GameObjectsWithWaypointTag)
        {
            WaypointCON tmpWaypointCon = waypoint.GetComponent<WaypointCON>();
            if (tmpWaypointCon)
            {
                Waypoints.Add(waypoint);
            }
        }
        // Go through the waypoints and create connections.
        foreach (GameObject waypoint in Waypoints)
        {
            WaypointCON tmpWaypointCon = waypoint.GetComponent<WaypointCON>();
            // Loop through a waypoints connections.
            foreach (GameObject WaypointConNode in tmpWaypointCon.Connections)
            {
                Connection aConnection = new Connection();
                aConnection.SetFromNode(waypoint);
                aConnection.SetToNode(WaypointConNode);
                AStarManager.AddConnection(aConnection);
            }
        }
        // Run A Star...
        ConnectionArray = AStarManager.PathfindAStar(start, end);
    }
    // Draws debug objects in the editor and during editor play (if option set).
    void OnDrawGizmos()
    {
        // Draw path.
        foreach (Connection aConnection in ConnectionArray)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine((aConnection.GetFromNode().transform.position + OffSet),
            (aConnection.GetToNode().transform.position + OffSet));
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // if block: to prevent index out of bound
        if (count < ConnectionArray.Count)
        {
            if (!reverse)
            {
                // this will run while going from source to destination
                // if block: to detect if cube reached its destination
                if (transform.position != ConnectionArray[count].GetToNode().transform.position)
                {
                    Speed.text = "Speed:" + speed.ToString();
                    transform.LookAt(ConnectionArray[count].GetToNode().transform);
                    //move forward to the position
                    transform.position = Vector3.MoveTowards(transform.position, ConnectionArray[count].GetToNode().transform.position, Time.deltaTime * speed);
                }
                else
                {
                    // if cube us reached to its destination then increment count by 1
                    count++;
                }
            }
            else
            {
                // this will run while returning from destination to source
                // if block: to detect if cube reached its destination
                if (transform.position != ConnectionArray[count].GetFromNode().transform.position)
                {
                    if (transform.position == ConnectionArray[count].GetFromNode().transform.position)
                    {
                        Speed.text = "Speed:" + speed.ToString();
                        var nodlook = ConnectionArray[current].GetToNode().transform.position - transform.position;
                        nodlook.y = 0;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nodlook), 1);
                        GetComponent<Rigidbody>().MovePosition(transform.position);
                    }

                    if (Items >= 9)
                    {

                        float s = (float)9 / 10;
                        float temp_speed =  speed - s * speed;
                        Speed.text = "Speed:" + temp_speed.ToString();
                        transform.LookAt(ConnectionArray[count].GetFromNode().transform);
                        //move forward to the position
                        transform.position = Vector3.MoveTowards(transform.position, ConnectionArray[count].GetFromNode().transform.position, Time.deltaTime * temp_speed);
                    }

                    else if(Items >= 1)
                    {
                        float s = (float)Items / 10;
                        float temp_speed = speed - s * speed;
                        Speed.text = "Speed:" + temp_speed.ToString();
                        transform.LookAt(ConnectionArray[count].GetFromNode().transform);
                        //move forward to the position
                        transform.position = Vector3.MoveTowards(transform.position, ConnectionArray[count].GetFromNode().transform.position, Time.deltaTime * temp_speed);
                    }
                    else
                    {
                        Speed.text = "Speed:" + speed.ToString();
                        transform.LookAt(ConnectionArray[count].GetFromNode().transform);
                        //move forward to the position
                        transform.position = Vector3.MoveTowards(transform.position, ConnectionArray[count].GetFromNode().transform.position, Time.deltaTime * speed);
                    }

                    
                }
                else
                {
                    // if cube us reached to its destination then increment count by 1
                    count++;
                }
            }
        }
        //If block: for Changing direction
        if (count == ConnectionArray.Count)
        {
            //reset count
            if (stop < 1)
            {
                stop += 1;
                count = 0;
            }
            //reverse both variable and the list
            reverse = !reverse;
            ConnectionArray.Reverse();
        }




    }

    private void OnTriggerEnter(Collider other)
    {



        if (other.gameObject.CompareTag("Food"))
        {
            other.gameObject.SetActive(false);
            pickupcount = pickupcount + 1;
            data();
            
        }
       
    }



    // text for speed and item pickups

    void data()
    {
        PackageReceived.text = "Package received:" + pickupcount.ToString();
    }

    
}