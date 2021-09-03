using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake(){
        CreateGrid();
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