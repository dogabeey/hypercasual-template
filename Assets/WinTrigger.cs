using DG.Tweening;
using Dogabeey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Entity entity))
        {
            if (collision.CompareTag("Player"))
            {
                enabled = false;
                entity.GetComponent<Collider2D>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Debug.Log("Level Completed");
                EventManager.TriggerEvent(Const.GameEvents.LEVEL_COMPLETED, new EventParam());
                Spiral(entity.transform, 1);
            }
        }
    }

    // Spiral in a transform around this transform.
    public void Spiral(Transform target, float duration)
    {
        target.SetParent(transform, true);
        transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        target.DOMove(transform.position, duration).SetEase(Ease.Linear);
        target.DOScale(Vector3.zero, duration).SetEase(Ease.Linear);
    }
}
