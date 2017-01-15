using System.Collections;
using UnityEngine;

public class ZUpdate : MonoBehaviour {

    void Start() {
        StartCoroutine(ZUpdateRoutine());
    }

    IEnumerator ZUpdateRoutine() {
        while (true) {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            yield return null;
        }
    }
}
