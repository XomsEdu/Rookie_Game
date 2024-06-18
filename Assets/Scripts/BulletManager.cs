using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float lifetime = 10f; // Час життя кулі
    public LayerMask collisionLayers; // Маска слоїв для перевірки зіткнень

    private void Start()
    {
        // Знищити кулю коли час закінчиться
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Перевірка чи шар об'єкта, з яким відбулося зіткнення, входить до маски слоїв
        if ((collisionLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Знищити кулю при зіткненні з об'єктом
            Destroy(gameObject);

            // Знищення стіни з тегом
            if (collision.CompareTag("DestructibleWall"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
