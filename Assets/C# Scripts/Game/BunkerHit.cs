using UnityEngine;

public class BunkerHit : MonoBehaviour
{
    private SpriteRenderer bunkerSpriteRenderer;

    private DestroyBunker destroyBunker;

    private Color[] bunkerColors = new Color[2];

    private int bunkerHealth;

	// Use this for initialization
	void Start ()
    {
        bunkerHealth = 2;
        bunkerColors[0] = new Color(1.0f, 0.31f, 0.19f, 1.0f);
        bunkerColors[1] = new Color(0.96f, 0.875f, 0.408f, 1.0f);
        bunkerSpriteRenderer = GetComponent<SpriteRenderer>();
        destroyBunker = GetComponentInParent<DestroyBunker>();
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerMissile" || collision.gameObject.tag == "EnemyMissile")
        {
            collision.gameObject.SetActive(false);

            destroyBunker.playHitSound();

            bunkerHealth--;

            if (bunkerHealth == -1)
            {
                destroyBunker.decrementNumOfBunkerPieces();
                Destroy(gameObject);
            }
            else
                bunkerSpriteRenderer.color = bunkerColors[bunkerHealth];
        }
        else if(collision.gameObject.tag == "Invader")
        {
            destroyBunker.decrementNumOfBunkerPieces();
            Destroy(gameObject);
        }
    }
}
