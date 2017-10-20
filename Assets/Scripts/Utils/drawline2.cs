using UnityEngine;
using System.Collections;

public class drawline2 : MonoBehaviour {
	public Color color1 = Color.yellow;
	public Color color2 = Color.red;
	public int lenghtOfLineRenderer;


	public float distanceFactor;
	public float width;

	[Range(0.1F,1.5f)]
	public float frecuencia;

	[Range(0.1F,2.0f)]
	public float amplitud;

	[Range(0.1f,20.0f)]
	public float timeFactor;

	void Start(){
		LineRenderer lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.SetColors (color1, color2);
		lineRenderer.SetWidth (width, width);
		lineRenderer.SetVertexCount (lenghtOfLineRenderer);
		for(int i=0; i<lenghtOfLineRenderer; i++){
			Vector3 pos = new Vector3 (i*distanceFactor, 0);
			lineRenderer.SetPosition (i, pos);
				}
	}

	void Update(){
		LineRenderer lineRenderer = GetComponent<LineRenderer> ();

		for(int i=0; i<lenghtOfLineRenderer; i++){
			Vector3 pos = new Vector3 (i * distanceFactor, Mathf.Cos (i * frecuencia  + Time.time * timeFactor) * amplitud, 0);
			lineRenderer.SetPosition (i, pos);
		}

	}
}


