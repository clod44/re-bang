using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public float mouseOffsetFactor = 0.1f;

    private Vector3 initialPosition;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector2 desiredPosition = new Vector2(target.position.x, target.position.y);
            Vector2 myPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 smoothedPosition = Vector3.Lerp(myPosition, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }

        //get mouse pos
        Vector3 mousePos = Input.mousePosition;
        //normalize from -1 to 1
        float clampedX = Mathf.Clamp(mousePos.x, 0, Screen.width) / Screen.width;
        float clampedY = Mathf.Clamp(mousePos.y, 0, Screen.height) / Screen.height;

        Vector3 mouseOffset = new Vector3(clampedX - 0.5f, clampedY - 0.5f, 0f) * mouseOffsetFactor;
        transform.position += mouseOffset;
    }

    public void Shake(float strength, float duration)
    {
        StartCoroutine(ShakeCoroutine(strength, duration));
    }

    private IEnumerator ShakeCoroutine(float strength, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transform.position = initialPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;
    }
}
