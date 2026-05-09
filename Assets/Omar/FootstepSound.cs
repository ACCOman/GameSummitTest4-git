using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepAudio;
    public float stepInterval = 0.1f;

    private float timer = 0f;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float moved = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if (moved > 0.0001f)
        {
            timer += Time.deltaTime;
            if (timer >= stepInterval)
            {
                footstepAudio.Play();
                timer = 0f;
                Debug.Log("Playing sound!");
            }
        }
        else
        {
            timer = 0f;
        }
    }
}