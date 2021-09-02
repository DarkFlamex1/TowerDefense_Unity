using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ObjectPool : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnTimer = 1f;
    [SerializeField] int poolSize = 5;

    GameObject[] pool;

    void Awake() {
        PopulatePool();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool(){
        //Init the array
        pool = new GameObject[poolSize];

        for(int i = 0; i < poolSize; i++){
            pool[i] = Instantiate(enemyPrefab, transform);
            //disable game object
            pool[i].SetActive(false);
        }

    }

    void EnableObjectInPool(){
        for(int i = 0; i < pool.Length; i++){
            if(pool[i].active == false){
                pool[i].SetActive(true);
                return;
            }
        }
    }

    IEnumerator SpawnEnemy(){
        while(true){
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
