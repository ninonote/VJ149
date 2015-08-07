using UnityEngine;
using System.Collections;
using System.Linq;

// AudioSource wrapper for sound visualization.
[RequireComponent (typeof (AudioSource))]
public class AudioReceiver : MonoBehaviour {

	private AudioSource audio;
	private float[] waveData_ = new float[1024];

	public float sensitivity = 100;
	public float loudness = 0;
	private float lastLoudness = 0;
	public string teststr = "hello dear";

	// Use this for initialization
	void Awake () {
		audio = GetComponent<AudioSource>();
		audio.clip = Microphone.Start(null, true, 10, 44100);
		audio.loop = true;
		audio.mute = true;
		while (Microphone.GetPosition(null) <= 0) {}
		audio.Play();
	}

	void Start() {

	}

	// Update is called once per frame
	void Update () {

		lastLoudness = loudness;
		loudness = lastLoudness * 0.8f + GetAveragedVolume() * sensitivity * 0.2f;
		/*audio.GetOutputData(waveData_, 1);
		Debug.Log (waveData_);

		var volume = waveData_.Select(x => x*x).Sum() / waveData_.Length;
		transform.localScale = Vector3.one * volume;

		Debug.Log (volume);*/

		//float volume = GetAveragedVolume ();
		//Debug.Log (volume);
		//Debug.DrawLine (Vector3.zero, new Vector3 (1, 0, 0), Color.red);
	}

	float GetAveragedVolume()
	{ 
		float[] data = new float[256];
		float a = 0;
		audio.GetOutputData(data, 0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/256;
	}
}
