using UnityEngine;
using System.Collections;

public class BaseFSprite : FSprite
{

	public BaseFSprite(string name) : base(name)  {
		this.SetAnchor(0.0f, 0.0f);
	}
	
}

