using UnityEngine;

public class SpeedRemove : MonoBehaviour {

	[SerializeField]
	float decelerationForce;

	Rigidbody2D movebody;

	void Start() {
		movebody = GetComponent<Rigidbody2D>();
	}

	void Update () {
		float currentSpeed = movebody.velocity.magnitude;
		if (currentSpeed < 0) {
			movebody.velocity = Vector2.zero;
		}
		else {
			movebody.AddForce(movebody.velocity.normalized * -decelerationForce, ForceMode2D.Force);
		}
	}
}
