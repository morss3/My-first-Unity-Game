using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PatrolerEnemy : MonoBehaviour {
    public float speed;
    public int positionOfPatrol; //������� ��������������
    public Transform point; //����� �� ������� ���� ����� �������� � ���� ����� ������������
    bool moveingRight = false;
    Transform player;//��� �����
    public float stoppingDistance; //���������� �� ���������� �� ��
    public Animator anim;

    private Rigidbody2D rb;

    //��������� �����
    bool chill = false;
    bool angry = false;
    bool goBack = false;

    //�������� ����� (����� ��������)
    bool canAttack = true;

    EnemyHealth healthScript;
    PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>(); // �������� ��� ������

        anim = GetComponent<Animator>();
        if (anim == null) Debug.LogError("Animator not found!");
        else Debug.Log("Animator found");

        //player = GameObject.FindGameObjectWithTag("Player").transform;//����� ������� � ����� ������
        //playerHealth = player.GetComponent<PlayerHealth>();
        //if (playerHealth == null) {
        //    Debug.LogError("PlayerHealth �� ������ �� ������!");
        //}
        UpdatePlayerReference();

        healthScript = GetComponent<EnemyHealth>();


    }

    // Update is called once per frame
    void Update() {
        if (healthScript != null && healthScript.IsDead()) return;

        // �������� � ��������������� � ������, ���� �� ��� �����/���������
        if (player == null || playerHealth == null || playerHealth.IsDead()) {
            UpdatePlayerReference();
            if (player == null || playerHealth == null) return; // ��� ���� ��������
        }

        //if (playerHealth != null && playerHealth.IsDead()) {
        //    // ����� ����� � ���� �������� ��� ��������
        //    angry = false;
        //    chill = true;
        //    goBack = false;
        //    return; // ������ �� ������
        //}
        //���������� �� ����� ������� ����� �������������, �� ����� ������ ���������� �� ������� ����������� ����
        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && angry == false)
            chill = true; //��������� ��������� ��� ��� �� ������ ����������� ��� ������� � ����� ��� ������

        //���������� �� ����� �� �� ������ ������������
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance) {//
            angry = true;
            chill = false;
            goBack = false;
        }
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance) {
            goBack = true;
            angry = false;
        }

        ////����� �� ���� ����������
        //if (chill == true) {
        //    Chill();
        //}
        //else if (angry == true) {
        //    Angry();
        //}
        //else if (goBack == true) {
        //    GoBack();
        //}

        if (angry && canAttack) {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= healthScript.attackRange) {
                Debug.Log("Try attack");
                TryAttack();
            }
        }

        if (moveingRight)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    void FixedUpdate() {
        // ���������� �������� � FixedUpdate ��� ������� ������
        if (chill) Chill();
        else if (angry) Angry();
        else if (goBack) GoBack();
    }
    //void OnDrawGizmosSelected() {
    //Gizmos.color = Color.red;
    //Gizmos.DrawWireSphere(transform.position, healthScript.attackRange);
    //}
    //��������� ���������
    void Chill() {
        if (transform.position.x > point.position.x + positionOfPatrol) {//���� ������� �� ������� ��������������, �� ��� ���� ����������� ������
            moveingRight = false;
        }
        else if (transform.position.x < point.position.x - positionOfPatrol) {
            moveingRight = true;
        }
        //if (moveingRight) {
        //    transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        //}
        //else {
        //    transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        //}
        // �������� ������ ��������� position �� velocity
        float moveDirection = moveingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }
    void Angry() {
        if (player == null) return;
        //MoveToward(������ �������(����� �����), ���� �������(���� ������), �������� ����������) - ���������� � ���� �� (� ������)
        //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        //if (player.position.x > transform.position.x)
        //         moveingRight = true;
        //     else
        //         moveingRight = false;

        // ������� ���������� � ������� MoveTowards, �� ������ �� X ���
        float targetX = player.position.x;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        
        // ��������� �������� ����� velocity, �������� Y-������������ (����������)
        rb.velocity = new Vector2((newX - transform.position.x) / Time.deltaTime, rb.velocity.y);

        // ��������� ����������� �������
        if (player.position.x > transform.position.x)
            moveingRight = true;
        else
            moveingRight = false;

    }

    void GoBack() {
        //MoveToward(������ �������(����� �����), ���� �������(���� ����� ��������������), �������� ����������) - ���������� � ���� �� (� �����)
        //transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
       
        // ������� ����������� � ����� �� X
        float targetX = point.position.x;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        rb.velocity = new Vector2((newX - transform.position.x) / Time.deltaTime, rb.velocity.y);
        if (player.position.x > transform.position.x)
            moveingRight = true;
        else
            moveingRight = false;
    }

    void TryAttack() {
        canAttack = false;  // ��������� ��������� �����

        healthScript.LastAttackTime = Time.time;
        anim.SetTrigger("Attack");
        Invoke(nameof(DealDamage), 0.4f); // ������� ���� ����� 0.6 �������

        // �������� ����������� ����� ����� ����� cooldown
        Invoke(nameof(ResetAttack), healthScript.attackCooldown);
    }
    void ResetAttack() {
        canAttack = true;
    }


    public void DealDamage() {
        Debug.Log("DealDamage called");

        if (player == null) {
            Debug.Log("player == null, ������� FindGameObjectWithTag!");
            return;
        }

        if (playerHealth == null) {
            Debug.Log("PlayerHealth �� ������ �� ������� " + player.name);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= healthScript.attackRange) {
            playerHealth.TakeDamage(healthScript.damageToPlayer);
            Debug.Log(">> DAMAGE DEALT TO PLAYER <<");
        }
        else {
            Debug.Log("����� ��� ���� �����");
        }
    }
    void UpdatePlayerReference() {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) {
            player = playerObj.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
            Debug.Log("Player re-referenced by enemy.");
        }
        else {
            Debug.Log("Player not found by enemy.");
        }
    }

}
//������� ����


