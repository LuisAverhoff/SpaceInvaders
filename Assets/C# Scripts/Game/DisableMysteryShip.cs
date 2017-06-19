using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisableMysteryShip : MonoBehaviour
{
    private GameController gameController;
    private MysteryShipMovement mysteryShipMovement;

    public Text mysteryShipPoints;

    public AudioClip deadClip;

    private readonly int MYSTERYSHIP_SCORE_MULTIPLIER = 50;
    private readonly int ALIVE_ENEMY_LAYER = 9;
    private readonly int DEAD_LAYER = 10;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        mysteryShipMovement = GetComponent<MysteryShipMovement>();
        mysteryShipPoints.text = "";
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerMissile")
        {
            collision.gameObject.SetActive(false);

            gameObject.layer = DEAD_LAYER;

            int pointsEarned = Random.Range(1, 7) * MYSTERYSHIP_SCORE_MULTIPLIER;

            StartCoroutine(disableMysteryShip(1.0f, pointsEarned));

            gameController.addScore(pointsEarned);
        }
    }

    IEnumerator disableMysteryShip(float duration, int pointsToDisplay)
    {
        mysteryShipMovement.setIsMysteryShipDead(true);
        mysteryShipMovement.playMysteryShipSound(deadClip, false);
        mysteryShipPoints.rectTransform.anchoredPosition = GetTextScreenPosition();
        StartCoroutine(blinkText(pointsToDisplay));
        yield return new WaitForSeconds(duration);
        mysteryShipMovement.setIsMysteryShipDead(false);
        gameObject.layer = ALIVE_ENEMY_LAYER;
    }

    public Vector3 GetTextScreenPosition()
    {
        Vector3 pos;
        Canvas canvas = mysteryShipPoints.GetComponentInParent<Canvas>();
        float width = canvas.GetComponent<RectTransform>().sizeDelta.x;
        float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
        float x = Camera.main.WorldToScreenPoint(transform.position).x / Screen.width;
        float y = Camera.main.WorldToScreenPoint(transform.position).y / Screen.height;
        pos = new Vector3(width * x - width / 2, y * height - height / 2);
        return pos;
    }

    IEnumerator blinkText(int pointsToDisplay)
    {
        int numTimesBlinkedText = 0;

        while(numTimesBlinkedText < 3)
        {
            mysteryShipPoints.text = pointsToDisplay.ToString();
            yield return new WaitForSeconds(.125f);
            mysteryShipPoints.text = "";
            yield return new WaitForSeconds(.125f);
            numTimesBlinkedText++;
        }
    }
}
