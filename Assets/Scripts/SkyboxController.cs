using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Camera mainCamera;
    float x, y, z;

    // Start is called before the first frame update
    void Start()
    {
        z = transform.position.z;
    }

    // Update is called once per frame
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
