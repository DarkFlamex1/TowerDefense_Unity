using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid {get{return grid;}}

    void Awake(){
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinates){
        if(grid.ContainsKey(coordinates)){
            return grid[coordinates];
        }
        return null;
    }

    //Block Node with coordinates
    public void BlockNode(Vector2Int coordinates){
        if(grid.ContainsKey(coordinates)){
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNode(){
        //Reset isexplored, connected, reached
        foreach(KeyValuePair<Vector2Int, Node> entry in grid){
            entry.Value.parentNode = null;
            entry.Value.isPath = false;
            entry.Value.isExplored = false;
        }

    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position){
        Vector2Int coordinates = new Vector2Int();
        //Get the x & y coordinate of each parent tile
        coordinates.x = Mathf.RoundToInt(position.x/10);
        coordinates.y = Mathf.RoundToInt(position.z/10);

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates){
        Vector3 position = new Vector3();
        //Get the x & y coordinate of each parent tile
        position.x = coordinates.x * 10;
        position.z = coordinates.y * 10;

        return position;
    }

    void CreateGrid(){
        //Create the nodes and place into the dictionary
        for(int x = 0; x <= gridSize.x; x++){
            for(int y = 0; y <= gridSize.y; y++){
                //Temp position
                Vector2Int tempPos = new Vector2Int(x,y);
                //Add the node to the dictionary
                grid.Add(tempPos, new Node(tempPos, true));
                Debug.Log(grid[tempPos].coordinates);
            }
        }
    }
}
