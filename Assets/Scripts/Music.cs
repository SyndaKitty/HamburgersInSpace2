using UnityEngine;

public class Music : MonoBehaviour {
    public AudioClip[] Tracks;
    public float FadeTime = 1f;
    public float VolumeLevel;

    AudioSource source;
    AudioSource secondarySource;
    bool fading;
    float t;

    bool odd = true;

    void Start() {
        source = GetComponent<AudioSource>();
        secondarySource = transform.Find("Secondary").GetComponent<AudioSource>();
    }

    public void PlayTrack(int index) {
        if (odd) {
            secondarySource.clip = Tracks[index];
            secondarySource.Play();
        }
        else {
            source.clip = Tracks[index];
            source.Play();
        }
        t = 0;
        fading = true;
    }

    void Update() {
        if (!fading) return;
        t += Time.deltaTime;
        float T = Mathf.Clamp01(t / FadeTime);

        if (odd) {
            source.volume = VolumeLevel * (1 - T);
            secondarySource.volume = VolumeLevel * T;
        }
        else {
            source.volume = VolumeLevel * T;
            secondarySource.volume = VolumeLevel * (1 - T);
        }
        
        if (t > FadeTime) {
            if (odd) {
                source.Stop();
            }
            else {
                secondarySource.Stop();
            }

            fading = false;
            odd = !odd;
        }
    }
}