using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool IsCollisionTrap { get; set; } = false;
    public bool IsCollisionFinishPoint { get; set; }
    public Vector2 CheckPointPos { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
            IsCollisionTrap = true;
        if (collision.name.StartsWith("End (Pressed) (64x64)_0"))
        {
            IsCollisionFinishPoint = true;
            StartCoroutine(SetTimeScale());
        }
        if (collision.CompareTag("Checkpoint"))
            CheckPointPos = collision.transform.position;
    }

    private IEnumerator SetTimeScale()
    {
        yield return null;
        Time.timeScale = 1;
    }
}
