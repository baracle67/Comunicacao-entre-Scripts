using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float gravityModifier;
    private bool isOnGround;
    private static bool gameOver;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    [SerializeField] int currentLifes = 3;
    [SerializeField] int maxLifes;
    [SerializeField] HudManager hudManager;

    InputAction jumpAction;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        currentLifes = maxLifes;
        hudManager.updateLifes(currentLifes);
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpAction.WasPerformedThisFrame() && isOnGround && !gameOver)
        {
            Jump();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            isOnGround = true; 
            dirtParticle.Play();
        } else if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentLifes--;
            hudManager.updateLifes(currentLifes);
            if(currentLifes == 0)
            {
                processGameOver();
            }

            
        }     
    }
    private void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        playerAnim.SetTrigger("Jump_trig");
        dirtParticle.Stop();
    }

    public static bool isGameOver()
    {
        return gameOver;
    }

    private void processGameOver()
    {
        Debug.Log("Game Over");
        gameOver = true;
        playerAnim.SetInteger("DeathType_int", 1);
        playerAnim.SetBool("Death_b", true);
        dirtParticle.Stop();
        explosionParticle.Play();
    }

    private void Awake()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
    }
}
