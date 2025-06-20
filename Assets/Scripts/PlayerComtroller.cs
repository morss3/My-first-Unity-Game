using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class PlayerComtroller : MonoBehaviour
{
    public float playerSpeed;
    public Rigidbody2D playerRb;
    private float movementX;
    public Animator anim;

    public float jumpForce = 25f; //power of jump
    public Transform legs;
    public LayerMask groundLayer;

    public GameObject slashEffectPrefab;
    //private Animator slashEffectAnimator; // Аниматор для SlashEffect
//    public Transform attackPoint;
    public float attackCooldown = 0.5f;
    private bool isAttacking = false;
    private float lastAttackTime=-10f;


    private PlayerHealth playerHealth;
    private void Start() {
        playerHealth = GetComponent<PlayerHealth>(); // Получаем ссылку на компонент PlayerHealth
        //slashEffectAnimator = slashEffectPrefab.GetComponent<Animator>();
    }
    private void Update() {
        if (!isAttacking) {
            movementX = Input.GetAxisRaw("Horizontal");
        }
        else {
            movementX = 0f;
        }
        movementX = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isAttacking) {
            Jump();
        }
        if (Mathf.Abs(movementX) > 0.05f) {
            anim.SetBool("isWalking", true);
        }
        else {
            anim.SetBool("isWalking", false);
        }
        if (movementX > 0f) {
            transform.localScale=new Vector3(1f,1f,1f);
        }
        else if(movementX < 0f){
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        
        anim.SetBool("isGrounded", IsGrounded());
        if (Input.GetKeyDown(KeyCode.X) && Time.time >= lastAttackTime + attackCooldown) {
            Attack();
        }
    }

    private void FixedUpdate() {
        if (!isAttacking) {
            Vector2 movement = new Vector2(movementX * playerSpeed, playerRb.velocity.y);
            playerRb.velocity = movement;
        }
        else {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
        }
        if (!playerHealth.IsDead() && transform.position.y < -10f) {
            Debug.Log("Игрок упал за пределы карты");
            playerHealth.TakeDamage(playerHealth.playerLives); // или FallToDeath()
        }
        //Vector2 movement = new Vector2(movementX * playerSpeed, playerRb.velocity.y);
        //playerRb.velocity = movement;
    }

    void Jump() {
        Vector2 movement = new Vector2(playerRb.velocity.x, jumpForce);
        playerRb.velocity = movement;

    }

    public bool IsGrounded() {
        Collider2D checkGround = Physics2D.OverlapCircle(legs.position, 0.5f, groundLayer);
        if(checkGround != null) {
            return true;
        }
        return false;
    }
    void Attack() {
        if(isAttacking) {
            return;
        }
        // Анимация атаки
        anim.SetTrigger("attack");

        // Запуск анимации SlashEffect
        if (slashEffectPrefab != null) {
            DestroyAttack destroyAttackScript = slashEffectPrefab.GetComponent<DestroyAttack>();
            if (destroyAttackScript != null) {
                destroyAttackScript.ActivateSlashEffect(); // Активируем слеш-эффект
            }
        }
        // Блокируем повторную атаку до конца кулдауна
        isAttacking = true;

        // Обновляем время последней атаки
        lastAttackTime = Time.time;

        // Делаем паузу (задержка) перед сбросом флага атаки
        Invoke(nameof(ResetAttack), attackCooldown); // После кулдауна разблокируем возможность атаки
    }

        void ResetAttack() {
        isAttacking = false;
    }

    

}
