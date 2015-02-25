﻿using UnityEngine;
using System.Collections;

public class PullApart : MonoBehaviour {

	public GameObject otherSphere;
	public GameObject connector;
	private float xScale;
	public float currentDistance;
	public float breakDistance = 5.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		connector.transform.position = (transform.position + otherSphere.transform.position)/2;
		xScale = Vector3.Distance(transform.position, otherSphere.transform.position);
		connector.transform.localScale = new Vector3(xScale, 2, 1);
		connector.transform.right = transform.position - otherSphere.transform.position;

		currentDistance = Vector3.Distance(transform.position, otherSphere.transform.position);

		Vector3 toOther = transform.position - otherSphere.transform.position;
		toOther.z = 0;
		//transform.forward = otherSphere.transform.forward = Vector3.forward;
		transform.right = -toOther;
		Vector3 rot = transform.rotation.eulerAngles;
		if (rot.y == 180)
		{
			rot.z = rot.y;
			rot.y = 0;
			transform.rotation = Quaternion.Euler(rot);
		}

		otherSphere.transform.right = toOther;
		Vector3 otherRot = otherSphere.transform.rotation.eulerAngles;
		if (otherRot.y == 180)
		{
			otherRot.z = otherRot.y;
			otherRot.y = 0;
			otherSphere.transform.rotation = Quaternion.Euler(otherRot);
		}


		if(currentDistance > breakDistance)
		{
			Destroy(otherSphere);
			Destroy(connector);
			Destroy(gameObject);
		}
	}
}
