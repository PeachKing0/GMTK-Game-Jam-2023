using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    private Transform player;
    public BallType type;
    public int bounces = 0;

    [Header("Movement")]
    [Range(0, 25)] public float moveSpeed;
    [Range(0, 25)] public float acceleration;
    [Range(0, 25)] public float deceleration;
    [Range(0, 5)] public float velPower;
    [field: SerializeField] public bool canMove { get; private set; } = true;
    private float moveInput_X;
    private float moveInput_Y;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartMovement();
    }

    private void StartMovement()
    {
        #region Inputs
        if (canMove)
        {
            if (type == BallType.Flimsy || type == BallType.Bouncy)
            {

            }
            else if (type == BallType.Mark_Rober || type == BallType.Bouncy_Rober)
            {
                Vector2 dir = transform.position - player.position;

                if (dir.x == 0) moveInput_X = 0;
                else moveInput_X = (dir.x > 0) ? moveInput_X = -1 : moveInput_X = 1;

                if (dir.y == 0) moveInput_Y = 0;
                else moveInput_Y = (dir.y > 0) ? moveInput_Y = -1 : moveInput_Y = 1;
            }
            Moverton();
        }
        #endregion
    }

    private void Moverton()
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

            canMove = false;
        }
        #endregion
    }

    public void SetCanMove(bool chingie) { canMove = chingie; }

    public enum BallType
    {
        Flimsy,
        Bouncy,
        Mark_Rober,
        Bouncy_Rober
    }
}
