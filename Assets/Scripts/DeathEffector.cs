using UnityEngine;
using System.Collections;

public class DeathEffector : MonoBehaviour {

	public float explosionTime = 0.2f;
	public float maxRotation = 500;
	public Rigidbody2D headRB2D;
	public Rigidbody2D frontHandRB2D;
	public Rigidbody2D backHandRB2D;
	public Rigidbody2D frontFootRB2D;
	public Rigidbody2D backFootRB2D;

	// Use this for initialization
	void Start () {
		Invoke("Deactivate", explosionTime);

		PartsRotation();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Deactivate () {
		gameObject.SetActive(false);
	}

	void PartsRotation() {
		float rand = Random.Range(-maxRotation, maxRotation);
		headRB2D.angularVelocity = rand;

		rand = Random.Range(-maxRotation, maxRotation);
		frontHandRB2D.angularVelocity = rand;

		rand = Random.Range(-maxRotation, maxRotation);
		backHandRB2D.angularVelocity = rand;

		rand = Random.Range(-maxRotation, maxRotation);
		frontFootRB2D.angularVelocity = rand;

		rand = Random.Range(-maxRotation, maxRotation);
		backFootRB2D.angularVelocity = rand;
	}
}
