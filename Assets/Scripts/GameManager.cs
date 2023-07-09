using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text timer;
    private float time = 0.0f;
    private float prev10Time = 0.0f;
    private float prev30Time = 0.0f;
    private BallSpawner spawner;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<BallSpawner>();
        player = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
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
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
