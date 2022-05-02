using System.Collections;
using UnityEngine;

public class OneShotSound : MonoBehaviour {
    public AudioClip[] clips;

    void Start() {
        var source = GetComponent<AudioSource>();

        source.volume *= Game.VolumeModifier;        
        if (clips != null && clips.Length > 0) {
            source.clip = clips[Random.Range(0, clips.Length)];
        }
        source.Play();
        StartCoroutine(DieLater(source.clip.length + 0.1f));
    }

    IEnumerator DieLater(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}