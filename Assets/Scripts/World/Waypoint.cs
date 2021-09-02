using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] 
    bool isPlaceable = true;
    public bool IsPlaceable {get{return isPlaceable;}}

    [SerializeField]
    T_Controller towerPrefab;

    void OnMouseDown() {
        if(isPlaceable){
            //Instantiate the tower that is selected
            bool isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);

            //Disable placement on the tile
            isPlaceable = !isPlaced;
        }
    }
}
