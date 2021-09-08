using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Controller : MonoBehaviour
{
    [SerializeField]
    int damage = 1;
    public int getDamage { get { return damage; } }

    [SerializeField]
    //Use this to set the speed of the shots/particle/so on
    int speed = 0;

    [SerializeField]
    float buildTimer = 1f;

    [SerializeField]
    int cost = 75;
    B_BankController bankController;


    void Start(){
        StartCoroutine(Build());
    }

    public bool CreateTower(T_Controller tower, Vector3 position){

        //Get the bank controller
        bankController = FindObjectOfType<B_BankController>();
        if(bankController == null){
            return false;
        }

        if(bankController.CurrentBalance >= cost){
            bankController.Withdraw(cost);
            Instantiate(tower.gameObject, position, Quaternion.identity);
            return true;
        }
        
        return false;
    }

    IEnumerator Build(){
        //Disable all the gameobjects
        foreach (Transform child in transform){
            child.gameObject.SetActive(false);
            foreach(Transform grandchild in child){
                grandchild.gameObject.SetActive(false);
            }
        }
        
        foreach (Transform child in transform){

            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildTimer);
            foreach(Transform grandchild in child){
                grandchild.gameObject.SetActive(false);
            }
        }
    }
}
