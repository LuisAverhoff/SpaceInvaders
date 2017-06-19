using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly Vector2 MAX_VELOCITY = Vector2.right * 8.0f;

    private Animator playerAnimator;

    private Rigidbody2D playerRigidbody2D;

    private Vector2 startingPosition;

    public GameObject projectilePrefab;
    private GameObject projectilePrefabClone;

    public Transform shotSpawn;

    private AudioSource projectileSoundSource;
    public AudioClip shootClip;

    private float leftBorder;
    private float rightBorder;

    private readonly int ALIVE_PLAYER_LAYER = 8;

    private bool playerDead;

    private float playerInput;

    void Start()
    {
        setPlayerDead(false);
        playerInput = 0;
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        startingPosition = playerRigidbody2D.position;
        playerAnimator = GetComponent<Animator>();
        projectileSoundSource = GetComponent<AudioSource>();
        projectilePrefabClone = Instantiate(projectilePrefab);
        projectilePrefabClone.SetActive(false);
        setRightAndLeftBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput = Input.GetAxis("Horizontal");

        if ((Input.GetButton("Fire1") || Input.GetMouseButton(0)) && !isProjectileActive() && !playerDead)
        {
            fireProjectile();
        }
    }

    void FixedUpdate()
    {
       if (playerInput != 0 && !playerDead)
       {
          clampPlayerMovement();
          playerRigidbody2D.MovePosition(playerRigidbody2D.position + MAX_VELOCITY * playerInput * Time.fixedDeltaTime);
          playerInput = 0;
       }
    }

    private bool isProjectileActive()
    {
        return projectilePrefabClone.activeInHierarchy;
    }

    public bool isPlayerDead()
    {
        return playerDead;
    }

    public void setPlayerDead(bool playerDead)
    {
        this.playerDead = playerDead;
    }

    public void setAnimationBool(string condition, bool value)
    {
        playerAnimator.SetBool(condition, value);
    }

    public void setInitialPlayerLayer()
    {
        gameObject.layer = ALIVE_PLAYER_LAYER;
    }

    public void setStartingPosition()
    {
        playerRigidbody2D.MovePosition(startingPosition);
    }

    private void clampPlayerMovement()
    {
        Vector2 position = playerRigidbody2D.position;
        position.x = Mathf.Clamp(position.x, leftBorder, rightBorder);
        playerRigidbody2D.position = position;
    }

    private void fireProjectile()
    {
        projectilePrefabClone.transform.position = shotSpawn.position;
        projectilePrefabClone.transform.rotation = Quaternion.identity;
        projectilePrefabClone.SetActive(true);
        projectileSoundSource.PlayOneShot(shootClip);
    }

    private void setRightAndLeftBoundaries()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        float halfPlayerSizeX = (GetComponent<SpriteRenderer>().bounds.size.x / 2) + 0.2f;
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x + halfPlayerSizeX;
        rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x - halfPlayerSizeX;
    }
}