using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TurretLogic : MonoBehaviour
{
    [Header("Enemy Tracking")]
    public float TurretRange;
    public float Offset;
    public float TurnSpeed;
    public String EnemyTag;

    [Header("Damage")]
    public int Damgae;
    public float fireRate;
    public bool CanShoot = true;


    private SphereCollider RangeCollider;
    private List<GameObject> Enemies = new List<GameObject>();
    private Transform Body;

    void Start()
    {
        Body = transform.GetChild(0).transform;
        RangeCollider = GetComponent<SphereCollider>();
        RangeCollider.radius = TurretRange*10;
        RangeCollider.isTrigger = true;
        transform.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        Enemies.RemoveAll(Enemy => Enemy == null);
        float ShortestDistance = Mathf.Infinity;
        GameObject Traget = null;
        
        foreach(GameObject Enemy in Enemies)
        {
            float DistanceToEnemy = Vector3.Distance(Enemy.transform.position, transform.position);
            if (ShortestDistance > DistanceToEnemy)
            {
                ShortestDistance = DistanceToEnemy;
                Traget = Enemy.gameObject;
            }
        }
        if (Traget != null )
        {
            Vector3 dir = Traget.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            Quaternion fixedRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            Body.rotation = Quaternion.RotateTowards(
                Body.rotation,
                fixedRotation,
                TurnSpeed * Time.deltaTime * 100f);

            float Aim = math.dot(Body.transform.forward.normalized, dir.normalized);

            if (CanShoot && Aim > 0.9)
            {
                StartCoroutine(Shoot(Traget));
            }
        }
    }
    IEnumerator Shoot(GameObject Traget)
    {
        CanShoot = false;
        Fire(Traget);
        yield return new WaitForSeconds(fireRate);
        CanShoot = true;
    }

    void Fire(GameObject Traget)
    {
        if (Traget != null){
            EnemyStat EnemyHealth = Traget.GetComponent<EnemyStat>();
            EnemyHealth.Health -= Damgae;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(EnemyTag))
        {
            Enemies.Add(other.gameObject);
        }
    }

        void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(EnemyTag))
        {
            Enemies.Remove(other.gameObject);
        }
    }
}
