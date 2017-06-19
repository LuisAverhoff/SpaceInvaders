using UnityEngine;

public class DestroyBunker : MonoBehaviour
{
    private int numOfBunkerPieces;

    private AudioSource bunkerAudioSource;

    // Use this for initialization
    void Start ()
    {
        numOfBunkerPieces = transform.childCount;
        bunkerAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(numOfBunkerPieces == 0)
        {
            Destroy(gameObject);
        }
    }

    public void decrementNumOfBunkerPieces()
    {
        numOfBunkerPieces--;
    }

    public void playHitSound()
    {
        bunkerAudioSource.Play();
    }
}
