using System.Collections;
using UnityEngine;

public class WaitForLag : MonoBehaviour {
    public float LagTime;
    void Start() {
        StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(LagTime);
        GetComponent<Animator>().enabled = true;
        gameObject.transform.Find("Burger").gameObject.SetActive(true);
    }
}
