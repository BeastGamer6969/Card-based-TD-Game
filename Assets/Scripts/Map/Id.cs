using UnityEngine;

public class Id : MonoBehaviour
{
    [SerializeField] private int id;
    public int TowerId
    {
        get{ return id;}
    }
}