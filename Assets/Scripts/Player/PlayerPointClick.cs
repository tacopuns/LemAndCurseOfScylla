using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPointClick : MonoBehaviour, IDataGrabber
{
    private Vector2 target;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    public GameObject inventoryGO;

    public Animator camAnimator;
    int PlayerCam = 0;
    int DialogueCam = 0;

    [Header("Keyboard Movement")]
    public float keyboardSpeed = 15f; // Speed multiplier for WASD movement
    private bool usingKeyboard = false;


    public void LoadData(GameData data)
    {
        transform.position = data.position;
    }

    public void SaveData(GameData data)
    {
        data.position = transform.position;
    }

    private void Start()
    {
        target = transform.position;
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        camAnimator = GetComponent<Animator>();
        camAnimator.enabled = true;

        PlayerCam = 1;
    }

    private void Update()
    {
        HandleMouseInput();
        HandleKeyboardInput();
        OpenInventory();

        camAnimator.SetInteger("PlayerCam", PlayerCam);
        camAnimator.SetInteger("DialogueCam", DialogueCam);
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && 
            !PauseMenu.instance.GetPauseStatus() && 
            GameManager.instance.lemCanMove && 
            !InventoryManager.instance.currentlyHoveringItem)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target = new Vector3(mousePosition.x, mousePosition.y, 0f);
            agent.isStopped = false; // Resume NavMeshAgent movement
            agent.SetDestination(new Vector3(target.x, target.y, 0f));
            usingKeyboard = false; // Mouse takes priority
            HandleSpriteFlip();
        }
        else if (Input.GetMouseButtonDown(0) && 
                 !PauseMenu.instance.GetPauseStatus() && 
                 !GameManager.instance.lemCanMove)
        {
            if (DialogueManager.instance.currentNPC == null) return;
            DialogueManager.instance.ContinueDialogue();
        }
    }

    private void HandleKeyboardInput()
    {
       if (PauseMenu.instance.GetPauseStatus() || !GameManager.instance.lemCanMove) return;

    float moveX = Input.GetAxisRaw("Horizontal"); // A/D
    float moveY = Input.GetAxisRaw("Vertical");   // W/S

    Vector3 inputDir = new Vector3(moveX, moveY, 0f).normalized;

    if (inputDir.magnitude > 0.1f) // Active WASD input
    {
        usingKeyboard = true;

        // Stop following mouse path
        agent.ResetPath();

        // Calculate a new target slightly in front of the player
        Vector3 nextPos = transform.position + inputDir * keyboardSpeed * Time.deltaTime;

        // Continuously move the agent toward that target
        agent.isStopped = false;
        agent.SetDestination(nextPos);

        // Flip sprite based on movement direction
        if (moveX < 0) spriteRenderer.flipX = false;
        else if (moveX > 0) spriteRenderer.flipX = true;
    }
    else if (usingKeyboard) // No input, stop
    {
        agent.ResetPath();
        agent.isStopped = true;
    }
    }

    public void SetTarget(Vector3 targetPositionToSet)
    {
        target = targetPositionToSet;
        agent.isStopped = false;
        agent.SetDestination(new Vector3(target.x, target.y, 0f));
        usingKeyboard = false;
    }

    private void HandleSpriteFlip()
    {
        if (target.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void WarpPlayer(Vector3 newPosition)
    {
        agent.Warp(newPosition);
        target = transform.position;
    }

    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryGO.SetActive(!inventoryGO.activeInHierarchy);
        }
    }

    public void LemTalking()
    {
        Debug.Log("lem talking active");
        PlayerCam = 0;
        DialogueCam = 1;
    }

    public void LemNotTalking()
    {
        Debug.Log("lem talking not active");
        PlayerCam = 1;
        DialogueCam = 0;
    }
}