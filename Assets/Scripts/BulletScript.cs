using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private GameObject _shooter; // Ссылка на стреляющего (вашего игрока)

    public void SetShooter(GameObject shooter)
    {
        _shooter = shooter;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _shooter != null)
        {
            // Уничтожаем врага и пулю
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            // Уничтожаем пулю при столкновении со стеной
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Проверяем, выходит ли пуля за пределы экрана
        if (!IsObjectVisible())
        {
            Destroy(gameObject);
        }
    }

    private bool IsObjectVisible()
    {
        // Проверяем, видима ли пуля в камере
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1);
    }
}
