using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{   
    public static UI_Manager uiManager;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] TextMeshProUGUI goldText;

    //Create a singlton
    void Awake(){
        if(uiManager != null){
            GameObject.Destroy(uiManager);
        }
        else{
            uiManager = this;
        }
        DontDestroyOnLoad(this);
            
    }

    //Set max health on UI
    public void SetMaxHealth(int health){
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    //Takes in a value to set the ui manager
    public void SetHealth(int health){
        healthSlider.value = health;
        healthText.text = health.ToString();
    }
    //Set gold on change - research how to change at a specific time
    public void SetGold(int gold){
        goldText.text = "Gold: " + gold.ToString();
    }
}
