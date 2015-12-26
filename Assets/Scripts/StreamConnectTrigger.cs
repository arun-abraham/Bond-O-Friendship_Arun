﻿using UnityEngine;
using System.Collections;

public class StreamConnectTrigger : MonoBehaviour {

	public GameObject connectionPartner;
	public float connectionDistance = 0.2f;
	public StreamChannelChange channelChange;
	public float waitTime = 0;
	public ZoomCamera zoomToDisable;
	private bool connected;

	void Update()
	{
		Vector3 toConnection = connectionPartner.transform.position - transform.position;
		toConnection.z = 0;
		if (connectionPartner != null && toConnection.sqrMagnitude <= Mathf.Pow(connectionDistance, 2) && ! connected)
		{
			StartCoroutine(ConnectStreamChannels());
			if (zoomToDisable != null)
			{
				zoomToDisable.FadeOff();
			}
		}
	}

	private IEnumerator ConnectStreamChannels()
	{
		if (channelChange != null && channelChange.preChangeChannel != null && channelChange.nextSeries != null)
		{
			connected = true;
			yield return new WaitForSeconds(waitTime);

			channelChange.preChangeChannel.parentSeries.streamChanges.Add(channelChange);
			
		}
	}
}
