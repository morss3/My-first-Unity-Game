using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public int maxHealth = 3;
    private int _currentHealth;
    private bool _isDead = false;

    private Animator _anim;

    public float attackCooldown = 1f;
    private float _lastAttackTime = 0f;
    public int damageToPlayer = 1;
    public float attackRange = 2f;
    
    //EnemyHealth healthScript;

    //новое
    public bool isKnockbacking = false;
    private float knockbackTimer = 0f;
    private const float knockbackDuration = 0.2f;
//
    void Start() { 
        _currentHealth = maxHealth;
        _anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, Vector2 attackDirection) {
        if (_isDead) return;
        Debug.Log("Attack");
        _currentHealth -= damage;
        _anim.SetTrigger("hit");

         Knockback(attackDirection);


        if (_currentHealth <= 0) {
            Invoke(nameof(Die), 0.15f); //подождем пока отлетит
        }
    }

    void Die() {
        if (_isDead) return;
        Debug.Log("die");
        _isDead = true;
        _anim.SetTrigger("Dead");
        GetComponent<Collider2D>().enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //this.enabled = false; // Отключаем AI врага
        Destroy(gameObject, 1.5f); // Уничтожаем через 2 секунды (успевает проиграть анимацию смерти)

    }
    

    void Knockback(Vector2 attackDirection) {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Сброс скорости и толчок
        rb.velocity = Vector2.zero;
        rb.AddForce(attackDirection.normalized * 5000f, ForceMode2D.Impulse);

        // Отключить патруль и ИИ
        GetComponent<PatrolerEnemy>().enabled = false;

        // Включить обратно через 0.2 сек
        Invoke(nameof(EnableAI), 0.2f);
    }

    void EnableAI() {
        if (!_isDead) {
            GetComponent<PatrolerEnemy>().enabled = true;
        }
    }
    public bool IsDead() {
            return _isDead;
        }
    public float LastAttackTime {
        get => _lastAttackTime;
        set => _lastAttackTime = value;
    }

}

