using UnityEngine;
using System.Collections;

public class Level1 : FSprite
{
	public Level1(string name) : base(name)
	{
		
		
	}
	
	public Vector2 randomSpawnPosition(){
		// random col between 16 and 19
		// random row between 2 and 8
		
		int horzMin = (16 * 64) + 32;
		int horzMax = (19 * 64) - 32;
		int vertMin = (2 * 64) + 32;
		int vertMax = (8 * 64) - 32;
		float x = RXRandom.Range(horzMin, horzMax);
		float y = RXRandom.Range(vertMin, vertMax);
		
		// lets make sure the crew don't spawn on the wall between the spawning zones
		if (y >= (4 * 64) + 32 && y <= (5 * 64)){
			y = 4 * 64;	
		}
		
		if (y >= (5 * 64) && y <= (5 * 64) + 32){
			y = (5 * 64) + 34;	
		}
		
		return new Vector2(x, y);
	}
}

