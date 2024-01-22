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

    [Header("References")]
    private BallSpawner spawner;
    private PlayerController player;
    public GameObject playerPrefab;

    [Header("ScreenShake")]
    // Transform of the GameObject you want to shake
    public new Transform camera;
    // Desired duration of the shake effect
    public float shakeDuration = 0.0f;
    // A measure of magnitude for the shake. Tweak based on your preference
    public float shakeMagnitude = 0.7f;
    // A measure of how quickly the shake effect should evaporate
    public float dampingSpeed = 1.0f;

    [Header("Points/Upgrades")]
    public int points = 0;
    private bool upgrades = true;
    private const int initialMoveSpeed = 5;
    private const int initialLives = 3;
    public TMP_Text pointsText;
    public TMP_Text speedText;
    public TMP_Text healthText;
    public TMP_Text toggleText;
    private int speedUpd = 0;
    private int healthUpd = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<BallSpawner>();
        player = FindObjectOfType<PlayerController>();
        if (pointsText != null)
        {
            GetAllSaveData();
            UpdateAllUpgradesText();
        }
    }

    public void StartGame()
    {
        //Apply all upgrades if any
        if (upgrades)
        {
            playerPrefab.GetComponent<PlayerController>().moveSpeed = initialMoveSpeed + speedUpd;
            playerPrefab.GetComponent<PlayerController>().lives = initialLives + healthUpd;
        }
        else
        {
            playerPrefab.GetComponent<PlayerController>().moveSpeed = initialMoveSpeed;
            playerPrefab.GetComponent<PlayerController>().lives = initialLives;
        }

        LevelLoader.Instance.LoadScene("Main");
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
                points++;
                PlayerPrefs.SetInt("points", points);
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
                print("60 sec");
            }
        }
    }

    public void TriggerShake()
    {
        shakeDuration = 0.4f;
    }

    private void Update()
    {
        if (camera != null)
        {
            if (shakeDuration > 0)
            {
                Vector2 test = Random.insideUnitSphere * shakeMagnitude;
                camera.localPosition = new Vector3(test.x, test.y, -10);

                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                shakeDuration = 0f;
                camera.localPosition = new Vector3(0, 0, -10);
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        GetAllSaveData();
        if (level == 0) UpdateAllUpgradesText();
        if (LevelLoader.curScene.Equals("Main"))
        {
            Instantiate(playerPrefab);
            lives.text = $"Lives: {FindObjectOfType<PlayerController>().lives}";
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpgradeSpeed()
    {
        if (points >= 3)
        {
            points -= 3;
            UpdatePointsText();
            speedUpd++;
            speedText.text = $"Current Buff: +{speedUpd}";
            PlayerPrefs.SetInt("speedUpd", speedUpd);
            PlayerPrefs.SetInt("points", points);
        }
        else StartCoroutine(AnnouncePoorFinancialStatus());
    }

    public void UpgradeHeartiness()
    {
        if (points >= 5)
        {
            points -= 5;
            UpdatePointsText();
            healthUpd++;
            healthText.text = $"Current Buff: +{healthUpd}";
            PlayerPrefs.SetInt("healthUpd", healthUpd);
            PlayerPrefs.SetInt("points", points);
        }
        else StartCoroutine(AnnouncePoorFinancialStatus());
    }

    public void ToggleUpgrades()
    {
        upgrades = !upgrades;
        toggleText.text = $"Currently: {upgrades}";
        PlayerPrefs.SetInt("toggle", System.Convert.ToInt32(upgrades));
    }

    public void ResetUpgrades()
    {
        player.moveSpeed = initialMoveSpeed;
        player.lives = initialLives;
        PlayerPrefs.DeleteAll();
    }

    private void UpdatePointsText()
    {
        pointsText.text = $"Points: {points}";
    }

    private void UpdateAllUpgradesText()
    {
        UpdatePointsText();
        speedText.text = $"Current Buff: +{speedUpd}";
        healthText.text = $"Current Buff: +{healthUpd}";
        toggleText.text = $"Currently: {upgrades}";
    }

    private void GetAllSaveData()
    {
        points = PlayerPrefs.GetInt("points");
        speedUpd = PlayerPrefs.GetInt("speedUpd");
        healthUpd = PlayerPrefs.GetInt("healthUpd");
        upgrades = System.Convert.ToBoolean(PlayerPrefs.GetInt("toggle"));
    }

    private IEnumerator AnnouncePoorFinancialStatus()
    {
        pointsText.text = "Not Enough Points!";
        pointsText.color = Color.red;
        pointsText.fontSize = 50;

        yield return new WaitForSeconds(0.3f);
        pointsText.enabled = false;
        yield return new WaitForSeconds(0.2f);
        pointsText.enabled = true;
        yield return new WaitForSeconds(0.3f);

        UpdatePointsText();
        pointsText.color = Color.white;
        pointsText.fontSize = 32;
    }
}
