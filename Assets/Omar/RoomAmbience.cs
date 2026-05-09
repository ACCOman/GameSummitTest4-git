using UnityEngine;

public class RoomAmbience : MonoBehaviour
{
    public AudioSource ambienceAudio;

    private static AudioSource currentPlaying;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentPlaying != null && currentPlaying != ambienceAudio)
            {
                currentPlaying.Stop();
            }
            ambienceAudio.Play();
            currentPlaying = ambienceAudio;
        }
    }
}