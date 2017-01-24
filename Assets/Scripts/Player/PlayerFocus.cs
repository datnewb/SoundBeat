using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerFocus : MonoBehaviour {

	[SerializeField]
	private GameObject friendPointer;
	[SerializeField]
	private Transform friendPointerTransform;

	[SerializeField] float focusLimit;

	[SerializeField]
	List<AudioClip> voices;

	[SerializeField] float startDelay;
	[SerializeField] float pointerShowInterval;

	PlayerMove movement;

	internal bool isFocusing = false;

	internal GameObject[] friendSpots;
	List<GameObject> friendPointers;

	void Start() {
		movement = GetComponent<PlayerMove>();

		// Create UI pointers for possible friend spots
		friendPointers = new List<GameObject>();
		for(int index = 0; index < friendSpots.Length; index++) {
			friendPointers.Add(Instantiate(friendPointer, friendPointerTransform.position + new Vector3(0, 0, -9), Quaternion.identity, friendPointerTransform));
			try {
				friendPointers[index].GetComponent<FriendPointer>().voice = voices[index];
			}
			catch { }
			friendPointers[index].SetActive(false);
		}
	}

	void Update () {
		Focus();
	}

	void Focus() {
		// Trigger check. Focus when not moving, stop focus when moving
		if (isFocusing == movement.isMoving) {
			// Stop focusing
			if (movement.isMoving) {
				Debug.Log("Stopped focusing.");

				StopAllCoroutines ();

				for (int index = 0; index < friendSpots.Length; index++) {
					friendPointers[index].SetActive(false);
				}

				FindObjectOfType<GameManager> ().GetComponent<AudioSource> ().volume = 1.0f;
			}
			// Start focusing
			else {
				Debug.Log("Started focusing.");

				Vector3 origin = friendPointers[0].transform.position;
				origin.x = friendPointerTransform.position.x;
				origin.y = friendPointerTransform.position.y;
				for(int index = 0; index < friendSpots.Length; index++) {
					Vector3 lookPos = friendSpots[index].transform.position;
					lookPos.z = origin.z;
					friendPointers[index].transform.LookAt(lookPos);
					
					// fix arrow angles
					friendPointers[index].transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 90, 90);
				}

				StartCoroutine(ShowPointers());

				FindObjectOfType<GameManager> ().GetComponent<AudioSource> ().volume = 0.5f;
			}

			isFocusing = !movement.isMoving;
		}
	}

	IEnumerator ShowPointers() {
		yield return new WaitForSeconds(startDelay);

		int currentCount = 0;
		List<int> finishedPointers = new List<int>();
		while (currentCount < voices.Count) {
			int pointerIndexToShow = Random.Range(0, friendPointers.Count);
			if (finishedPointers.Contains(pointerIndexToShow))
				continue;

			friendPointers[pointerIndexToShow].GetComponent<FriendPointer>().voice = friendSpots[pointerIndexToShow].GetComponent<FriendArea>().voice;
			friendPointers[pointerIndexToShow].SetActive(true);
			finishedPointers.Add(pointerIndexToShow);

			currentCount++;
			yield return new WaitForSeconds(pointerShowInterval);
		}
	}

	IEnumerator FocusTimer() {
		float currentFocusTime = focusLimit;

		while (currentFocusTime > 0) {
			yield return new WaitForSeconds (1);

			currentFocusTime--;
		}

		if (currentFocusTime <= 0) {
			FindObjectOfType<GameManager> ().GameEnd (EndType.FocusTimeOut);
		}

		yield return null;
	}
}
