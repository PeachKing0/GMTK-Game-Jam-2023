using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int lives = 3;

    [Header("References")]
    private Rigidbody2D rb;

    [Header("Movement")]
    [Range(0, 25)] public float moveSpeed;
    [Range(0, 25)] public float acceleration;
    [Range(0, 50)] public float deceleration;
    [Range(0, 5)] public float velPower;
    [field: SerializeField] public bool canMove { get; private set; } = true;
    private float moveInput_X;
    private float moveInput_Y;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        #region Inputs
        if (canMove)
        {
            moveInput_X = Input.GetAxisRaw("Horizontal");
            moveInput_Y = Input.GetAxisRaw("Vertical");
        }
        #endregion
    }

    private void FixedUpdate()
    {
        #region Movement
        if (canMove)
        {
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
        }
        #endregion
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (lives - 1 >= 0) lives--;
        }
    }

    public void SetCanMove(bool chingie) { canMove = chingie; }
}