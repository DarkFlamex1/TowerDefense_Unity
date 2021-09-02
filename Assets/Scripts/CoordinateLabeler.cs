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


    // The label of the tile
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();

    Waypoint waypoint;

    void Awake(){
        label = GetComponent<TextMeshPro>();
        waypoint = GetComponentInParent<Waypoint>();
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
        //Set the color based of of whether the coordinate is a placeable location or not
        if(waypoint.IsPlaceable){
            label.color = defaultColor;
        }
        else{
            label.color = blockedColor;
        }
    }
}
