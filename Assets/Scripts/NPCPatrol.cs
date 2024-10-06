using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 5f;
    private bool movingRight = true;
    private Vector3 startPosition;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

            if (transform.position.x >= startPosition.x + moveDistance)
            {
                movingRight = false;
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            if (transform.position.x <= startPosition.x - moveDistance)
            {
                movingRight = true;
                spriteRenderer.flipX = true;
            }
        }
    }
}