using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    private new Collider2D collider;
    private PlayerController player;
    private AudioSource audioSource;
    public BallType type;
    public int bounces = 0;
    public LayerMask onion;

    [Header("Movement")]
    [Range(25, 50)] public float moveSpeed;
    [Range(0, 25)] public float acceleration;
    [Range(0, 25)] public float deceleration;
    [Range(0, 5)] public float velPower;
    public float targetVel = 25;
    private float moveInput_X;
    private float moveInput_Y;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
        player = FindObjectOfType<PlayerController>();
        audioSource = FindObjectOfType<BallSpawner>().GetComponent<AudioSource>();
        StartMovement();
    }

    private void StartMovement()
    {
        #region Inputs
        Vector2 dir = Vector2.zero;

        if (type == BallType.Flimsy || type == BallType.Bouncy || type == BallType.Booze)
        {
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            Vector3 randPos = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
            dir = randPos - transform.position;
        }
        else if (type == BallType.Mark_Rober || type == BallType.Bouncy_Rober)
            dir = player.transform.position - transform.position;

        dir.Normalize();
        moveInput_X = dir.x;
        moveInput_Y = dir.y;
        Moverton();
        #endregion
    }

    private void Moverton()
    {
        #region Movement
        //calculate the direction we want to move in and our desired velocity
        float targetSpeed_X = moveInput_X * moveSpeed;
        float targetSpeed_Y = moveInput_Y * moveSpeed;
        //calculate difference between current velocity and desired velocity
        float speedDif_X = targetSpeed_X - rb.velocity.x;
        float speedDif_Y = targetSpeed_Y - rb.velocity.y;
        //change acceleration rate depending on situation
        float accelRate_X = (Mathf.Abs(speedDif_X) > 0.01f) ? acceleration : deceleration;
        float accelRate_Y = (Mathf.Abs(speedDif_Y) > 0.01f) ? acceleration : deceleration;
        //applies acceleration to speed difference, then raises to a set power so acceleration increases with higher speeds
        //finally multiplies by sign to reapply direction
        float movement_X = Mathf.Pow(Mathf.Abs(speedDif_X) * accelRate_X, velPower) * Mathf.Sign(speedDif_X);
        float movement_Y = Mathf.Pow(Mathf.Abs(speedDif_Y) * accelRate_Y, velPower) * Mathf.Sign(speedDif_Y);

        //applies force to rigidbody
        rb.AddForce(movement_X * Vector2.right);
        rb.AddForce(movement_Y * Vector2.up);
        #endregion
    }

    private void FixedUpdate()
    {
        if (rb.velocity == Vector2.zero) StartMovement();

        if (rb.velocity.magnitude < 4.5f)
        {
            Vector2 test = Vector2.zero;
            rb.velocity = test;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.lives--;
            if (type == BallType.Booze) player.lives--;
            GameManager.instance.lives.text = $"Lives: {player.lives}";
            if (type == BallType.Booze) FindObjectOfType<BallSpawner>().doSpawn = true;
            Destroy(gameObject);
            if (player.lives <= 0) LevelLoader.Instance.LoadScene("GameOver");
        }
        else
        {
            if (bounces > 0)
            {
                StartMovement();
                bounces--;
                if (type == BallType.Booze)
                {
                    audioSource.PlayOneShot(FindObjectOfType<BallSpawner>().boozeHitWall);
                    moveSpeed += 3.5f;
                    GameManager.instance.TriggerShake();
                }
            }
            else
            {
                if (type == BallType.Booze) FindObjectOfType<BallSpawner>().doSpawn = true;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wololo"))
        {
            collider.isTrigger = false;
            if (type == BallType.Booze) StartCoroutine(WaitToEnlargenThyBooze());
        }
    }

    IEnumerator WaitToEnlargenThyBooze()
    {
        yield return new WaitForSeconds(0.425f);
        transform.localScale += new Vector3(5, 5, 0);
    }

    public enum BallType
    {
        Flimsy,
        Bouncy,
        Mark_Rober,
        Bouncy_Rober,
        Booze
    }
}