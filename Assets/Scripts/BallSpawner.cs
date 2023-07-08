using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;
    private float repeatTime = 3.0f;
    private new Camera camera;

    private void Start()
    {
        camera = FindObjectOfType<Camera>();
        Invoke("SpawnBall", 1.0f);
    }

    private void SpawnBall()
    {
        Vector2 spawnPos = Vector2.zero;
        float height = camera.orthographicSize + 0.25f;
        float width = camera.orthographicSize * camera.aspect + 0.25f;
        spawnPos.x = Random.Range(-0.5f, 0.5f);
        spawnPos.y = Random.Range(-0.5f, 0.5f);
        if (spawnPos.x > spawnPos.y)
        {
            spawnPos.x = (spawnPos.x >= 0) ? spawnPos.x + width : spawnPos.x - width;
            spawnPos.y += Random.Range(-height, height);
        }
        else if (spawnPos.x < spawnPos.y)
        {
            spawnPos.y = (spawnPos.y >= 0) ? spawnPos.y + height : spawnPos.y - height;
            spawnPos.x += Random.Range(-width, width);
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);
        StartCoroutine(WaitToSpawn());
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(repeatTime);
        SpawnBall();
    }
}
