using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private GameObject _shooter; // ������ �� ����������� (������ ������)

    public void SetShooter(GameObject shooter)
    {
        _shooter = shooter;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _shooter != null)
        {
            // ���������� ����� � ����
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            // ���������� ���� ��� ������������ �� ������
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // ���������, ������� �� ���� �� ������� ������
        if (!IsObjectVisible())
        {
            Destroy(gameObject);
        }
    }

    private bool IsObjectVisible()
    {
        // ���������, ������ �� ���� � ������
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1);
    }
}
