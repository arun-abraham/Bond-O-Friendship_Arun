﻿using UnityEngine;
using System.Collections;

public class ClusterNodeColorSpecific : ClusterNode {

	public PlayerInput.Player neededPlayer;
	private Collider neededCollider;
	public Color colorDesaturation;

	protected override void Start()
	{
		base.Start();

		CharacterComponents neededCharacter = Globals.Instance.player1.character;
		if (neededPlayer == PlayerInput.Player.Player2)
		{
			neededCharacter = Globals.Instance.player2.character;
		}
		neededCollider = neededCharacter.GetComponent<Collider>();

		for (int i = 0; i < nodeRenderers.Length; i++)
		{
			nodeRenderers[i].material.color = neededCharacter.colors.baseColor - colorDesaturation;
		}
		startingcolor = nodeRenderers[0].material.color;
	}

	public override void CheckCollision(Collider col)
	{
		if (col == neededCollider)
		{
			base.CheckCollision(col);
		}
	}
}
