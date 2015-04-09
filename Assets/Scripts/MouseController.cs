using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {
	public float jetpackForce = 75.0f;
	public float forwardMovementSpeed = 3.0f;

	public Transform groundCheckTransfrom;

	public LayerMask grounCHeckLayerMask;

	public ParticleSystem jetpack;

	public Texture2D coinIconTexture;

	public GUIStyle restartButtonStyle;

	public AudioClip coinCollectSound;

	public AudioClip jetpackAudio;
	public AudioClip footstepsAudio;


	private bool grounded;

	private bool dead = false;

	private uint coins = 0;
	Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
	}
	void OnGUI()
	{
		DisplayCoinsCount();
		DisplayRestartButton();
	}
	void FixedUpdate()
	{
		Vector2 newVelocity = rigidbody2D.velocity;
		newVelocity.x = forwardMovementSpeed;
		rigidbody2D.velocity = newVelocity;
		bool jetpackActive = Input.GetButton("Fire1");
		jetpackActive = jetpackActive & !dead;
		if(jetpackActive)
		{
			rigidbody2D.AddForce(new Vector2(0,jetpackForce));
		}
		if(!dead)
		{
			Vector2 newVelcoity = rigidbody2D.velocity;
			newVelcoity.x = forwardMovementSpeed;
			rigidbody2D.velocity = newVelcoity;
		}
		UpdateGroundStatus();
		AdjustJetpack(jetpackActive);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Coins"))
			CollectCoin(collider);
		else
			HitByLaser(collider);
	}
	
	void HitByLaser(Collider2D laserCollider)
	{
		if (!dead)
			laserCollider.gameObject.audio.Play();
		dead = true;
		animator.SetBool("dead",true);
	}

	void UpdateGroundStatus()
	{
		grounded = Physics2D.OverlapCircle(groundCheckTransfrom.position,0.1f,grounCHeckLayerMask);
		animator.SetBool("grounded",grounded);
	}

	void AdjustJetpack(bool jecpackActive)
	{
		jetpack.enableEmission = !grounded;
		jetpack.emissionRate = jecpackActive ? 300.0f : 75.0f;
	}

	void AdjustFootstepsAndJetpackSound(bool jetpackActive)
	{

	}

	void CollectCoin(Collider2D coinCollider)
	{
		coins++;
		AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
		Destroy(coinCollider.gameObject);
	}

	void DisplayCoinsCount()
	{
		Rect coinIconRect = new Rect(10, 10, 32, 32);
		GUI.DrawTexture(coinIconRect, coinIconTexture);                         
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.yellow;
		
		Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
		GUI.Label(labelRect, coins.ToString(), style);
	}

	void DisplayRestartButton()
	{
		if (dead && grounded)
		{
			Rect buttonRect = new Rect(Screen.width * 0.35f, Screen.height * 0.45f, Screen.width * 0.30f, Screen.height * 0.1f);
			if (GUI.Button(buttonRect, "Tap to restart!"))
			{
				Application.LoadLevel (Application.loadedLevelName);
			};
		}
	}
}
