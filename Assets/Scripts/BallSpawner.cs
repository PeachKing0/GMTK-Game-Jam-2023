using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float repeatTime = 3.0f;
    private new Camera camera;

    private void Start()
    {
        //Reset back to default vals
        repeatTime = 3;
        for (int i = 0; i < prefabs.Length; i++) prefabs[i].GetComponent<BallController>().moveSpeed = 25;

        camera = FindObjectOfType<Camera>();
        Invoke("SpawnBall", 1.0f);
    }

    private void SpawnBall()
    {
        Vector2 spawnPos = Vector2.zero;
        float height = camera.orthographicSize + 0.35f;
        float width = camera.orthographicSize * camera.aspect + 0.35f;
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

        Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPos, Quaternion.identity);
        StartCoroutine(WaitToSpawn());
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(repeatTime);
        SpawnBall();
    }
}
