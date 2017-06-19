using UnityEngine;

public class DisableProjectile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyMissile" || collision.gameObject.tag == "PlayerMissile")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
