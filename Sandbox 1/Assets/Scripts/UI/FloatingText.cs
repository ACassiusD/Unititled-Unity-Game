using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destoryTime = 3f;
    public Vector3 offset = new Vector3(0, 2, 0);
    public Vector3 randomizeIntensity = new Vector3(0, 0, 0);
    public float xrand = 20000f;
    public float yrand = 20000f;

    void Start()
    {
        randomizeIntensity = new Vector3(xrand, yrand, 0);
        Destroy(gameObject, destoryTime);
        transform.localPosition += offset;

        transform.localPosition += new Vector3(
            Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
            Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
            Random.Range(-randomizeIntensity.z, randomizeIntensity.z)
        );
    }

    void LateUpdate()
    {
        var cameraToLookAt = Camera.main;
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }
}
