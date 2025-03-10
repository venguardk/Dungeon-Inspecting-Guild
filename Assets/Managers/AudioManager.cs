using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioClip buttonPressSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayAddAssetSound(int ID)
    {
        audioSource.clip = audioClips[ID];
        audioSource.Play();
    }

    public void PlayButtonPressSound()
    {
        audioSource.clip = buttonPressSound;
        audioSource.Play();
    }
}
