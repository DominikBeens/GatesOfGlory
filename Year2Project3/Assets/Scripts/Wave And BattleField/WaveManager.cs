using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public static WaveManager instance;

    public List<Wave> Waves = new List<Wave>();
    int currentWave; 
    int currentStage;
    int currentSoldier; 
    public float wavePause;
    public float maxSoldierWait, minSoldierWait;
    public float maxStageWait, minStageWait;
    public List<Transform> spwanPoints = new List<Transform>();
    public float SpawnsetOff;

    public List<Enemy> enemiesInScene = new List<Enemy>();
    public List<Allie> alliesInScene = new List<Allie>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start(){
        StartCoroutine(StageTimer());
    }

    public void NextWave(){
        currentWave++;
        StartCoroutine(WaveTimer());
    }

    public void NextStage(){
        currentStage++;
        StartCoroutine(StageTimer());
    }


    public IEnumerator WaveTimer(){

        yield return new WaitForSeconds(wavePause);

        if (currentWave < Waves.Count){
            StartCoroutine(StageTimer());
        }
        else{
            print("Finish");
        }
    }

    public IEnumerator StageTimer(){

        yield return new WaitForSeconds(Random.Range(minStageWait, maxStageWait));

        if(currentStage < Waves[currentWave].atackStage.Count){
            StartCoroutine(SoldierTimer());
        }
        else{
            currentStage = 0;
            NextWave();
        }
    }

    public IEnumerator SoldierTimer(){

        yield return new WaitForSeconds(Random.Range(minSoldierWait, maxSoldierWait));

        if (currentSoldier < Waves[currentWave].atackStage[currentStage].soldiers.Count){
            int k = Random.Range(0,1);
            GameObject newEnemy = Instantiate(Waves[currentWave].atackStage[currentStage].soldiers[currentSoldier], new Vector3(spwanPoints[k].transform.position.x + Random.Range(-SpawnsetOff, SpawnsetOff), 0,Random.Range(-SpawnsetOff,SpawnsetOff)), spwanPoints[k].transform.rotation);
            currentSoldier++;
            enemiesInScene.Add(newEnemy.GetComponent<Enemy>());
            StartCoroutine(SoldierTimer());
        }
        else{
            currentSoldier = 0;
            NextStage();
        }
    }
}
