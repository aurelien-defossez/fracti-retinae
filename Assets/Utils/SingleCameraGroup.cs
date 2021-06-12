using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleCameraGroup : MonoBehaviour
{
	[SerializeField, Range(1, 4)] int cameraIndex = 1;

	protected void Awake()
	{
		int cameraLayer = LayerMask.NameToLayer("Camera" + cameraIndex);

		foreach (Transform child in GetComponentsInChildren<Transform>(includeInactive: true))
		{
			child.gameObject.layer = cameraLayer;
		}
	}
}
