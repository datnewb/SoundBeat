using UnityEngine;

public class FriendPointer : MonoBehaviour {

	internal AudioClip voice;

	[SerializeField]
	AudioSource audioSource;

	void OnEnable() {
		if (voice != null) {
			audioSource.PlayOneShot(voice);
		}
	}
}
