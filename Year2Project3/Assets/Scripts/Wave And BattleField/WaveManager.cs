using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    public List<string> enemyTypes = new List<string>();
    public Wave thisWave;
    public float waveSize = 10;
    public float minWaveMultiplier, maxWaveMultiplier;
    public float HealthMultiplier;
    public float DamageMultiplier;
    public int maxStageSize;

    public static WaveManager instance;
    bool waveDone;
    public int currentWave;
    int currentStage;
    int currentSoldier;
    public float wavePause;
    public float maxSoldierWait, minSoldierWait;
    public float maxStageWait, minStageWait;
    public List<Transform> spwanPoints = new List<Transform>();
    public float SpawnsetOff;
    public List<Enemy> enemiesInScene = new List<Enemy>();
    public List<Allie> alliesInScene = new List<Allie>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        GeneradeWave();
        StartCoroutine(StageTimer());
    }

    void Update()
    {
        if (enemiesInScene.Count <= 0 && waveDone == true)
        {
            NextWave();
        }
    }

    public void NextWave()
    {
        HealthMultiplier *= HealthMultiplier;
        DamageMultiplier *= DamageMultiplier;
        GeneradeWave();

        currentWave++;
        waveDone = false;
        StartCoroutine(WaveTimer());
    }

    void GeneradeWave()
    {
        waveSize = waveSize * Random.Range(minWaveMultiplier, maxWaveMultiplier);

        thisWave = new Wave();
        for (int w = 0; w < Mathf.Round(waveSize); w++)
        {
            int stageSize = Random.Range(Mathf.Clamp(Mathf.RoundToInt(currentWave / 2f), 1, maxStageSize), Mathf.Clamp(Mathf.RoundToInt(currentWave / 0.5f), 1, maxStageSize));
            stageSize = Mathf.Clamp(stageSize - Random.Range(0, 3), 1, maxStageSize);
            Stage _newStage = new Stage();
            for (int s = 0; s < stageSize; s++)
            {
                _newStage.soldiers.Add(enemyTypes[Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp(currentWave / 10f, 0, enemyTypes.Count - 1) - Random.Range(0, enemyTypes.Count)), 0, enemyTypes.Count - 1)]);
            }
            thisWave.atackStage.Add(_newStage);
        }
    }

    void NextStage()
    {
        currentStage++;
        StartCoroutine(StageTimer());
    }


    IEnumerator WaveTimer()
    {

        yield return new WaitForSeconds(wavePause);

        StartCoroutine(StageTimer());
    }

    IEnumerator StageTimer()
    {

        yield return new WaitForSeconds(Random.Range(minStageWait, maxStageWait));

        if (currentStage < thisWave.atackStage.Count)
        {
            StartCoroutine(SoldierTimer());
        }
        else
        {
            waveDone = true;
            currentStage = 0;
        }
    }

    IEnumerator SoldierTimer()
    {

        yield return new WaitForSeconds(Random.Range(minSoldierWait, maxSoldierWait));

        if (currentSoldier < thisWave.atackStage[currentStage].soldiers.Count)
        {
            int k = Random.Range(0, 1);
            GameObject newEnemy = ObjectPooler.instance.GrabFromPool(thisWave.atackStage[currentStage].soldiers[currentSoldier], new Vector3(spwanPoints[k].transform.position.x + Random.Range(-SpawnsetOff, SpawnsetOff), 0, Random.Range(-SpawnsetOff, SpawnsetOff)), spwanPoints[k].transform.rotation); //Instantiate(thisWave.atackStage[currentStage].soldiers[currentSoldier], new Vector3(spwanPoints[k].transform.position.x + Random.Range(-SpawnsetOff, SpawnsetOff), 0, Random.Range(-SpawnsetOff, SpawnsetOff)), spwanPoints[k].transform.rotation);
            newEnemy.GetComponent<Enemy>().myStats.ChangeStats(HealthMultiplier, DamageMultiplier);
            newEnemy.GetComponent<NavMeshAgent>().speed = Random.Range(1.75f, 2.25f);
            currentSoldier++;
            enemiesInScene.Add(newEnemy.GetComponent<Enemy>());
            StartCoroutine(SoldierTimer());
        }
        else
        {
            currentSoldier = 0;
            NextStage();
        }
    }
}
