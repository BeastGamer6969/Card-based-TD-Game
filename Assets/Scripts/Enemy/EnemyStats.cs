using UnityEngine;
using System.Collections;

public class EnemyStat : MonoBehaviour
{
    [Header("References")]
    public Renderer rend;
    [Header("Movement Stats")]
    public int MoveSpeed;
    public bool CanMove;

    [Header("Health Stats")]
    public int Health;
    public Color FlashColor;
    public float FlashDuration;
    private Color OrignalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        OrignalColor = rend.material.color;
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        StartCoroutine(flash(OrignalColor));
    }

    IEnumerator flash(Color color)
    {
        rend.material.color = FlashColor;
        yield return new WaitForSeconds(FlashDuration);
        rend.material.color = color;
    }
}
