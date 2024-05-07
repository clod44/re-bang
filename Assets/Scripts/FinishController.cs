using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    public System.Action onFinish;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onFinish?.Invoke();
            GameManager.instance.PlaySound("level_complete");
            gameObject.GetComponent<Collider2D>().enabled = false;

        }
    }

}
