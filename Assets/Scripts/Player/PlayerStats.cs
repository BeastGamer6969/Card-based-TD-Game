using System.Threading;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Game Stats")]
    public int EnemyCount;
    public bool CanStartNextWave = true;
    public float CountdownNextWave = 3f;
    public float Wavetimer = 0f;
}