//public class PatrolerEnemy : MonoBehaviour
//{
//    public float speed;
//    public int positionOfPatrol; //������� ��������������
//    public Transform point; //����� �� ������� ���� ����� �������� � ���� ����� ������������
//    bool moveingRight = false;
//    Transform player;//��� �����
//    public float stoppingDistance; //���������� �� ���������� �� ��
//    Animator anim;


//    //��������� �����
//    bool chill = false;
//    bool angry = false;
//    bool goBack = false;

//    public float attackCooldown = 1f;
//    private float lastAttackTime = -10f;
//    public int damageToPlayer = 1;

//    EnemyHealth healthScript;
//    // Start is called before the first frame update
//    void Start()
//    {
//        anim = GetComponent<Animator>();
//        player = GameObject.FindGameObjectWithTag("Player").transform;//����� ������� � ����� ������
//        healthScript = GetComponent<EnemyHealth>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (healthScript == null || healthScript.enabled == false) return;

//        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
//        //���������� �� ����� ������� ����� �������������, �� ����� ������ ���������� �� ������� ����������� ����
//        if (distanceToPlayer < positionOfPatrol && angry==false) 
//            chill = true; //��������� ��������� ��� ��� �� ������ ����������� ��� ������� � ����� ��� ������

//        //���������� �� ����� �� �� ������ ������������
//        if(distanceToPlayer < stoppingDistance) {//
//            angry = true;
//            chill = false;
//            goBack = false;
//        }
//        if (distanceToPlayer > stoppingDistance) {
//            goBack = true;
//            angry = false;
//        }

//        //����� �� ���� ����������
//        if (chill==true) {
//            Chill();
//        }
//        else if (angry==true) {
//            Angry();
//        }
//        else if(goBack==true) {
//            GoBack();
//        }

//        if (moveingRight)
//            transform.localScale = new Vector3(1f, 1f, 1f);
//        else
//            transform.localScale = new Vector3(-1f, 1f, 1f);

//        // ������ �����
//        if (angry && distanceToPlayer < 1.2f && Time.time > lastAttackTime + attackCooldown) {
//            anim.SetTrigger("attack");
//            if (player.TryGetComponent<PlayerHealth>(out var playerHealth)) {
//                playerHealth.TakeDamage(damageToPlayer);
//            }
//            lastAttackTime = Time.time;
//        }

//    }

//    //��������� ���������
//    void Chill() {
//        if(transform.position.x>point.position.x+positionOfPatrol){//���� ������� �� ������� ��������������, �� ��� ���� ����������� ������
//            moveingRight = false;
//        } else if (transform.position.x < point.position.x - positionOfPatrol) {
//            moveingRight = true;
//        }
//        if(moveingRight) {
//            transform.position = new Vector2(transform.position.x+speed*Time.deltaTime,transform.position.y);
//        }
//        else {
//            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
//        }
//    }
//    void Angry() {
//        //MoveToward(������ �������(����� �����), ���� �������(���� ������), �������� ����������) - ���������� � ���� �� (� ������)
//        transform.position = Vector2.MoveTowards(transform.position, player.position, speed*Time.deltaTime);
//        if (player.position.x > transform.position.x)
//            moveingRight = true;
//        else
//            moveingRight = false;

//    }

//    void GoBack() {
//        //MoveToward(������ �������(����� �����), ���� �������(���� ����� ��������������), �������� ����������) - ���������� � ���� �� (� �����)
//        transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
//        if (player.position.x > transform.position.x)
//            moveingRight = true;
//        else
//            moveingRight = false;

//    }


//}
