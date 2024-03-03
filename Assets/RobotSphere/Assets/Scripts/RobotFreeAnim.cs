using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFreeAnim : MonoBehaviour
{

	Vector3 rot = Vector3.zero;
	float rotSpeed = 40f;
	Animator anim;

	// Use this for initialization
	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		gameObject.transform.eulerAngles = rot;
	}

	// Update is called once per frame
	void Update()
	{
		CheckKey();
		gameObject.transform.eulerAngles = rot;
	}

	void CheckKey()
	{
		// Walk
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
		{
			anim.SetBool("Walk_Anim", true);

			Vector3 cameraForward = Camera.main.transform.forward;
			Vector3 cameraRight = Camera.main.transform.right;

			cameraForward.y = 0;
			cameraRight.y = 0;

			cameraForward.Normalize();
			cameraRight.Normalize();

			if (Input.GetKeyDown(KeyCode.A))
			{
				// Rotate to face left relative to the cams direction
				rot = Quaternion.LookRotation(-cameraRight).eulerAngles;
				gameObject.transform.eulerAngles = rot;
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				// Rotate to face right relative to the cams direction
				rot = Quaternion.LookRotation(cameraRight).eulerAngles;
				gameObject.transform.eulerAngles = rot;
			}
		}
		else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
		{
			anim.SetBool("Walk_Anim", false);
		}

		// Rotate Left
		// if (Input.GetKey(KeyCode.A))
		// {
		// 	rot[1] -= rotSpeed * Time.fixedDeltaTime;
		// }

		// Rotate Right
		// if (Input.GetKey(KeyCode.D))
		// {
		// 	rot[1] += rotSpeed * Time.fixedDeltaTime;
		// }

		// Roll
		// if (Input.GetKeyDown(KeyCode.Space))
		// {
		// 	if (anim.GetBool("Roll_Anim"))
		// 	{
		// 		anim.SetBool("Roll_Anim", false);
		// 	}
		// 	else
		// 	{
		// 		anim.SetBool("Roll_Anim", true);
		// 	}
		// }

		// Close
		// if (Input.GetKeyDown(KeyCode.LeftControl))
		// {
		// 	if (!anim.GetBool("Open_Anim"))
		// 	{
		// 		anim.SetBool("Open_Anim", true);
		// 	}
		// 	else
		// 	{
		// 		anim.SetBool("Open_Anim", false);
		// 	}
		// }
	}

}
