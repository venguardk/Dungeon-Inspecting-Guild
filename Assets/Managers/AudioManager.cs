using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //This script currently handles the audio sound effects for when level editing

    public static AudioManager instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips; //Array of audio clips for placing assets; Set in the Unity inspector
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

    public void PlayAddAssetSound(int ID) //Play the sound for placing an asset; Uses ID of asset
    {
        audioSource.clip = audioClips[ID];
        audioSource.Play();
    }

    public void PlayButtonPressSound() //Play the sound for pressing an asset button in the LevelEditor
    {
        audioSource.clip = buttonPressSound;
        audioSource.Play();
    }
}
