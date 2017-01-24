using UnityEngine;

public class PlayerMove : MonoBehaviour {

	[SerializeField]
	float maxSpeed;

    internal bool isMoving = false;

	Vector2 currentDirection;
	Rigidbody2D movebody;

	void Start() {
		movebody = GetComponent<Rigidbody2D>();
	}

	void Update () {
		Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		UpdateDirection(inputDirection);
		Move();
	}

	void UpdateDirection(Vector2 direction) {
		currentDirection = direction;
		currentDirection.Normalize();

        isMoving = currentDirection.magnitude != 0;
		GetComponent<Animator> ().SetBool ("IsWalking", isMoving);
	}

	void Move() {
		Vector2 newPosition = movebody.position + currentDirection * maxSpeed * Time.deltaTime;
        movebody.MovePosition(newPosition);
	}
}
