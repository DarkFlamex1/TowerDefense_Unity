using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;
    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
    }
    // Start is called before the first frame update
    void Start()
    {

        startNode = gridManager.Grid[startCoordinates];
        destinationNode = gridManager.Grid[destinationCoordinates];
        BreadthFirstSearch();
        BuildPath();
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

    void BreadthFirstSearch(){
        bool isRunning = true;

        //Add to queue, and add that we have reached
        frontier.Enqueue(startNode);
        reached.Add(startCoordinates, startNode);

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
        

        while(currentNode != startNode){
            path.Add(currentNode);
            currentNode.isPath = true;
            currentNode = currentNode.parentNode;
        }

        path.Reverse();

        return path;
    }

}
