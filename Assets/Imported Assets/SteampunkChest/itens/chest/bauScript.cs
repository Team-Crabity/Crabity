using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bauScript : MonoBehaviour {


	public string player = "Player";
	public float velocity = 10.0f;

	public GameObject target;
	public List<GameObject> Contents;

	bool abrir = false;
	GameObject alvo;
	bool cheio = true;

	//rotatação max  -60 -900 -900
	//0, -720, -720

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (abrir) {
			if (this.transform.rotation.x > -0.9) {
				this.transform.Rotate (new Vector3 (-velocity * Time.deltaTime * 2, 0.0f, 0.0f));
				if(this.transform.rotation.x < -0.45 && cheio == true){
					liberar ();
					cheio = false;
				}
			} 
		} else {
			if (this.transform.rotation.x < 0) {
				this.transform.Rotate (new Vector3 (velocity * Time.deltaTime * 2 , 0.0f, 0.0f));

			} 
		}
	
	}


	void OnTriggerStay(Collider other) { 

		if (alvo == null && abrir == false) {
			Vector3 pos = this.transform.position;
			pos.y += 0.5f;
			pos.z += 0.25f;
			alvo = Instantiate (target, pos, Quaternion.identity) as GameObject;
		}


		if (Input.GetKeyDown (KeyCode.E)) {
				abrir = true;
				Destroy (alvo.gameObject);
		}



	}

	void OnTriggerExit(Collider other) {

		Destroy (alvo.gameObject);
		
		abrir = false;
	}


	public void liberar(){

		Vector3 pos = this.transform.position;
		//pos.x = 0;
		pos.y += 0.4f;
		pos.z += 0.4f;

		for (int i = 0; i < Contents.Count; i++) {
			GameObject premio = Instantiate (Contents[i], pos, Quaternion.identity) as GameObject;

			Rigidbody rb = premio.GetComponent<Rigidbody>();
			if (rb == null) {
				rb = premio.AddComponent<Rigidbody> ();
			}
			rb.AddForce (new Vector3(0,1,1));
		}



	}
}
