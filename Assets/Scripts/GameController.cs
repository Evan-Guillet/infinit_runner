using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField]
    private GameObject[] GroundsPrefabs;
    [SerializeField]
    private GameObject[] GroundsOnStage;

    [SerializeField]
    public int Health;

    private GameObject Ship;

    private float GroundSize;

    private int NumberOfGrounds = 4;

    private void Awake() {
        if(Instance != null){
            Destroy(gameObject);
        }
        Instance = this;

        Health = 100;
    }

    void Start(){

        Ship = GameObject.Find("Ship");

        GroundsOnStage = new GameObject[NumberOfGrounds];
        
        for(int i = 0; i < NumberOfGrounds; i++){
            int n = Random.Range(0, GroundsPrefabs.Length);
            GroundsOnStage[i] = Instantiate(GroundsPrefabs[n]);
        }

        GroundSize = GroundsOnStage[0].GetComponentInChildren<Transform>().Find("Road").localScale.z;

        float pos = Ship.transform.position.z + GroundSize/2 - 1.5f;
        foreach(var ground in GroundsOnStage){
            ground.transform.position = new Vector3(0, 0.2f, pos);
            pos += GroundSize;
        }
    }

    void Update(){
        for(int i = GroundsOnStage.Length - 1; i >= 0; i--){
            GameObject ground = GroundsOnStage[i];

            if(ground.transform.position.z + GroundSize/2 < Ship.transform.position.z - 6f){
                float z = ground.transform.position.z;
                Destroy(ground);
                int n = Random.Range(0, GroundsPrefabs.Length);
                ground = Instantiate(GroundsPrefabs[n]);
                ground.transform.position = new Vector3(0, 0.2f, z + GroundSize * NumberOfGrounds);
                GroundsOnStage[i] = ground;
            }
        }
    }

    private void OnDestroy(){
        Instance = null;
    }
}
