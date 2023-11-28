using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _bulletSpeed = 25f;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;

    private Rigidbody2D _rigidbody2D;
    private bool _canDash = true;
    private bool _isDashing;
    private float _dashingPower = 20f;
    private float _dashingTime = 0.2f;
    private float _dashingCooldown = 2f;
    private bool _hasShield = false;
    private AudioSource audioSource;

    public AudioClip gunshotSound;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_isDashing)
        {
            return;
        }

        MovePlayer();
        RotateTowardsMouse();

        if (Input.GetButtonDown("Fire1")) // Используйте кнопку стрельбы, которая подходит вашему управлению
        {
            Shoot();
            PlayGunshotSound();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (_isDashing)
            return;
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();

        _rigidbody2D.velocity = movement * _speed;

        _animator.SetFloat("Speed", Mathf.Abs(_speed));
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Shoot()
    {
        if (_bulletPrefab != null && _bulletSpawnPoint != null)
        {
            // Создаем пулю на позиции _bulletSpawnPoint
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);

            // Получаем компонент Rigidbody2D пули и придаем ей скорость в направлении взгляда игрока
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = _bulletSpawnPoint.up * _bulletSpeed; // bulletSpeed - скорость пули
            }

            // Добавляем скрипт обработки столкновений на пулю
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                bulletScript.SetShooter(gameObject); // Устанавливаем стреляющего (вашего игрока) для определения врагов
            }
        }
    }

    private void RestartGame()
    {
        _hasShield = false;
        // Получаем индекс текущей сцены и перезапускаем ее
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void PlayGunshotSound()
    {
        // Проверяем, что есть звук выстрела и AudioSource
        if (gunshotSound != null && audioSource != null)
        {
            // Воспроизводим звук выстрела
            audioSource.PlayOneShot(gunshotSound);
        }
    }

    private void ActivateAura()
    {
        // Ваш код активации ауры, например, включение спрайта
        SpriteRenderer auraRenderer = GetComponent<SpriteRenderer>();
        if (auraRenderer != null)
        {
            auraRenderer.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_hasShield)
            {
                // Игрок имеет щит, переживаем атаку врага
                _hasShield = false;
                return;
            }

            // Столкновение с врагом, вызываем функцию перезапуска игры
            RestartGame();
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            // Подбор щита
            _hasShield = true;
            ActivateAura(); // Активируем ауру при подборе щита
            Destroy(collision.gameObject); // Уничтожаем объект щита
        }
    }

    private IEnumerator Dash()
    {
        if (_canDash)
        {
            _canDash = false;
            _isDashing = true;

            // Получаем текущее направление движения
            Vector2 dashDirection = _rigidbody2D.velocity.normalized;

            // Устанавливаем скорость в направлении рывка с использованием _dashingPower
            _rigidbody2D.velocity = dashDirection * _dashingPower;

            // Ждем время действия рывка
            yield return new WaitForSeconds(_dashingTime);

            // Сбрасываем скорость
            _rigidbody2D.velocity = Vector2.zero;

            _isDashing = false;

            // Ждем кулдауна перед следующим рывком
            yield return new WaitForSeconds(_dashingCooldown);
            _canDash = true;
        }
    }
}
