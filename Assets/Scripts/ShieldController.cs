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
            // ѕровер€ем столкновение с врагом
            if (collision.CompareTag("Enemy"))
            {
                // ќтключаем у€звимость после первого столкновени€ с врагом
                _isImmortal = true;

                // »гнорируем урон от врагов дл€ некоторого времени
                StartCoroutine(DisableImmortalityTemporarily());
            }
        }
    }

    private IEnumerator DisableImmortalityTemporarily()
    {
        yield return new WaitForSeconds(1f); // ¬рем€, в течение которого персонаж неу€звим

        // ѕерсонаж снова у€звим
        _isImmortal = false;
    }
}
