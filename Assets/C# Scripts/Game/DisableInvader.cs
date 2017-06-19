using System.Collections;
using UnityEngine;

public class DisableInvader : MonoBehaviour
{
    private GameController gameController;
    private EnemyController enemyController;
    private InvaderMovement invaderMovement;

    private AudioSource deadClipSource;
    public AudioClip deadClip;

    private readonly int DEAD_LAYER = 10;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        enemyController = GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>();
        invaderMovement = GetComponent<InvaderMovement>();
        deadClipSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerMissile")
        {
            collision.gameObject.SetActive(false);

            gameObject.layer = DEAD_LAYER;

            StartCoroutine(disableInvader(gameObject, 0.2f));
            enemyController.reduceEnemyConstraints();
            enemyController.incrementTotalEnemiesDead();

            gameController.addScore(enemyController.getPointsFromEnemyType(gameObject.name));

            if (enemyController.areAllEnemiesDead())
            {
                gameController.startNextWave();
            }
        }
    }

    IEnumerator disableInvader(GameObject objectToDisable, float duration)
    {
        invaderMovement.setAnimationBool("InvaderDies", true);
        invaderMovement.setInvaderDead(true);
        deadClipSource.PlayOneShot(deadClip);
        yield return new WaitForSeconds(duration);
        objectToDisable.SetActive(false);
    }
}
