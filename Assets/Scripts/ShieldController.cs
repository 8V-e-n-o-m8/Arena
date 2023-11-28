using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private bool _isImmortal = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isImmortal)
        {
            // ��������� ������������ � ������
            if (collision.CompareTag("Enemy"))
            {
                // ��������� ���������� ����� ������� ������������ � ������
                _isImmortal = true;

                // ���������� ���� �� ������ ��� ���������� �������
                StartCoroutine(DisableImmortalityTemporarily());
            }
        }
    }

    private IEnumerator DisableImmortalityTemporarily()
    {
        yield return new WaitForSeconds(1f); // �����, � ������� �������� �������� ��������

        // �������� ����� ������
        _isImmortal = false;
    }
}
