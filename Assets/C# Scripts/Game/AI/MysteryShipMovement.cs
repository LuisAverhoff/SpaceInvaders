using System.Collections;
using UnityEngine;

public class MysteryShipMovement : MonoBehaviour
{
    private Vector3 leftSide;
    private Vector3 rightSide;

    private Rigidbody2D mysteryShipRigidBody;

    public float minWaitTime;
    public float maxWaitTime;
    private float currentWaitTime;

    private bool mysteryShipDead;
    private bool mysteryShipMoving;

    private int directionModifier;
    private float speed = 5.0f;

    public float frequency;
    public float amplitude;

    private AudioSource mysteryShipAudioSource;
    public AudioClip lowPitchMovementClip;

	// Use this for initialization
	void Start ()
    {
        mysteryShipMoving = false;
        mysteryShipDead = false;
        mysteryShipRigidBody = GetComponent<Rigidbody2D>();
        mysteryShipAudioSource = GetComponent<AudioSource>();
        setLeftAndRightPosition();
        setStartingPosition();
	}

    // Update is called once per frame
    public void spawnMysteryShip()
    {
        StartCoroutine(move());
    }

    public void generateWaitTime()
    {
        currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
    }

    public void decrementWaitTime()
    {
        currentWaitTime -= Time.fixedDeltaTime;
    }

    public void resetWaitTime()
    {
        currentWaitTime = 0.0f;
    }

    public bool isMysteryShipReady()
    {
        return (currentWaitTime >= 0.0f) ? false : true;
    }

    public bool isMysteryShipMoving()
    {
        return mysteryShipMoving;
    }

    IEnumerator move()
    {
        mysteryShipMoving = true;

        playMysteryShipSound(lowPitchMovementClip, true);

        while(!isMysteryShipVisible())
        {
            moveSinusoidal();
            yield return new WaitForFixedUpdate();
        }

        while (!mysteryShipDead && isMysteryShipVisible())
        {
            moveSinusoidal();
            yield return new WaitForFixedUpdate();
        }

        if(!mysteryShipDead)
        {
            mysteryShipAudioSource.Stop();
        }

        generateWaitTime();
        setStartingPosition();

        mysteryShipMoving = false;
    }

    private void moveSinusoidal()
    {
        float positionX = transform.right.x * Time.fixedDeltaTime * speed * directionModifier;
        float positionY = transform.up.y * amplitude * Mathf.Sin(Time.time * frequency);
        Vector2 newPosition = new Vector2(positionX, positionY);
        mysteryShipRigidBody.MovePosition(mysteryShipRigidBody.position + newPosition);
    }

    public void playMysteryShipSound(AudioClip soundClip, bool isLooping)
    {
        mysteryShipAudioSource.clip = soundClip;
        mysteryShipAudioSource.loop = isLooping;
        mysteryShipAudioSource.Play();
    }

    private bool isMysteryShipVisible()
    {
        return transform.position.x > leftSide.x && transform.position.x < rightSide.x;
    }

    public void setIsMysteryShipDead(bool mysteryShipDead)
    {
        this.mysteryShipDead = mysteryShipDead;
    }

    private void setLeftAndRightPosition()
    {
        rightSide = transform.position;

        float planeDistance = transform.position.z - Camera.main.transform.position.z;
        float halfEnemySizeX = GetComponent<SpriteRenderer>().bounds.size.x / 2;

        float leftOffset = transform.position.x - Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.9f, planeDistance)).x;
        leftOffset += halfEnemySizeX;

        Vector3 offset = new Vector3(leftOffset, 0, 0);
        leftSide = transform.position - offset;
    }

    public void setStartingPosition()
    {
        directionModifier = (Random.value < 0.5) ? 1 : -1;
        Vector2 startingPosition = (directionModifier == 1) ? leftSide : rightSide;
        mysteryShipRigidBody.MovePosition(startingPosition);
    }
}
