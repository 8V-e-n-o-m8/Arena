using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _health = 2; // ������ ��������� ��������
    [SerializeField] private bool _shieldAvailable = true;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private AudioClip _damageSound; // ����� ���� ��� ����� �����
    private AudioSource _audioSource;

    private GameObject _hero;
    private static int _killCount = 0;

    void Start()
    {
        _hero = GameObject.FindGameObjectWithTag("Player");
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_hero != null)
        {
            float distance = Vector2.Distance(transform.position, _hero.transform.position);
            Vector2 direction = _hero.transform.position - transform.position;

            // ���� ���������� �� ������ ������ ������������� ��������, ���������� ��������
            if (distance > 0.2f)
            {
                _animator.SetFloat("Speed", _speed);

                transform.position = Vector2.MoveTowards(transform.position, _hero.transform.position, _speed * Time.deltaTime);

                // ������������ �����, ����� �� ������� �� ������
                Vector3 targetDir = _hero.transform.position - transform.position;
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            _health--;

            // ������������� ���� �����
            if (_damageSound != null && _audioSource != null)
            {
                _audioSource.Stop();
                _audioSource.clip = _damageSound;
                _audioSource.Play();
            }

            if (_health <= 0)
            {
                _killCount++; // ����������� ������� �������

                if (_killCount % 20 == 0 && _shieldAvailable)
                {
                    // ��������� ������� ���� � ��������� � �� �������
                    if (_hero.GetComponent<ShieldController>() == null && GameObject.FindObjectOfType<ShieldController>() == null)
                    {
                        // ������� ��������� ����
                        Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
                        _shieldAvailable = false; // ��������, ��� ��� ����� � ������ �� ��������
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
