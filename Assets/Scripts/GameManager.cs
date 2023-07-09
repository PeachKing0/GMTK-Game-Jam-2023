using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text timer;
    public TMP_Text lives;
    private float time = 0.0f;
    private float prev10Time = 0.0f;
    private float prev30Time = 0.0f;
    private float prev60Time = 0.0f;
    private BallSpawner spawner;
    private PlayerController player;

    [Header("ScreenShake")]
    // Transform of the GameObject you want to shake
    public new Transform camera;
    // Desired duration of the shake effect
    public float shakeDuration = 0.0f;
    // A measure of magnitude for the shake. Tweak based on your preference
    public float shakeMagnitude = 0.7f;
    // A measure of how quickly the shake effect should evaporate
    public float dampingSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<BallSpawner>();
        player = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (timer != null)
        {
            time += Time.fixedDeltaTime;
            DisplayTime(time);
            if (time - prev10Time >= 10)
            {
                if (spawner.prefabs[0].GetComponent<BallController>().moveSpeed + 1 <= 50)
                {
                    for (int i = 0; i < spawner.prefabs.Length; i++)
                    {
                        spawner.prefabs[i].GetComponent<BallController>().moveSpeed++;
                    }
                }
                prev10Time = time;
                print("10 sec");
            }
            if (time - prev30Time >= 30)
            {
                if (spawner.repeatTime - 0.25f >= 0.75f)
                    spawner.repeatTime -= 0.25f;
                prev30Time = time;
                print("30 sec");
            }
            if (time - prev60Time >= 60)
            {
                BallController[] balls = FindObjectsOfType<BallController>();
                for (int i = 0; i < balls.Length; i++)
                {
                    Destroy(balls[i].gameObject);
                }
                spawner.doSpawn = false;
                spawner.SpawnBooze();
                prev60Time = time;
            }
        }
    }

    public void TriggerShake()
    {
        shakeDuration = 2.0f;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = Vector3.zero;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
