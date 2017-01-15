using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawn;
    [SerializeField] List<AudioClip> voices;

    [SerializeField] GameObject startCanvas;
    [SerializeField]  GameObject winCanvas;

    internal AudioClip friendVoice;
    private int friendVoiceIndex;

    internal GameObject[] friendSpots;

    void Start() {
        // Disable canvases except for start canvas
        winCanvas.SetActive(false);
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
    }

    public void GameEnd() {
        Debug.Log("Objective reached!");

        // Stop everything
        foreach(MonoBehaviour behaviour in FindObjectsOfType<MonoBehaviour>()) {
            behaviour.StopAllCoroutines();
        }
        GetComponent<AudioSource>().Stop();

        // Destroy player
        Destroy(GameObject.FindGameObjectWithTag("Player"));

        winCanvas.SetActive(true);

        StartCoroutine(TimerBackToMainMenu());
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

    IEnumerator TimerBackToMainMenu() {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("Main Menu");

        yield return null;
    }
}
