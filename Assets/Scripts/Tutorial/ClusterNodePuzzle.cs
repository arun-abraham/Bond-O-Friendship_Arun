﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClusterNodePuzzle : MonoBehaviour {

	public List<ClusterNode> nodes;
	public List<GameObject> listeners;
	public ParticleSystem nodeParticle;
	public bool solved;

	public GameObject streamBlocker;
	public GameObject streamBlocker2;

	private float startingSize;
	private int litCount;
    public float progress = 0;


	void Awake()
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			nodes[i].lit = false;
			nodes[i].targetPuzzle = this;
		}

		if(streamBlocker != null && streamBlocker2 != null)
			startingSize = streamBlocker.transform.localScale.y;
	}
	
	public void NodeColored()
	{
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].lit)
                litCount++;
        }

        if (nodes.Count > 0)
        {
            progress = Mathf.Max((float)litCount / nodes.Count, progress);
        }
        litCount = 0;

		if(streamBlocker != null && streamBlocker2 != null)
		{
			streamBlocker.transform.localScale = new Vector3(streamBlocker.transform.localScale.x, Mathf.Min(startingSize - (startingSize / nodes.Count) * litCount, streamBlocker.transform.localScale.y), streamBlocker.transform.localScale.z);
			streamBlocker2.transform.localScale = new Vector3(streamBlocker2.transform.localScale.x, Mathf.Min(startingSize - (startingSize / nodes.Count) * litCount, streamBlocker2.transform.localScale.y), streamBlocker2.transform.localScale.z);
			
		}

		bool allLit = true;
		for (int i = 0; i < nodes.Count && allLit; i++)
		{
			if (!nodes[i].lit)
			{
				allLit = false;
			}
		}

		if (allLit && !solved)
		{
			solved = true;
			Destroy(streamBlocker);
			Destroy(streamBlocker2);
			for (int i = 0; i < listeners.Count; i++)
			{
				listeners[i].SendMessage("ClusterNodesSolved", this, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
