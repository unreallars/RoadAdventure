using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float factor;

    private float spawnPosX;
    private Transform mainCamera;

    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        spawnPosX = transform.position.x;
    }

    private void Update()
    {
        Vector3 cameraPosition = mainCamera.position;
        transform.position = new Vector3(
            spawnPosX + cameraPosition.x / factor,
            cameraPosition.y / factor,
            0f
        );
    }
}
