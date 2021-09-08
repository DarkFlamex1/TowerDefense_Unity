using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] 
    bool isPlaceable = true;
    public bool IsPlaceable {get{return isPlaceable;}}

    [SerializeField]
    T_Controller towerPrefab;

    [SerializeField]
    E_Controller enemyPrefab;

    GridManager gridManager;
    P_Pathfinder pathfinder;
    Vector2Int coordinates = new Vector2Int();

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();    
        pathfinder = FindObjectOfType<P_Pathfinder>();
    }

    void Start() {
        if(gridManager != null){
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            
            if(!isPlaceable){
                gridManager.BlockNode(coordinates);
            }
        }
    }
    /// <summary>
    /// Instantiate a prefab on the tile(selected prefab) when a mouse down is pressed.
    /// </summary>
    void OnMouseDown() {
        if(gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates)){
            //Instantiate the tower that is selected
            bool isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);

            //Disable placement on the tile
            isPlaceable = !isPlaced;

            if(isPlaced){
                gridManager.BlockNode(coordinates);
                pathfinder.NotifyReceivers();
            }
            
        }
    }
}
