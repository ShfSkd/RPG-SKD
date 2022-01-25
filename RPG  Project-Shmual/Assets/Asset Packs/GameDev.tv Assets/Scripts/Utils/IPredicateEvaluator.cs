﻿using System.Collections;
using UnityEngine;

namespace GameDevTV.Utils
{
	public interface IPredicateEvaluator
	{
		bool? Evaluate(string predicate, string[] parameters);
	}
}