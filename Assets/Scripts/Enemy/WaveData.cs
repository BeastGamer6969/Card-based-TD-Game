using System;
using UnityEngine;

[System.Serializable]
public class EnemyWaveData
{
    public float Stagger;
    public int Enemycount;
    public float Rate;
    public GameObject Enemy;
    public Transform SpwanPoint;

}
[System.Serializable]
public class WaveData
{
  public int Wave;
  public float Wavelenght;
  public EnemyWaveData[] EnemyThisWaveInfo;
}

