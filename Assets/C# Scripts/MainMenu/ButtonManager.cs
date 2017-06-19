using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private AudioSource buttonAudioSource;
    public AudioClip [] buttonSoundClips;

    public AnimationClip titleBounceButtonsFadeInClip;

    private readonly string MAIN_SCENE = "Game";

    private Button playButton;

    void Start()
    {
        buttonAudioSource = GetComponent<AudioSource>();
        playButton = GameObject.FindWithTag("PlayGameButton").GetComponent<Button>();
    }

    public void playGame()
    {
        StartCoroutine(playEffectsAndStartGame());
    }

    IEnumerator playEffectsAndStartGame()
    {
        buttonAudioSource.PlayOneShot(buttonSoundClips[0]);
        GameObject.FindWithTag("MainMenuUI").GetComponent<Animator>().SetTrigger("PlayButtonClicked"); ;
        yield return new WaitForSeconds(titleBounceButtonsFadeInClip.length);
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public void playHighlightSound()
    {
        if(playButton.IsInteractable())
            buttonAudioSource.PlayOneShot(buttonSoundClips[1]);
    }
}
