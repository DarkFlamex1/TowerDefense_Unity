using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_Player : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    
    int currentHealth = 0;
    int CurrentHealth {get{return currentHealth;}}

    UI_Manager uiController;
    // Start is called before the first frame update
    void Start()
    {
        //Set the health
        currentHealth = maxHealth;

        //uiController = FindObjectOfType<UI_Manager>();
        UI_Manager.uiManager.SetMaxHealth(currentHealth);
    }

    public void takeDamage(int dmg){
        currentHealth -= dmg;
        UI_Manager.uiManager.SetHealth(currentHealth);
        
        //Handle death of player
        if(currentHealth <= 0){
            Debug.Log("You died");
        }
    }
}
