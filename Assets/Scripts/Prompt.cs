using UnityEngine;
using System.Collections;

public class Prompt : MonoBehaviour {

	public int promptHeight = 10;
	private Vector3 promptOffset;

	void Start()
	{
		GetComponentInChildren<ParticleSystem> ().Stop ();
		promptOffset = new Vector3 (0, 0, -promptHeight);
	}

	public void PlayPrompt(Vector3 pos)
	{
		transform.position = pos + promptOffset;
		if (GetComponentInChildren<ParticleSystem>().isStopped)
			GetComponentInChildren<ParticleSystem> ().Play ();
	}

	public void StopPrompt()
	{
		GetComponentInChildren<ParticleSystem> ().Stop ();
	}
}
