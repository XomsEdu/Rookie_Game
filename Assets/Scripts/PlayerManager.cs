using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Vector2 moveDirection; // це значення вектора яке використовується в визначенні напрямку за натисненими клавішами
    InputManager inputManager; // це виклик скріпта inputManager
    Rigidbody2D rigidbody; // це виклик компонента rigidbody прикріпленого до об'єкту гравця
    private bool isGrounded; // це значення що перевіряє чи приземлений гравець
    Animator animator;

    public LayerMask ground; // це поле в яке можна задати маску слою з яким взаємодіє рейкаст що перевіряє поверхню

    public float moveSpeed; // числове значення швидкості руху
    public float hopsForce; // числ. значення сили стрибка
    public int hopsAmount = 2; // стала к-кість стрибків які можна зробити в повітрі
    public float gravityMultiplier = 2.5f; // змінне значення сили тяжіння для кращого відчуття стрибка
    public float gravityHopMultiplier;

    public int hopsCount; // змінна к-кість стрибків (тут активно оновлюється число доступних стрибків)

    public Vector2 groundBoxCast; // векторне значення рейкасту що перевіряє поверхню
    public float castDistance; // числове значення довжини рейкасту 

    public GameObject bulletPrefab; // префаб кулі
    public Transform bulletSpawnPoint; // точка появи кулі
    public float bulletSpeed = 10f; // швидкість кулі

    private void Awake() // цей метод викликає ці компоненти на початку гри
    {
        inputManager = GetComponent<InputManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(rigidbody.velocity.x != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        PlayerRotation();
        GravityUpdate();
        PlayerShoot();
    }

    private void GravityUpdate() // цей метод відповідає за гравітацію для приємнішого стрибка
    {
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;  // falling
        }
        else if (rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (gravityHopMultiplier - 1) * Time.deltaTime;  // low jump
        }
    }

    private void FixedUpdate() // цей метод апдейтить операції з цих методів але з певною специфікою (у цьому методі краще апдейтити значення які працюють з фізикою типу velocity)
    {
        PlayerMovement();
        GroundCheck();
        PlayerHops();
    }

    private void PlayerMovement()
    {
        moveDirection = inputManager.movementInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, rigidbody.velocity.y);
    }

    private void PlayerRotation() // візуальний розворіт персонажа
    {
        if (rigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (rigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void GroundCheck() // цей метод вимальовує рейкаст який перевіряє поверхню
    {
        Vector2 boxOrigin = transform.position;
        Vector2 boxDirection = -transform.up;

        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, groundBoxCast, 0f, boxDirection, castDistance, ground); // це сам рейкаст

        if (hit.collider != null)
        {
            isGrounded = true;
            hopsCount = hopsAmount; // оновлення к-кості стрибків на поверхні
        }
        else
        {
            isGrounded = false;
        }
    }

    private void PlayerHops() // метод що обчислює стрибки
    {
        if (inputManager.isJump)
        {
            if (isGrounded)
            {
                rigidbody.velocity = Vector2.up * hopsForce;
                inputManager.isJump = false;
            }

            if (!isGrounded && hopsCount > 0)
            {
                hopsCount -= 1;
                rigidbody.velocity = Vector2.up * hopsForce;
                inputManager.isJump = false;
            }
        }
    }

    private void PlayerShoot() //метод для стрільби
    {
        if (inputManager.isFire)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            if (transform.localScale.x > 0)
            {
                bulletRb.velocity = new Vector2(bulletSpeed, 0);
            }
            else
            {
                bulletRb.velocity = new Vector2(-bulletSpeed, 0);
            }

            inputManager.isFire = false;
        }
    }
}
