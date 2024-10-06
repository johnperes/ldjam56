using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class AntMan : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float jumpForce = 3.1f;
    private float moveSpeedDefault = 1f;
    private float jumpForceDefault = 3.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool minify;
    private bool canMove = true;
    private bool shrinkFinished = true;

    public Vector3 defaultScale;
    public float defaultPPU = 300f;
    public float maxPPU = 1200f;

    public PixelPerfectCamera pixelPerfectCamera;
    public CameraFollow cameraFollow;

    public void OnMove(InputValue value)
    {
        if (!canMove) return;

        moveInput = value.Get<Vector2>();

        if (isGrounded && moveInput.y > 0)
        {
            SoundManager.Instance.PlayJump();
            isGrounded = false;
            animator.SetTrigger("jumping");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (moveInput.y < 0 && shrinkFinished)
        {
            SoundManager.Instance.PlayShrink();
            shrinkFinished = false;
            minify = !minify;
            if (minify)
            {
                gameObject.layer = 10;
                cameraFollow.shrink = true;
            }
            StartCoroutine(nameof(SizeToggle));
        }

        if (moveInput.x == 0)
        {
            animator.SetBool("moving", false);
        }
        else
        {
            animator.SetBool("moving", true);
        }

        if (moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
        }
        else if (moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
        }
    }

    private void Start()
    {
        defaultScale = transform.localScale;
        moveSpeedDefault = moveSpeed;
        jumpForceDefault = jumpForce;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    IEnumerator SizeToggle()
    {
        yield return new WaitForSeconds(0.005f);
        if (minify)
        {
            pixelPerfectCamera.assetsPPU += 75;
        }
        else
        {
            pixelPerfectCamera.assetsPPU -= 75;
        }

        pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(Mathf.Clamp(pixelPerfectCamera.assetsPPU, defaultPPU, maxPPU));

        float newScale = defaultPPU / pixelPerfectCamera.assetsPPU;
        transform.localScale = defaultScale * newScale;
        cameraFollow.offset = new Vector3(cameraFollow.offset.x, 0.65f * newScale, cameraFollow.offset.z);
        moveSpeed = (moveSpeedDefault * 0.45f) + (moveSpeedDefault * 0.55f * newScale);
        jumpForce = (jumpForceDefault * 0.35f) + (jumpForceDefault * 0.65f * newScale);
        rb.gravityScale = 0.5f + (0.5f * newScale);

        if ((minify && pixelPerfectCamera.assetsPPU < maxPPU) || (!minify && pixelPerfectCamera.assetsPPU > defaultPPU))
        {
            StartCoroutine(nameof(SizeToggle));
        }
        else if (!minify)
        {
            gameObject.layer = 11;
            cameraFollow.shrink = false;
            shrinkFinished = true;
        }
        else
        {
            shrinkFinished = true;
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemySmall") && minify)
        {
            Die();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyNormal") && !minify)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Portal"))
        {
            SoundManager.Instance.PlayNextLevel();
            canMove = false;
            animator.SetTrigger("next-level");
            Invoke(nameof(NextLevel), .55f);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            Die();
        }
    }

    void Die()
    {
        SoundManager.Instance.PlayDeath();
        canMove = false;
        animator.SetTrigger("death");
        Invoke(nameof(ReloadLevel), 1f);
    }

    private void NextLevel()
    {
        GameController.Instance.NextLevel();
    }

    private void ReloadLevel()
    {
        GameController.Instance.ReloadLevel();
    }
}
