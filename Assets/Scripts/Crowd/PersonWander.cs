using System.Collections;
using UnityEngine;

public class PersonWander : MonoBehaviour {

	private static float minWanderTime = 0.1f;
	private static float maxWanderTime = 0.7f;

	private static float minMoveSpeed = 1.0f;
	private static float maxMoveSpeed = 2.0f;

	private static float minWaitTime = 3.0f;
	private static float maxWaitTime = 7.0f;
	private bool isWaiting;

	private float curWanderTime = 0;
	private float curMoveSpeed = 0;
	private float curWaitTime = 0;

	private Vector2 curMoveDir;

	private Rigidbody2D moveBody;

	void Start() {
		isWaiting = Random.Range(0, 2) == 0;
		curMoveDir = Vector2.zero;

		moveBody = GetComponent<Rigidbody2D>();
        moveBody.isKinematic = true;

		StartCoroutine(Wander());
	}

	void Update() {
		if (!isWaiting) {
			moveBody.MovePosition(moveBody.position + curMoveDir * curMoveSpeed * Time.deltaTime);
		}
	}

	IEnumerator Wander() {
		while(true) {
			if (isWaiting) {
				curWaitTime = Random.Range(minWaitTime, maxWaitTime);
				yield return new WaitForSeconds(curWaitTime);
			}
			else {
				curWanderTime = Random.Range(minWanderTime, maxWanderTime);
				curMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

				switch(Random.Range(0, 4)) {
					case 0:
						curMoveDir = Vector2.up;
						break;
					case 1:
						curMoveDir =  Vector2.down;
						break;
					case 2:
						curMoveDir = Vector2.left;
						break;
					case 3:
						curMoveDir = Vector2.right;
						break;
				}

				yield return new WaitForSeconds(curWanderTime);
			}

			isWaiting = !isWaiting;
		}
	}
}
