using UnityEngine;

public class FriendArea : MonoBehaviour {

	internal AudioClip voice;

	internal bool isCurrentFriendArea = false;
	private bool isPreviousFriendArea = false;

	SpriteRenderer[] friendSprites;

	void Start() {
		friendSprites = GetComponentsInChildren<SpriteRenderer>();
		ShowSprites(isCurrentFriendArea);
	}

	void Update() {
		if (isCurrentFriendArea != isPreviousFriendArea) {
			ShowSprites(isCurrentFriendArea);
			isPreviousFriendArea = isCurrentFriendArea;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player" && isCurrentFriendArea) {
			FindObjectOfType<GameManager>().GameEnd();
		}
	}

	void ShowSprites(bool shouldEnable) {
		foreach(SpriteRenderer friendSprite in friendSprites) {
			friendSprite.enabled = shouldEnable;
		}
	}

}
