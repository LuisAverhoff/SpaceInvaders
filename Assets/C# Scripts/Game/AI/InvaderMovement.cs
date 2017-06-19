using UnityEngine;

public class InvaderMovement : MonoBehaviour
{
    private Rigidbody2D enemyRigidbody2D;

    private Animator invaderAnimator;

    private readonly string[] INVADER_TRIGGERS = {"GoToMovement2", "GoToMovement1"};

    private int animationIndex;

    private readonly int ALIVE_ENEMY_LAYER = 9;

    private bool invaderDead;

    void Start()
    {
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
        invaderAnimator = GetComponent<Animator>();
    }

    public void move(Vector2 movementVector, float timePassed)
    {
        setAnimationTrigger(INVADER_TRIGGERS[animationIndex]);
        moveToNextAnimationIndex();
        enemyRigidbody2D.MovePosition(enemyRigidbody2D.position + movementVector * timePassed);
    }

    public bool isInvaderDead()
    {
        return invaderDead;
    }

    public void setInvaderDead(bool invaderDead)
    {
        this.invaderDead = invaderDead;
    }

    public void setAnimationTrigger(string animationTrigger)
    {
        invaderAnimator.SetTrigger(animationTrigger);
    }

    public void setAnimationBool(string condition, bool value)
    {
        invaderAnimator.SetBool(condition, value);
    }

    private void moveToNextAnimationIndex()
    {
        animationIndex++;
        animationIndex %= INVADER_TRIGGERS.Length;
    }

    public void setInitialAnimationIndex()
    {
        animationIndex = 0;
    }

    public void setInitialEnemyLayer()
    {
        gameObject.layer = ALIVE_ENEMY_LAYER;
    }
}
