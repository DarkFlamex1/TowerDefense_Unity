using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_BankController : MonoBehaviour
{
    [SerializeField] int startingBalance = 100;

    //Balance of the players account
    [SerializeField] int currentBalance;
    public int CurrentBalance {get {return currentBalance;}}

    private void Start() {
        currentBalance = startingBalance;
        UI_Manager.uiManager.SetGold(currentBalance);
    }


    public void Depoist(int amount){
        if(amount >= 0){
            currentBalance += amount;
        }

        //Update UI
        UI_Manager.uiManager.SetGold(currentBalance);
    }

    //Takes a positive number to withdraw from the current balance
    public void Withdraw(int amount){
        if(amount >= 0){
            currentBalance -= amount;
        }

        UI_Manager.uiManager.SetGold(currentBalance);
    }
}
