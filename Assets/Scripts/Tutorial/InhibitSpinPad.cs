﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InhibitSpinPad : MonoBehaviour {

	public SpinPad spinPad;
	[SerializeField]
	public List<Collider> spinIgnoreCollisions;
	[SerializeField]
	public List<Collider> backSpinIgnoreCollisions;
	//public bool collided;
	public bool ignoreStream = true;

	void OnCollisionEnter(Collision col)
	{
		if (ignoreStream && LayerMask.LayerToName(col.gameObject.layer) == "Water")
		{
			return;
		}

		if (!spinIgnoreCollisions.Contains(col.collider))
		{
			spinPad.spinInhibitors++;
		}
		if (!backSpinIgnoreCollisions.Contains(col.collider))
		{
			spinPad.backSpinInhibitors++;
		}
		//if(col.transform.name == "Door Key Collider")
		//	collided = true;
	}
	
	void OnCollisionExit(Collision col)
	{
		if (ignoreStream && LayerMask.LayerToName(col.gameObject.layer) == "Water")
		{
			return;
		}

		if (!spinIgnoreCollisions.Contains(col.collider))
		{
			spinPad.spinInhibitors--;
		}
		if (!backSpinIgnoreCollisions.Contains(col.collider))
		{
			spinPad.backSpinInhibitors--;
		}
	}
}
