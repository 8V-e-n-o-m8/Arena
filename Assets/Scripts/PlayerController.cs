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

        if (Input.GetButtonDown("Fire1")) // ����������� ������ ��������, ������� �������� ������ ����������
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
            // ������� ���� �� ������� _bulletSpawnPoint
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);

            // �������� ��������� Rigidbody2D ���� � ������� �� �������� � ����������� ������� ������
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = _bulletSpawnPoint.up * _bulletSpeed; // bulletSpeed - �������� ����
            }

            // ��������� ������ ��������� ������������ �� ����
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                bulletScript.SetShooter(gameObject); // ������������� ����������� (������ ������) ��� ����������� ������
            }
        }
    }

    private void RestartGame()
    {
        _hasShield = false;
        // �������� ������ ������� ����� � ������������� ��
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void PlayGunshotSound()
    {
        // ���������, ��� ���� ���� �������� � AudioSource
        if (gunshotSound != null && audioSource != null)
        {
            // ������������� ���� ��������
            audioSource.PlayOneShot(gunshotSound);
        }
    }

    private void ActivateAura()
    {
        // ��� ��� ��������� ����, ��������, ��������� �������
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
                // ����� ����� ���, ���������� ����� �����
                _hasShield = false;
                return;
            }

            // ������������ � ������, �������� ������� ����������� ����
            RestartGame();
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            // ������ ����
            _hasShield = true;
            ActivateAura(); // ���������� ���� ��� ������� ����
            Destroy(collision.gameObject); // ���������� ������ ����
        }
    }

    private IEnumerator Dash()
    {
        if (_canDash)
        {
            _canDash = false;
            _isDashing = true;

            // �������� ������� ����������� ��������
            Vector2 dashDirection = _rigidbody2D.velocity.normalized;

            // ������������� �������� � ����������� ����� � �������������� _dashingPower
            _rigidbody2D.velocity = dashDirection * _dashingPower;

            // ���� ����� �������� �����
            yield return new WaitForSeconds(_dashingTime);

            // ���������� ��������
            _rigidbody2D.velocity = Vector2.zero;

            _isDashing = false;

            // ���� �������� ����� ��������� ������
            yield return new WaitForSeconds(_dashingCooldown);
            _canDash = true;
        }
    }
}
