using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates {get{return startCoordinates;}}
    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates {get{return destinationCoordinates;}}
    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;
    void Awake() {
        Debug.Log("Awake pathfinder");
        gridManager = FindObjectOfType<GridManager>();

        startNode = gridManager.Grid[startCoordinates];
        destinationNode = gridManager.Grid[destinationCoordinates];

        startNode.isWalkable = true;
        destinationNode.isWalkable = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath(){
        gridManager.ResetNode();
        BreadthFirstSearch(startCoordinates);
        return BuildPath();
    }
    
    //Takes in a vector2 int
    public List<Node> GetNewPath(Vector2Int startCoord){
        gridManager.ResetNode();
        BreadthFirstSearch(startCoord);
        return BuildPath();
    }

    void ExploreNeighbors(){
        List<Node> neighbors = new List<Node>();

        foreach(Vector2Int direction in directions){
            //check if the neighbor's coordinates exist in the grid
            Vector2Int tempCoords = currentSearchNode.coordinates + direction;
            Node tempNode = gridManager.GetNode(tempCoords);
            //Check if exist in grid
            if(gridManager.GetNode(tempCoords) != null){
                //The node exists in grid
                neighbors.Add(tempNode);
            }
        }

        foreach(Node neighbor in neighbors){
            if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable){
                neighbor.parentNode = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }  
        }

    }

    void BreadthFirstSearch(Vector2Int startCoord){
        frontier.Clear();
        reached.Clear();


        bool isRunning = true;

        //Add to queue, and add that we have reached
        frontier.Enqueue(gridManager.Grid[startCoord]);
        reached.Add(startCoord, startNode);

        while(frontier.Count > 0 && isRunning){
            //Setup current search node
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            //explore the neighbors
            ExploreNeighbors();

            if(currentSearchNode.coordinates == destinationCoordinates){
                //Stop early as reached destination
                isRunning = false;
            }
        }

    }

    List<Node> BuildPath(){
        List<Node> path = new List<Node>();
        
        Node currentNode = destinationNode;
        
        path.Add(currentNode);
        currentNode.isPath = true;

        while(currentNode.parentNode != null){
            currentNode = currentNode.parentNode;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates){
        //Make sure the coordinates exist
        if(gridManager.Grid.ContainsKey(coordinates)){

            bool previousState = gridManager.Grid[coordinates].isWalkable;
            //Not a walkable node
            gridManager.Grid[coordinates].isWalkable = false;
            //Get a new path
            List<Node> newPath = GetNewPath();
            gridManager.Grid[coordinates].isWalkable = previousState;
            Debug.Log(newPath.Count + " COUNT");
            if(newPath.Count <= 1){
                Debug.Log("WILL BLOCK");
                //No larger than the first single node, so we need new path from old state
                GetNewPath();
                return true;
            }

            //We can place!
            return false;
        }

        //Grid Manager not FOUND!
        return false;
    }
    
    public void NotifyReceivers(){
        BroadcastMessage("FindPath", false, SendMessageOptions.DontRequireReceiver);
    }

}
