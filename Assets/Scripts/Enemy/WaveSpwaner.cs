using System.Collections;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class WaveSpwaner : MonoBehaviour
{
    public WaveData[] waveData;
    PlayerStats Stats;
    public int WaveIndex = 0;
    public int MaxEnemycount = 0;

    void Awake()
    {
       Stats = Camera.main.GetComponentInChildren<PlayerStats>(); 
    }

    void Update()
    {   
        if (Stats.CanStartNextWave && Stats.CountdownNextWave <= 0)
        {
            StartCoroutine(SpawnWave());
            Stats.CanStartNextWave = false;
        } 
        else if(!Stats.CanStartNextWave && (Stats.EnemyCount <= 0 || Stats.Wavetimer <= 0))
        {
            WaveIndex++;

            if(WaveIndex >= waveData.Length)
            {
                enabled = false;
                return;
            }
            Stats.CanStartNextWave = true;
            Stats.CountdownNextWave = 3;
            Stats.Wavetimer = 0;
        }

        if(Stats.Wavetimer > 0 && Stats.CountdownNextWave <= 0 && !Stats.CanStartNextWave) 
        Stats.Wavetimer -= Time.deltaTime;

        else if(Stats.Wavetimer <= 0 && Stats.CountdownNextWave > 0 && Stats.CanStartNextWave) 
        Stats.CountdownNextWave -= Time.deltaTime;
    }
    IEnumerator SpawnWave()
    {
        MaxEnemycount = 0;
        Stats.Wavetimer = waveData[WaveIndex].Wavelenght;
        foreach( EnemyWaveData Group in waveData[WaveIndex].EnemyThisWaveInfo)
            {
                StartCoroutine(SpawnEnemygroup(Group));
            }
        
        yield return new WaitForSeconds(waveData[WaveIndex].Wavelenght);
    }
    IEnumerator SpawnEnemygroup (EnemyWaveData Group)
	{
        if(Group.Stagger > 0) yield return new WaitForSeconds(Group.Stagger);
        for(int CurrentEnemy = 0; CurrentEnemy < Group.Enemycount; CurrentEnemy++)
        {
            SpwanEnemy(Group.Enemy, Group.SpwanPoint);
            yield return new WaitForSeconds(1f/Group.Rate);  
        }
	}

    public void SpwanEnemy(GameObject EnemyPre, Transform spwanPoint)
    {
        Instantiate(EnemyPre, transform, true);
        MaxEnemycount++;
    }
}
