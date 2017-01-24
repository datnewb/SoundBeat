using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	[SerializeField] float time;
	private float currentTime;

	[SerializeField] Text timerText;

	void Start() {
		currentTime = time;

		timerText.transform.parent.gameObject.SetActive (false);
		UpdateTimerText ();
	}

	internal void StartTimer() {
		timerText.transform.parent.gameObject.SetActive (true);
		StartCoroutine (TimerRoutine ());
	}

	IEnumerator TimerRoutine() {
		while (currentTime > 0) {
			yield return new WaitForSeconds (1);
			currentTime--;
			UpdateTimerText ();

			// If 10 seconds left, play audio
			if (currentTime == 10) {
				GetComponent<AudioSource> ().Play();
			}
		}

		if (currentTime <= 0) {
			FindObjectOfType<GameManager> ().GameEnd (EndType.TimeOut);
		} 
			
		yield return null;
	}

	void UpdateTimerText() {
		int seconds = (int)currentTime % 60;
		string strSeconds = seconds + "";
		if (seconds < 10) {
			strSeconds = "0" + seconds;
		}
		timerText.text = (int)(currentTime / 60) + ":" + strSeconds;
	}
}
