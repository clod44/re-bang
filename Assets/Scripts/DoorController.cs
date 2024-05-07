using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string label = "Door 1";
    public int code = -1;
    private bool _isOpen = false;
    public bool isOpen
    {
        get
        {
            return _isOpen;
        }
        set
        {
            _isOpen = value;
            if (doorCollider != null)
                doorCollider.enabled = !_isOpen;
            if (spriteRenderer)
                spriteRenderer.color = new Color(1, 1, 1, _isOpen ? 0.1f : 1f);

        }
    }


    private KeyController key;
    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;
    private ParticleGenerator particleGenerator;
    void Start()
    {
        code = Random.Range(0, 100000);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        particleGenerator = GetComponentInChildren<ParticleGenerator>();
        key = GetComponentInChildren<KeyController>();
        if (key != null)
        {
            key.label = label;
            key.code = code;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        KeyController keyController = collision.gameObject.GetComponent<KeyController>();
        if (keyController != null)
        {
            if (keyController.code == code && !isOpen)
            {
                particleGenerator.Emit("unlock", transform.position, Quaternion.identity);
                isOpen = true;
            }
        }
    }

}
