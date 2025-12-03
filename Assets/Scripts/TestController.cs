using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestController : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private NavMeshAgent agent;

    public float moveSpeed = 5f;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Only move if pressing a key
        if (moveX != 0 || moveY != 0)
        {
            Vector3 direction = new Vector3(moveX, 0f, moveY).normalized;
            Vector3 targetPosition = transform.position + direction * moveSpeed;

            // Tell agent to walk there
            agent.SetDestination(targetPosition);
        }
        else
        {
            // Stop agent when no input
            agent.ResetPath();
        }

        if (moveX < 0) spriteRenderer.flipX = false;
        else if (moveX > 0) spriteRenderer.flipX = true;
    }
}
