using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController playerController;

    private InvaderMovement [] invaderMovementScripts;
    private MysteryShipMovement mysteryShipMovement;
    private InvaderShoot[] invaderShootingScripts;

    private Vector2 invaderHorizontalVelocity;
    private Vector2 invaderVerticalVelocity;

    private AudioSource enemySoundSource;
    public AudioClip[] movementSoundEffectClips;
    
    public GameObject spawnPoint;

    public float spawnWidth;
    public float spawnHeight;
    public int EnemiesPerRow;
    public int EnemiesPerColumn;
    private int totalEnemies;
    private int totalEnemiesDead;
    private int allowedDeadEnemiesAmt;

    private readonly Dictionary<string, int> enemyScoreDict = new Dictionary<string, int>
    {
        {"Invader1(Clone)", 10},
        {"Invader2(Clone)", 20},
        {"Invader3(Clone)", 30}
    };

    private readonly string[] ENEMY_SETUP = { "Invader3", "Invader2", "Invader2", "Invader1", "Invader1" };

    private float amountToIncreaseVelX;
    private float amountToIncreaseVelY;
    private float maximumShootWaitTime;
    private readonly float minimumShootWaitTime = 3.0f;
    private float currentAcceptedShotRoll;
    private readonly float MAXIMUM_ACCEPTED_SHOT_ROLL = 0.5f;
    private float currentSightDistance;
    private float reduceShootWaitTimeAmount;
    private float amountToIncreaseShotPercentageBy;
    private float amountToIncreaseSightDistanceBy;
    private Vector3 relativeInvaderDistanceY;

    private float timePassed;

    private float movementDelay;
    private float reduceMovementDelayAmount;

    private bool isInvaderMovingDown;

    private int soundClipIndex;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        enemySoundSource = GetComponent<AudioSource>();

        spawnEnemies();
        totalEnemies = EnemiesPerRow * EnemiesPerColumn;
        allowedDeadEnemiesAmt = (int)(totalEnemies * 0.8f);

        reduceMovementDelayAmount = 1.0f / totalEnemies;
        reduceShootWaitTimeAmount = (maximumShootWaitTime - 1.0f) / totalEnemies;
        amountToIncreaseShotPercentageBy = (MAXIMUM_ACCEPTED_SHOT_ROLL - currentAcceptedShotRoll) / totalEnemies;
        amountToIncreaseSightDistanceBy = (10.0f - currentSightDistance) / totalEnemies;
        relativeInvaderDistanceY = new Vector3(0.0f, -spawnHeight, 0.0f);

        invaderMovementScripts = (InvaderMovement[])FindObjectsOfType(typeof(InvaderMovement));
        invaderShootingScripts = (InvaderShoot[])FindObjectsOfType(typeof(InvaderShoot));
        mysteryShipMovement = GameObject.FindWithTag("MysteryShip").GetComponent<MysteryShipMovement>();

        setEnemiesToInitialState();
    }

    public void spawnEnemies()
    {
        Vector3 startingPosition = spawnPoint.transform.position;

        for (int i = 0; i < EnemiesPerRow; i++)
        {
            for (int j = 0; j < EnemiesPerColumn; j++)
            {
                PoolingSystem.Instance.InstantiateAPS(ENEMY_SETUP[i], startingPosition, Quaternion.identity);
                startingPosition.x += spawnWidth;
            }

            startingPosition.y -= spawnHeight;
            startingPosition.x = spawnPoint.transform.position.x;
        }
    }

    void FixedUpdate()
    {
        timePassed += Time.fixedDeltaTime;

        if (isMysteryShipSpawnable() && totalEnemiesDead < allowedDeadEnemiesAmt)
        {
            mysteryShipMovement.spawnMysteryShip();
        }
        else
            mysteryShipMovement.decrementWaitTime();

        if (invaderCanMove())
        {
            if(!isInvaderMovingDown)
                moveAndShoot(invaderHorizontalVelocity, timePassed);
            else
            {
                moveAndShoot(invaderVerticalVelocity, timePassed);
                changeHorizontalDirAndSpeed();
                isInvaderMovingDown = false;
            }

            playMovementSoundEffect(movementSoundEffectClips[soundClipIndex]);
            moveToNextSoundClip();
            timePassed = 0.0f;
        }
    }

    private bool isMysteryShipSpawnable()
    {
        return !mysteryShipMovement.isMysteryShipMoving() && mysteryShipMovement.isMysteryShipReady();
    }

    private bool invaderCanMove()
    {
        if(playerController.isPlayerDead())
        {
            timePassed = 0.0f;
            return false;
        }

        return timePassed > movementDelay && !areAllEnemiesDead();
    }

    public bool areAllEnemiesDead()
    {
        return totalEnemiesDead == totalEnemies;
    }

    public void incrementTotalEnemiesDead()
    {
        totalEnemiesDead++;
    }

    private void moveAndShoot(Vector2 movementVector, float timePassed)
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            if(!invaderMovementScripts[i].isInvaderDead())
            {
                invaderMovementScripts[i].move(movementVector, timePassed);
                invaderShootingScripts[i].incrementTimePassed(timePassed);

                if (invaderShootingScripts[i].canShot())
                {
                    invaderShootingScripts[i].shoot(minimumShootWaitTime, maximumShootWaitTime, 
                                                    currentAcceptedShotRoll, currentSightDistance, 
                                                    relativeInvaderDistanceY);
                }
            }
        }
    }

    private void playMovementSoundEffect(AudioClip movementSoundEffectClip)
    {
        enemySoundSource.PlayOneShot(movementSoundEffectClip);
    }

    private void moveToNextSoundClip()
    {
        soundClipIndex++;
        soundClipIndex %= movementSoundEffectClips.Length;
    }

    public void setIsInvaderMovingDown(bool isInvaderMovingDown)
    {
        this.isInvaderMovingDown = isInvaderMovingDown;
    }

    private void changeHorizontalDirAndSpeed()
    {
        invaderHorizontalVelocity.x = -invaderHorizontalVelocity.x;
        amountToIncreaseVelX = -amountToIncreaseVelX;
    }

    public void reduceEnemyConstraints()
    {
        movementDelay -= reduceMovementDelayAmount;
        currentAcceptedShotRoll += amountToIncreaseShotPercentageBy;
        maximumShootWaitTime -= reduceShootWaitTimeAmount;
        invaderHorizontalVelocity.x += amountToIncreaseVelX;
        invaderVerticalVelocity.y -= amountToIncreaseVelY;
        currentSightDistance += amountToIncreaseSightDistanceBy;
    }

    public void setEnemiesToInitialState()
    {
        movementDelay = 1.0f;
        maximumShootWaitTime = 7.0f;
        currentAcceptedShotRoll = 0.3f;
        currentSightDistance = 5.0f;

        invaderHorizontalVelocity = Vector2.right * 0.1f;
        invaderVerticalVelocity = Vector2.down * 0.1f;

        amountToIncreaseVelX = (2.0f - invaderHorizontalVelocity.x) / totalEnemies;
        amountToIncreaseVelY = amountToIncreaseVelX;

        timePassed = 0.0f;
        soundClipIndex = 0;

        isInvaderMovingDown = false;

        for(int i = 0; i < totalEnemies; i++)
        {
            invaderMovementScripts[i].setInitialAnimationIndex();
            invaderMovementScripts[i].setInitialEnemyLayer();
            invaderMovementScripts[i].setInvaderDead(false);

            invaderShootingScripts[i].resetShotTime();
            invaderShootingScripts[i].randomizeShootTime(minimumShootWaitTime, maximumShootWaitTime);
        }

        mysteryShipMovement.resetWaitTime();
        mysteryShipMovement.generateWaitTime();

        totalEnemiesDead = 0;
    }

    public int getPointsFromEnemyType(string enemyType)
    {
        return enemyScoreDict[enemyType];
    }
}
