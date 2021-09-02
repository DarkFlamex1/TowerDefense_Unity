using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Controller : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f,5f)]float speed = 1f;

    [SerializeField] int maxHitPoints = 5;

    [SerializeField] int goldReward = 25;
    [SerializeField] int healthPenalty = 10;

    B_BankController bankController;
    GM_Player player;
    
    int currentHitPoints = 0;

    void Start() {
        //Assign the bank controller
        bankController = FindObjectOfType<B_BankController>();
        player = FindObjectOfType<GM_Player>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        //Init the maximum health
        currentHitPoints = maxHitPoints;
        StartCoroutine(FollowPath());
    }

    void ReturnToStart(){
        transform.position = path[0].transform.position;
    }

    void FindPath(){
        path.Clear();

        //All objects tagged as a path
        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        foreach(Transform child in parent.transform){
            path.Add(child.GetComponent<Waypoint>());
        }

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
        foreach(Waypoint pt in path){
            Vector3 startPosition = transform.position;
            Vector3 endPosition = pt.transform.position;
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
