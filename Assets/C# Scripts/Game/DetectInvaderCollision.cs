using UnityEngine;

public class DetectInvaderCollision : MonoBehaviour
{
    private GameController gameController;
    private PlayerController playerController;
    private EnemyController enemyController;

	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        enemyController = GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Invader")
        {
            if(gameObject.name == "GameOverWall")
            {
                playerController.setPlayerDead(true);
                gameController.playGameOverScene();
            }
            else
                enemyController.setIsInvaderMovingDown(true);
        }
    }
}
