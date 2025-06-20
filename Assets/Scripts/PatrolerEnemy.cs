using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PatrolerEnemy : MonoBehaviour {
    public float speed;
    public int positionOfPatrol; //площадь патрулирования
    public Transform point; //точка от которой враг будет отходить и куда будет возвращаться
    bool moveingRight = false;
    Transform player;//где игрок
    public float stoppingDistance; //расстояние от противника до ГГ
    public Animator anim;

    private Rigidbody2D rb;

    //состояния врага
    bool chill = false;
    bool angry = false;
    bool goBack = false;

    //контроль атаки (после кулдавна)
    bool canAttack = true;

    EnemyHealth healthScript;
    PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>(); // Добавьте эту строку

        anim = GetComponent<Animator>();
        if (anim == null) Debug.LogError("Animator not found!");
        else Debug.Log("Animator found");

        //player = GameObject.FindGameObjectWithTag("Player").transform;//поиск объекта с тегом плейер
        //playerHealth = player.GetComponent<PlayerHealth>();
        //if (playerHealth == null) {
        //    Debug.LogError("PlayerHealth не найден на игроке!");
        //}
        UpdatePlayerReference();

        healthScript = GetComponent<EnemyHealth>();


    }

    // Update is called once per frame
    void Update() {
        if (healthScript != null && healthScript.IsDead()) return;

        // Проверка и переподключение к игроку, если он был удалён/респавнен
        if (player == null || playerHealth == null || playerHealth.IsDead()) {
            UpdatePlayerReference();
            if (player == null || playerHealth == null) return; // ждём пока появится
        }

        //if (playerHealth != null && playerHealth.IsDead()) {
        //    // Игрок мертв — враг перестаёт его замечать
        //    angry = false;
        //    chill = true;
        //    goBack = false;
        //    return; // ничего не делаем
        //}
        //расстояние от точки которую нужно патрулировать, до врага меньше расстояния на которое патрулирует враг
        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && angry == false)
            chill = true; //спокойное состояние так как он просто патрулирует эту область и рядом нет игрока

        //расстояние от врага до гг меньше дозволенного
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance) {//
            angry = true;
            chill = false;
            goBack = false;
        }
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance) {
            goBack = true;
            angry = false;
        }

        ////чтобы не было конфликтов
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
        // Обновление скорости в FixedUpdate для плавной физики
        if (chill) Chill();
        else if (angry) Angry();
        else if (goBack) GoBack();
    }
    //void OnDrawGizmosSelected() {
    //Gizmos.color = Color.red;
    //Gizmos.DrawWireSphere(transform.position, healthScript.attackRange);
    //}
    //состояния персонажа
    void Chill() {
        if (transform.position.x > point.position.x + positionOfPatrol) {//если выходит за границу патрулирования, то ему надо повернуться налево
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
        // Заменяем прямое изменение position на velocity
        float moveDirection = moveingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }
    void Angry() {
        if (player == null) return;
        //MoveToward(откуда следует(коорд врага), куда следует(коор игрока), скорость следования) - следование к чему то (к игроку)
        //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        //if (player.position.x > transform.position.x)
        //         moveingRight = true;
        //     else
        //         moveingRight = false;

        // Плавное следование с помощью MoveTowards, но только по X оси
        float targetX = player.position.x;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        
        // Применяем движение через velocity, сохраняя Y-составляющую (гравитацию)
        rb.velocity = new Vector2((newX - transform.position.x) / Time.deltaTime, rb.velocity.y);

        // Обновляем направление взгляда
        if (player.position.x > transform.position.x)
            moveingRight = true;
        else
            moveingRight = false;

    }

    void GoBack() {
        //MoveToward(откуда следует(коорд врага), куда следует(коор точки патрулирования), скорость следования) - следование к чему то (к точке)
        //transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
       
        // Плавное возвращение к точке по X
        float targetX = point.position.x;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        rb.velocity = new Vector2((newX - transform.position.x) / Time.deltaTime, rb.velocity.y);
        if (player.position.x > transform.position.x)
            moveingRight = true;
        else
            moveingRight = false;
    }

    void TryAttack() {
        canAttack = false;  // блокируем повторный вызов

        healthScript.LastAttackTime = Time.time;
        anim.SetTrigger("Attack");
        Invoke(nameof(DealDamage), 0.4f); // наносим урон через 0.6 секунды

        // Включаем возможность атаки снова после cooldown
        Invoke(nameof(ResetAttack), healthScript.attackCooldown);
    }
    void ResetAttack() {
        canAttack = true;
    }


    public void DealDamage() {
        Debug.Log("DealDamage called");

        if (player == null) {
            Debug.Log("player == null, проверь FindGameObjectWithTag!");
            return;
        }

        if (playerHealth == null) {
            Debug.Log("PlayerHealth не найден на объекте " + player.name);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= healthScript.attackRange) {
            playerHealth.TakeDamage(healthScript.damageToPlayer);
            Debug.Log(">> DAMAGE DEALT TO PLAYER <<");
        }
        else {
            Debug.Log("Игрок вне зоны атаки");
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
//рабочее выше


//public class PatrolerEnemy : MonoBehaviour
//{
//    public float speed;
//    public int positionOfPatrol; //площадь патрулирования
//    public Transform point; //точка от которой враг будет отходить и куда будет возвращаться
//    bool moveingRight = false;
//    Transform player;//где игрок
//    public float stoppingDistance; //расстояние от противника до ГГ
//    Animator anim;


//    //состояния врага
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
//        player = GameObject.FindGameObjectWithTag("Player").transform;//поиск объекта с тегом плейер
//        healthScript = GetComponent<EnemyHealth>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (healthScript == null || healthScript.enabled == false) return;

//        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
//        //расстояние от точки которую нужно патрулировать, до врага меньше расстояния на которое патрулирует враг
//        if (distanceToPlayer < positionOfPatrol && angry==false) 
//            chill = true; //спокойное состояние так как он просто патрулирует эту область и рядом нет игрока

//        //расстояние от врага до гг меньше дозволенного
//        if(distanceToPlayer < stoppingDistance) {//
//            angry = true;
//            chill = false;
//            goBack = false;
//        }
//        if (distanceToPlayer > stoppingDistance) {
//            goBack = true;
//            angry = false;
//        }

//        //чтобы не было конфликтов
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

//        // логика атаки
//        if (angry && distanceToPlayer < 1.2f && Time.time > lastAttackTime + attackCooldown) {
//            anim.SetTrigger("attack");
//            if (player.TryGetComponent<PlayerHealth>(out var playerHealth)) {
//                playerHealth.TakeDamage(damageToPlayer);
//            }
//            lastAttackTime = Time.time;
//        }

//    }

//    //состояния персонажа
//    void Chill() {
//        if(transform.position.x>point.position.x+positionOfPatrol){//если выходит за границу патрулирования, то ему надо повернуться налево
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
//        //MoveToward(откуда следует(коорд врага), куда следует(коор игрока), скорость следования) - следование к чему то (к игроку)
//        transform.position = Vector2.MoveTowards(transform.position, player.position, speed*Time.deltaTime);
//        if (player.position.x > transform.position.x)
//            moveingRight = true;
//        else
//            moveingRight = false;

//    }

//    void GoBack() {
//        //MoveToward(откуда следует(коорд врага), куда следует(коор точки патрулирования), скорость следования) - следование к чему то (к точке)
//        transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
//        if (player.position.x > transform.position.x)
//            moveingRight = true;
//        else
//            moveingRight = false;

//    }


//}
