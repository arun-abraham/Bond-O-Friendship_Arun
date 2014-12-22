﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BondAttachable : MonoBehaviour {
	public Rigidbody body;
	public bool handleFluffAttachment = true;
	public bool bondAtFluffPoint = true;
	public Color attachmentColor;
	public GameObject bondPrefab;
	[SerializeField]
	public List<Bond> bonds;
	public int volleysToBond;
	public int volleys = 0;
	public BondAttachable volleyPartner;

	void Awake()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
	}

	public void AttachFluff(Fluff fluff)
	{
		if (handleFluffAttachment && fluff != null)
		{
			AttemptBond(fluff.creator, fluff.transform.position);
		}
	}

	public Bond AttemptBond(BondAttachable bondPartner, Vector3 contactPosition, bool forceBond = false)
	{
		Bond newBond = null;
		if (bondPartner == null || bondPartner == this)
		{
			return newBond;
		}

		if (bondPartner.gameObject != gameObject)
		{
			volleys = 1;
			volleyPartner = bondPartner;
			if (bondPartner.volleyPartner == this)
			{
				volleys = bondPartner.volleys + 1;
			}

			if (forceBond || volleys >= volleysToBond)
			{
				// If enough volleys have been passed, and the volleyers are not already connected, establish a new bond.
				if (!IsBondMade(bondPartner))
				{
					Vector3 bondPoint = transform.position;
					if (bondAtFluffPoint)
					{
						bondPoint = contactPosition;
					}

					newBond = ((GameObject)Instantiate(bondPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Bond>();
					bonds.Add(newBond);
					bondPartner.bonds.Add(newBond);
					BondStatsHolder statsHolder = GetComponent<BondStatsHolder>();
					if (statsHolder != null && statsHolder.stats != null)
					{
						newBond.stats = statsHolder.stats;
					}

					// TODO this should be able to happen in reverse order (pulling attachments is buggy).
					//newBond.AttachPartners(this, bondPoint, bondPartner, bondPartner.transform.position);
					newBond.AttachPartners(bondPartner, bondPartner.transform.position, this, bondPoint);
					volleys = 0;
					bondPartner.volleys = 0;
				}
			}
		}

		return newBond;
	}

	private void OnDestroy()
	{
		for (int i = 0; i < bonds.Count; )
		{
			bonds[i].BreakBond();
		}
	}

	public bool IsBondMade(BondAttachable partner)
	{
		if (partner == null)
		{
			return bonds.Count > 0;
		}

		bool bondAlreadyMade = false;
		for (int i = 0; i < bonds.Count && !bondAlreadyMade; i++)
		{
			if ((bonds[i].attachment1.attachee == this && bonds[i].attachment2.attachee == partner) || (bonds[i].attachment2.attachee == this && bonds[i].attachment1.attachee == partner))
			{
				bondAlreadyMade = true;
			}
		}
		return bondAlreadyMade;
	}
}