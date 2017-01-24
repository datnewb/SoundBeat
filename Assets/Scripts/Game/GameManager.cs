using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawn;
    [SerializeField] List<AudioClip> voices;

    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject winCanvas;
	[SerializeField] GameObject loseCanvas;
	[SerializeField] GameObject trampleCanvas;

    internal AudioClip friendVoice;
    private int friendVoiceIndex;

    internal GameObject[] friendSpots;

    void Start() {
        // Disable canvases except for start canvas
        winCanvas.SetActive(false);
		loseCanvas.SetActive(false);
		trampleCanvas.SetActive(false);
        startCanvas.SetActive(true);

        // Randomize which voice clip is friends'
        friendVoiceIndex = Random.Range(0, voices.Count);
        friendVoice = voices[friendVoiceIndex];

        PlayFriendVoice();
    }

    public void GameStart() {
        // Disable start screen
        startCanvas.SetActive(false);

        // Start music
        GetComponent<AudioSource>().Play();

        // Initialize friend spots
        friendSpots = GameObject.FindGameObjectsWithTag("FriendSpot");
        RandomizeFriendSpot();

        // Spawn player
        GameObject player = Instantiate(playerPrefab, playerSpawn.transform.position, Quaternion.identity);
        player.GetComponent<PlayerFocus>().friendSpots = friendSpots;

        StartCoroutine(ResetFriendSpot());

		// Start timer
		FindObjectOfType<Timer>().StartTimer();
    }
		
	public void GameEnd(EndType endType) {
        // Stop everything
        foreach(MonoBehaviour behaviour in FindObjectsOfType<MonoBehaviour>()) {
            behaviour.StopAllCoroutines();
        }
        GetComponent<AudioSource>().Stop();

        // Destroy player
        Destroy(GameObject.FindGameObjectWithTag("Player"));

		switch (endType) {
		case EndType.Win:
			Debug.Log("Objective reached!");
			winCanvas.SetActive (true);
			break;

		case EndType.TimeOut:
			Debug.Log("Objective not reached!");
			loseCanvas.SetActive (true);
			break;

		case EndType.FocusTimeOut:
			Debug.Log("Focused too long!");
			trampleCanvas.SetActive (true);
			break;
		}
    }

    void RandomizeFriendSpot() {
        List<int> finishedAreas = new List<int>();
        for (int i = 0; i < voices.Count; ++i) {
            int index = Random.Range(0, voices.Count);
            if (finishedAreas.Contains(index)) {
                --i;
                continue;
            }

            FriendArea friendArea = friendSpots[i].GetComponent<FriendArea>();
            friendArea.voice = voices[index];
            friendArea.isCurrentFriendArea = (index == friendVoiceIndex);

            finishedAreas.Add(index);
        }
    }

    IEnumerator ResetFriendSpot() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(20.0f, 35.0f));

            RandomizeFriendSpot();
        }
    }

    public void PlayFriendVoice() {
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().PlayOneShot(friendVoice);
    }

	public void Replay() {
		SceneManager.LoadScene ("Game");
	}
}
