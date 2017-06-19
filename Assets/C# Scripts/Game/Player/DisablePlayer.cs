using System.Collections;
using UnityEngine;

public class DisablePlayer : MonoBehaviour
{
    private GameController gameController;
    private PlayerController playerController;

    public Sprite aliveSprite;
    private SpriteRenderer playerSpriteRenderer;

    private AudioSource deadClipSource;
    public AudioClip deadClip;

    private readonly int DEAD_LAYER = 10;

    // Use this for initialization
    void Start ()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        playerController = GetComponent<PlayerController>();
        deadClipSource = GetComponent<AudioSource>();
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyMissile")
        {
            collision.gameObject.SetActive(false);

            gameObject.layer = DEAD_LAYER;
            gameController.decrementPlayerLives();

            StartCoroutine(playDeadAnimation(2.0f));
        }
    }

    IEnumerator playDeadAnimation(float duration)
    {
        playerController.setAnimationBool("PlayerDies", true);
        playerController.setPlayerDead(true);
        deadClipSource.PlayOneShot(deadClip);
        yield return new WaitForSeconds(duration);
        
        if(gameController.doesPlayerHaveLivesLeft())
        {
            revivePlayer();
        }
        else
            gameController.playGameOverScene();
    }

    public void revivePlayer()
    {
        playerController.setStartingPosition();
        playerController.setAnimationBool("PlayerDies", false);
        playerController.setPlayerDead(false);
        playerSpriteRenderer.sprite = aliveSprite;
        playerController.setInitialPlayerLayer();
    }
}
