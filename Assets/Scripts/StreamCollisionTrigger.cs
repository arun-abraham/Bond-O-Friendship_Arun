﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamCollisionTrigger : MonoBehaviour {

	public Rigidbody collisionBody;
	public float oscillateAmount = 0.1f;
	private Vector3 oscillateDirection = new Vector3(0, 0, 1);
	private int streamLayer;
	public List<StreamReaction> reactions;
	public bool blockStream = true;
	private bool wasBlocking = true;
	public List<Stream> streamsTouched;
	public bool warnForBody = true;
	

	void Start()
	{
		streamLayer = LayerMask.NameToLayer("Water");

		if (collisionBody == null)
		{
			collisionBody = GetComponent<Rigidbody>();

			if (collisionBody == null)
			{
				Debug.LogError("Stream Collision Trigger on " + gameObject.name + " is not attached to a non-kinematic rigidbody. A non-kinematic rigidbody is required for collision detection with streams.");
			}
		}

		wasBlocking = blockStream;
	}

	void Update()
	{
		// Keep body moving to ensure that collisions are listened for.
		transform.position += oscillateDirection * oscillateAmount * Time.deltaTime;
		oscillateDirection *= -1;

		// If no longer blocking streams, unblock currently blocked streams.
		if (wasBlocking && !blockStream)
		{
			for (int i = 0; i < streamsTouched.Count; i++)
			{
				StopBlockingStream(streamsTouched[i]);
			}
		}
		else if (!wasBlocking && blockStream)
		{
			for (int i = 0; i < streamsTouched.Count; i++)
			{
				StartBlockingStream(streamsTouched[i]);
			}
		}
		wasBlocking = blockStream;

		float oldStreamTouchCount = streamsTouched.Count;
		for (int i = 0; i < streamsTouched.Count; i++)
		{
			if (streamsTouched[i] == null)
			{
				streamsTouched.RemoveAt(i);
				i--;
			}
		}
		if (oldStreamTouchCount != streamsTouched.Count)
		{
			SetReactionsStreamTouches();
		}
	}

	// If blocking is desired, block newly colliding streams.
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			Stream stream = col.collider.GetComponent<Stream>();
			if (stream != null)
			{
				CallStreamAction(stream);
				AddStreamTouched(stream);
			}
		}
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			Stream stream = col.collider.GetComponent<Stream>();
			if (stream != null)
			{
				CallStreamAction(stream);
				AddStreamTouched(stream);
			}
		}
	}

	// React to colliding streams.
	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			Stream stream = col.collider.GetComponent<Stream>();
			if (stream != null)
			{
				CallStreamAction(stream);
			}
		}
	}
	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			Stream stream = col.collider.GetComponent<Stream>();
			if (stream != null)
			{
				CallStreamAction(stream);
			}
		}
	}


	// Stop blocking streams that are no colliding.
	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			RemoveStreamTouched(col.collider.GetComponent<Stream>());
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			RemoveStreamTouched(col.collider.GetComponent<Stream>());
		}
	}

	void OnDestroy()
	{
		while (streamsTouched.Count > 0)
		{
			RemoveStreamTouched(0);
		}
	}

	private void AddStreamTouched(Stream stream)
	{
		if (!streamsTouched.Contains(stream))
		{
			if (blockStream)
			{
				StartBlockingStream(stream);
			}
			streamsTouched.Add(stream);
			SetReactionsStreamTouches();
		}
	}

	private void RemoveStreamTouched(Stream stream)
	{
		RemoveStreamTouched(streamsTouched.IndexOf(stream));
	}

	private void RemoveStreamTouched(int index)
	{
		if (index >= 0 && index < streamsTouched.Count)
		{
			StopBlockingStream(streamsTouched[index]);
			streamsTouched.RemoveAt(index);
			SetReactionsStreamTouches();
		}
	}

	private void StartBlockingStream(Stream stream)
	{
		stream.streamBlockers++;
	}

	private void StopBlockingStream(Stream stream)
	{
		stream.streamBlockers--;
	}

	private void SetReactionsStreamTouches()
	{
		for (int i = 0; i < reactions.Count; i++)
		{
			if (reactions[i] != null)
			{
				reactions[i].SetTouchedStreams(streamsTouched.Count);
			}
		}
	}

	void CallStreamAction(Stream stream)
	{
		if (stream != null)
		{
			for (int i = 0; i < reactions.Count; i++)
			{
				if (reactions[i] != null)
				{
					stream.ProvokeReaction(reactions[i]);
				}
			}
		}
	}
}
