﻿using System.Collections;
using UnityEngine;

namespace RPG.Saving
{
	[System.Serializable]
	public class SerializbleVector3
	{
		float x, y, z;

		public SerializbleVector3(Vector3 vector3)
		{
			x = vector3.x;
			y = vector3.y;
			z = vector3.z;
		}
		public Vector3 ToVector()
		{
			return new Vector3(x,y,z);
		}
	}
}