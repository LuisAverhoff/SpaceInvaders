using UnityEngine;

public class InvaderShoot : MonoBehaviour
{
    public GameObject enemyProjectilePrefab;
    private GameObject enemyProjectilePrefabClone;

    private int playerLayer;
    private int invaderLayer;
    private int bunkerLayer;

    public Transform shotSpawn;

    private Color bulletColor;

    private float shotTimePassed;
    private float shootTime;
    private float shootRoll;

    private readonly float SHOOT_PERCENTAGE_BOOST = 0.2f;

    private AudioSource projectileSoundSource;
    public AudioClip shootClip;

    // Use this for initialization
    void Start ()
    {
        playerLayer = LayerMask.GetMask("Player");
        invaderLayer = LayerMask.GetMask("Invader");
        bunkerLayer = LayerMask.GetMask("Bunker");
        bulletColor = GetComponent<SpriteRenderer>().color;
        projectileSoundSource = GetComponent<AudioSource>();
        enemyProjectilePrefabClone = Instantiate(enemyProjectilePrefab);
        enemyProjectilePrefabClone.transform.parent = GameObject.FindWithTag("AdvancedPoolingSystem").GetComponent<Transform>();
        enemyProjectilePrefabClone.GetComponent<SpriteRenderer>().color = bulletColor;
        enemyProjectilePrefabClone.SetActive(false);
    }
	
	// Update is called once per frame
	public void shoot(float minimumShootWaitTime, float maximumShootWaitTime, 
                      float maximumAcceptedShootRoll, float currentSightDistance, 
                      Vector3 relativeInvaderDistanceY)
    {
       if(!isInvaderInLineOfSight(relativeInvaderDistanceY))
       {
            if (isEnemyInLineOfSight(currentSightDistance, playerLayer))
                tryToShot(maximumAcceptedShootRoll + SHOOT_PERCENTAGE_BOOST);
            else if (isEnemyInLineOfSight(currentSightDistance, bunkerLayer))
                fireProjectile();
            else
                tryToShot(maximumAcceptedShootRoll);
        }

        shotTimePassed = 0.0f;
        randomizeShootTime(minimumShootWaitTime, maximumShootWaitTime);
	}

    private bool isEnemyInLineOfSight(float currentSightDistance, int layerToShoot)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, currentSightDistance, layerToShoot);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private bool isInvaderInLineOfSight(Vector3 relativeInvaderDistanceY)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + relativeInvaderDistanceY, Vector2.down, Screen.height, invaderLayer);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void tryToShot(float maximumAcceptedShootRoll)
    {
        shootRoll = Random.Range(0.0f, 1.0f);

        if (shootRoll <= maximumAcceptedShootRoll)
        {
            fireProjectile();
        }
    }

    void fireProjectile()
    {
        enemyProjectilePrefabClone.transform.position = shotSpawn.position;
        enemyProjectilePrefabClone.transform.rotation = Quaternion.identity;
        enemyProjectilePrefabClone.SetActive(true);
        projectileSoundSource.PlayOneShot(shootClip);
    }

    public void incrementTimePassed(float time)
    {
        shotTimePassed += time;
    }

    public void resetShotTime()
    {
        shotTimePassed = 0.0f;
    }

    public bool canShot()
    {
        return shotTimePassed > shootTime;
    }

    public void randomizeShootTime(float minimumShootWaitTime, float maximumShootWaitTime)
    {
        shootTime = Random.Range(minimumShootWaitTime, maximumShootWaitTime);
    }
}
