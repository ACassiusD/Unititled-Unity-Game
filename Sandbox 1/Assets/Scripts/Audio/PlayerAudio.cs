using UnityEngine;

//.FBX events call these functions to play noise during animations.
public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip walking;
    private AudioSource audioSource;

    private void Awake()
    {
        //audioSource.volume = 0.5f;
        audioSource = GetComponent<AudioSource>();
    }
    private void StepAudio()
    {
        audioSource.PlayOneShot(walking);
    }

}
