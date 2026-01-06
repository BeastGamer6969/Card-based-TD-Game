using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float Speed;
    [SerializeField] private float ZoomSpeed;
    [SerializeField] private float MinFOV;
    [SerializeField] private float MaxFOV;

    [SerializeField] private Vector3 Dir;
    [SerializeField] private float ZoomVector;

    void Start()
    {
        inputReader.MoveEvent += MoveDirection;
        inputReader.ZoomEvent += SetZoom;
    }

    void Update()
    {
        Move();
        Zoom();
    }

  private void Zoom()
  {
    if(ZoomVector == 0f) return;
    Camera.main.fieldOfView += ZoomVector * ZoomSpeed * Time.deltaTime * 10;
    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, MinFOV, MaxFOV);
  }

  public void MoveDirection(Vector3 dir)
    {
        Dir = dir;
    }

    private void Move()
    {
        if(Dir == Vector3.zero) return;
        transform.parent.transform.position += Dir * (Speed*Time.deltaTime);
    }


    
    private void SetZoom(float Zoomlevel)
    {
        ZoomVector = Zoomlevel;
    }
}
