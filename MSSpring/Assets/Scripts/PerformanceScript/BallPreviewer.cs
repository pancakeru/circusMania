using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BallPreview : MonoBehaviour
{
    private Transform throwerTransform;
    private Transform ballTransform;
    private float arcHeight = 3f;

    private void Awake()
    {
        ballTransform = transform.parent;
        GameObject thrower = GameObject.FindWithTag("BananaThrower");
        if (thrower != null)
        {
            throwerTransform = thrower.transform;
        }
    }

    private void Update()
    {
        if (ballTransform == null || throwerTransform == null)
            return;

        Vector3 start = throwerTransform.position;
        Vector3 end = ballTransform.position;
        Vector3 mid = (start + end) * 0.5f;
        mid.y += arcHeight;

        transform.position = mid;
    }
}
