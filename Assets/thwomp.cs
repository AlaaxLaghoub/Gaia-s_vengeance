using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    [Header("Thwomp Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float checkDelay = 1f;
    [SerializeField] private LayerMask playerLayer;

    // Optional Animator for animations
    // [SerializeField] private Animator anim;

    private float checkTimer;
    private Vector3 moveDirection;
    private bool attacking;
    private Vector3[] directions = new Vector3[4];

    private void OnEnable()
    {
        StopMovement();
    }

    private void Update()
    {
        if (attacking)
        {
            transform.Translate(moveDirection * Time.deltaTime * speed);
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= checkDelay)
            {
                CheckForPlayer();
                checkTimer = 0f;
            }
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections();

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 dir = directions[i].normalized;
            Debug.DrawRay(transform.position, dir * range, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, range, playerLayer);

            if (hit.collider != null)
            {
                // anim?.SetTrigger("Attack"); // Optional animation trigger
                moveDirection = dir;
                attacking = true;
                break; // Attack in only one direction
            }
        }
    }

    private void CalculateDirections()
    {
        directions[0] = transform.right;   // Right
        directions[1] = -transform.right;  // Left
        directions[2] = transform.up;      // Up
        directions[3] = -transform.up;     // Down
    }

    private void StopMovement()
    {
        moveDirection = Vector3.zero;
        attacking = false;

        // anim?.SetTrigger("Idle"); // Optional idle state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StopMovement();

        // Optional: Handle interaction with player
        // if (collision.CompareTag("Player"))
        // {
        //     // Apply damage or trigger effect here
        // }
    }
}
