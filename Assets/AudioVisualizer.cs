using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class AudioVisualizer : MonoBehaviour {

	private AudioSource audio;
	public LineRenderer lr;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		lr = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*audio.GetOutputData(waveData_, 1);
		Debug.Log (waveData_);

		var volume = waveData_.Select(x => x*x).Sum() / waveData_.Length;
		transform.localScale = Vector3.one * volume;

		Debug.Log (volume);*/
		
		float volume = GetAveragedVolume ();
		Debug.Log (volume);
		//Debug.DrawLine (Vector3.zero, new Vector3 (1, 0, 0), Color.red);
		float[] data = new float[256];
		float a = 0;
		audio.GetOutputData(data, 0);
		int count = 256;
		lr.SetVertexCount(count);
		for(int k=0; k<count; k++) {
			lr.SetPosition(k, new Vector3(-200 + 400/count*k, 200 * data[k], 200));
		}

		var spectrum = audio.GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
		var i = 1;
		while ( i < 1023 ) {
			Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red, 2, false);
			Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
			i++;
		}

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
