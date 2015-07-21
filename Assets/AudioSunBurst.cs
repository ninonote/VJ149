// Sunburst effects.
// By Keijiro Takahashi, 2013
// https://github.com/keijiro/unity-sunburst-mesh-fx
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
[RequireComponent (typeof (AudioSource))]
public class AudioSunBurst : MonoBehaviour
{
	#region Public variables
	public int beamCount = 100;
	[Range(0.01f, 0.5f)]
	public float beamWidth = 0.1f;
	[Range(0.1f, 10.0f)]
	public float speed = 0.4f;
	[Range(1.0f, 10.0f)]
	public float scalePower = 1.0f;
	#endregion
	
	#region Beam vectors
	Vector3[] beamDir;
	Vector3[] beamExt;
	#endregion
	
	#region Mesh data
	Mesh mesh;
	Vector3[] vertices;
	#endregion
	
	#region Animation parameters
	const float indexToNoise = 0.77f;
	float time;
	#endregion

	private AudioSource audio;
	
	#region Private functions
	void ResetBeams ()
	{
		// Allocate arrays.
		beamDir = new Vector3[beamCount];
		beamExt = new Vector3[beamCount];
		vertices = new Vector3[beamCount * 3];
		var normals = new Vector3[beamCount * 3];
		
		// Initialize the beam vectors.
		var normalIndex = 0;
		for (var i = 0; i < beamCount; i++) {
			// Make a beam in a completely random way.
			var dir = Random.onUnitSphere;
			//var v = new Vector3(0.1f, 0.1f, 0.1f);
			var ext = Random.onUnitSphere;
			Debug.Log (dir);
			Debug.Log (ext);
			beamDir [i] = dir;
			beamExt [i] = ext;
			
			// Use a slightly modified vector on the first vertex to make a gradation.
			var normal = Vector3.Cross (dir, ext).normalized;
			normals [normalIndex++] = Vector3.Lerp (dir, normal, 0.5f).normalized;
			normals [normalIndex++] = normal;
			normals [normalIndex++] = normal;
		}
		
		// Initialize the triangle set.
		var indices = new int[beamCount * 3];
		for (var i = 0; i < indices.Length; i++) {
			indices [i] = i;
		}
		
		// Initialize the mesh.
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.triangles = indices;
	}
	
	void UpdateVertices ()
	{
		var range = 1024;
		var spectrum = audio.GetSpectrumData(range, 0, FFTWindow.BlackmanHarris);
		//while ( j < 1023 ) {
		for (var i = 1; i < range-1; i++) {
			Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red, 2, false);
			Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
		}

		var vertexIndex = 0;
		for (var i = 0; i < beamCount; i++) {
			// Use 2D Perlin noise to animate the beam.
			var scale = Mathf.Pow (Mathf.PerlinNoise (time, i * indexToNoise), scalePower);
			
			// Never modify the first vertex.
			vertexIndex++;
			
			// Update the 2nd and 3rd vertices.
			var tip = beamDir [i] * scale;
			var ext = beamExt [i] * beamWidth * scale;
			vertices [vertexIndex++] = tip - ext; 
			//vertices [vertexIndex++] = tip + ext;
			vertices [vertexIndex++] = tip + ext;
		}
	}
	#endregion
	
	#region Monobehaviour functions
	void Awake ()
	{
		// Initialize the mesh instance.
		mesh = new Mesh ();
		mesh.MarkDynamic (); //Call this when you continually update mesh vertices.
		GetComponent<MeshFilter> ().sharedMesh = mesh;
		
		// Initialize the beam array.
		ResetBeams ();

	}

	void Start() 
	{
		audio = GetComponent<AudioSource>();
	}
	
	void Update ()
	{
		// Reset the beam array if the number was changed.
		if (beamCount != beamDir.Length) {
			ResetBeams ();
		}
		
		// Do animation.
		UpdateVertices ();
		
		// Update the vertex array.
		mesh.vertices = vertices;
		
		// Advance the time count.
		time += Time.deltaTime * speed;
	}
	#endregion
}