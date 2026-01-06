using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMovement : MonoBehaviour
{
    EnemyStat EM;
    int NextAnchors = 1;
    public float Gamespeed;
    private PlayerStats Stats;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
        {
            Stats = Camera.main.GetComponentInChildren<PlayerStats>();
            Stats.EnemyCount++;

            EM = GetComponent<EnemyStat>();
            if(AnchorList.Anchors[0].transform.position != transform.position)
            {
                transform.position = AnchorList.Anchors[0].position;
            }
        }
        // Update is called once per frame
        void Update()
        {
            Time.timeScale = Gamespeed;
            if (AnchorList.Anchors == null || NextAnchors >= AnchorList.Anchors.Length || EM.Health <= 0) EndProtocol();
            else
            {
                float Distance = Vector3.Distance(AnchorList.Anchors[NextAnchors].position, transform.position);

                if(Distance <= 0.01f)
                {
                    NextAnchors++;
                    transform.position = AnchorList.Anchors[NextAnchors-1].position;
                }
                
                else if(EM.CanMove == true & NextAnchors < AnchorList.Anchors.Length & Distance > 0.01f)
                {
                    transform.position = Vector3.MoveTowards
                    (
                        transform.position, 
                        AnchorList.Anchors[NextAnchors].position, 
                        EM.MoveSpeed*Time.deltaTime
                    );
                }
            }
        }

        void EndProtocol()
        {
            Stats.EnemyCount--;
            Destroy(gameObject);
            return;
        }
    }
