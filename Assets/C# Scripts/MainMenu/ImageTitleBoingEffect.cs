using UnityEngine;

public class ImageTitleBoingEffect : MonoBehaviour
{
    private AudioSource imageAudioSource;

    void Start()
    {
       imageAudioSource = GetComponent<AudioSource>();
    }

    public void playImageBoingSound()
    {
        imageAudioSource.Play();
    }
}
