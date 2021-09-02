using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_TargetLocater : MonoBehaviour
{

    [SerializeField]
    Transform weapon;

    [SerializeField]
    Transform target;

    [SerializeField]
    ParticleSystem ps_system;

    [SerializeField]
    float range = 15f;

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
        
    }

    void FindClosestTarget(){
        E_Controller[] enemies = FindObjectsOfType<E_Controller>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(E_Controller enemy in enemies){
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if(targetDistance < maxDistance){
                maxDistance = targetDistance;
                closestTarget = enemy.transform;
            }
        }

        target = closestTarget;
    }

    void AimWeapon(){

        //Make sure the target exists before aiming!
        if(target != null){
            float targetDistance = Vector3.Distance(transform.position, target.position);

            weapon.LookAt(target);

            if(targetDistance < range){
                Attack(true);
            }
            else{
                Attack(false);
            }
        }
        else{
            Attack(false);
        }

        
    }

    //Attack with the particle system
    void Attack(bool isActive){
        var emissionModule = ps_system.emission;
        emissionModule.enabled = isActive;
    }
}
