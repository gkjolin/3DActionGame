﻿using UnityEngine;
using System.Collections;
using Cradle;
using Cradle.FM;

namespace Cradle{
public class HitArea : MonoBehaviour {
	
		void Damage(AttackInfo attackInfo)
	{
		this.transform.root.SendMessage ("Damage",attackInfo);
	}
}
}