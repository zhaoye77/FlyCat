﻿using UnityEngine;
using System.Collections;

public class ParticleSortingLayerFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		particleSystem.renderer.sortingLayerName = "Player";
		particleSystem.renderer.sortingOrder = -1;
	}
	

}
