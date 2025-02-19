using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Camera mainCamera;
    float x, y, z;

    void Start()
    {
        z = transform.position.z;
    }

    void Update()
    {
        x = mainCamera.transform.position.x;
        y = mainCamera.transform.position.y;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(x, y, z);
    }
}
