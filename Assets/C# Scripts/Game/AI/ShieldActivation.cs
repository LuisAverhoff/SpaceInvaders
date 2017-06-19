using UnityEngine;

public class ShieldActivation : MonoBehaviour
{
    public GameObject shieldPrefab;
    private GameObject shieldPrefabClone;

    public float waitTime;
    private float currentTime;

    void Start()
    {
        currentTime = 0.0f;
        shieldPrefabClone = Instantiate(shieldPrefab);
        shieldPrefabClone.transform.parent = GetComponent<Transform>();
        deactivateShield();
    }

    public void activateShield()
    {
        shieldPrefabClone.SetActive(true);
    }

    public void deactivateShield()
    {
        shieldPrefabClone.SetActive(false);
    }

    public void incrementTimePassed(float timePassed)
    {
        currentTime += timePassed;
    }

    public void resetShieldActivationTime()
    {
        currentTime = 0.0f;
    }
}
