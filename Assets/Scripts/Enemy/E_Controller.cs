using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Controller : MonoBehaviour
{
    List<Node> path = new List<Node>(); 
    [SerializeField] [Range(0f,5f)]float speed = 1f;

    [SerializeField] int maxHitPoints = 5;

    [SerializeField] int goldReward = 25;
    [SerializeField] int healthPenalty = 10;

    B_BankController bankController;
    GM_Player player;
    
    GridManager gridManager;
    P_Pathfinder pathfinder;
    int currentHitPoints = 0;

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<P_Pathfinder>();
        //Assign the bank controller
        bankController = FindObjectOfType<B_BankController>();
        player = FindObjectOfType<GM_Player>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        ReturnToStart();
        FindPath(true);
        //Init the maximum health
        currentHitPoints = maxHitPoints;
    }

    void ReturnToStart(){
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void FindPath(bool reset){
        Vector2Int coordinates = new Vector2Int();

        if(reset){
            coordinates = pathfinder.StartCoordinates;
        }
        else{
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();

        path.Clear();
        path = pathfinder.GetNewPath(coordinates);

        StartCoroutine(FollowPath());
    }

    private void OnParticleCollision(GameObject other) {
        //Process the damage on hit from the tower
        ProcessHit(other.GetComponentInParent<T_Controller>().getDamage);
    }

    void ProcessHit(int dmg)
    {
        //Remove damage
        currentHitPoints -= dmg;

        //Destroy at 0
        if(currentHitPoints <= 0){
            DeathSequence();
        }
    }

    //Courtintine 
    IEnumerator FollowPath()
    {

        for(int i = 1; i < path.Count; i++){
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            //Follow the
            transform.LookAt(endPosition);

            while(travelPercent < 1f){
                travelPercent += Time.deltaTime*speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }


        SuccessfulTrip();
        
    }


    void SuccessfulTrip(){
        //Remove the player health here
        player.takeDamage(healthPenalty);
        //For object pool
        gameObject.SetActive(false);
    }
    void DeathSequence(){
        //Reward gold before destruction
        RewardGold();
        //For object pool
        gameObject.SetActive(false);
    }

    void RewardGold(){
        if(bankController == null){ return; }
        //Deposit the gold reward
        //Currently only called on death
        bankController.Depoist(goldReward);
    }
}
