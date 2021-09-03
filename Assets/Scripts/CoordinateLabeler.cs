using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Always runnning(in edit and play mode)
[ExecuteAlways]

public class CoordinateLabeler : MonoBehaviour
{

    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f,0.5f,0f);


    // The label of the tile
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;

    void Awake(){
        label = GetComponent<TextMeshPro>();
        gridManager = FindObjectOfType<GridManager>();
        //Set to the coordinates @ play start
        DisplayCoordinates();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Only execute in edit mode
        if(!Application.isPlaying){
            //Display the coordinates
            DisplayCoordinates();
            UpdateObjectName();
        }

        ColorCoordinates();
        ToggleLabels();
    }

    void DisplayCoordinates(){
        //Get the x & y coordinate of each parent tile
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x)/10;
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z)/10;

        label.text = coordinates.x + "," + coordinates.y;
    }


    void ToggleLabels(){
        if(Input.GetKeyDown(KeyCode.C)){
            label.enabled = !label.IsActive();
        }
    }

    void UpdateObjectName(){
        transform.parent.name = coordinates.ToString();
    }

    void ColorCoordinates(){
        if(gridManager == null) {return;}

        Node node = gridManager.GetNode(coordinates);

        if(node == null){return;}        
        if(!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if(node.isPath)
        {
            label.color = pathColor;
        }
        else if(node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }

    }
}
