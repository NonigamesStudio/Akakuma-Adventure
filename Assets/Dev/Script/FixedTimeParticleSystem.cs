using UnityEngine;
using System.Collections;

public class FixedTimeParticleSystem : MonoBehaviour {
	
	[SerializeField] private ParticleSystem ps;
	private bool playing = false;

	public void Restart() {
		ps.Simulate(0.0f, true, true);
		playing = true;
	}

	public void Stop() {
		playing = false;
	}

	void FixedUpdate() {
		if(playing)
			ps.Simulate(Time.fixedDeltaTime, true, false);
	}
	
}