using UnityEngine;
using System.Collections;

public class Clock : FSprite
{
	public bool shouldDestroy = false;
	
	private int _frameIndex = 0;
	private float _lastAnimated = 0;
	
	private FAtlasElement[] _frameElements;
	
    public Clock() : base("clock0")  {
		_frameElements = new FAtlasElement[11];
		_lastAnimated = Main.GameTime;
		
		FAtlasManager am = Futile.atlasManager;
		
		_frameElements[0] = am.GetElementWithName("clock0");
		_frameElements[1] = am.GetElementWithName("clock1");
		_frameElements[2] = am.GetElementWithName("clock2");
		_frameElements[3] = am.GetElementWithName("clock3");
		_frameElements[4] = am.GetElementWithName("clock4");
		_frameElements[5] = am.GetElementWithName("clock5");
		_frameElements[6] = am.GetElementWithName("clock6");
		_frameElements[7] = am.GetElementWithName("clock7");
		_frameElements[8] = am.GetElementWithName("clock8");
		_frameElements[9] = am.GetElementWithName("clock9");
		_frameElements[10] = am.GetElementWithName("clock10");
		
		
		ListenForUpdate(HandleUpdate);
    }

	
	public void HandleUpdate()
	{		
		if (!this.shouldDestroy){

			if(Main.GameTime - _lastAnimated >= 1) //update every 1 second
			{
				_lastAnimated = Main.GameTime;
				
				_frameIndex++;
				
				// update animation frame			
				if (_frameIndex >= _frameElements.Length){
					_frameIndex = 1;	
				}
				
				this.element = _frameElements[_frameIndex];	
			}
		} else {	
			//this.Destroy();
		}
	}
	
	public void Destroy(){
		this.RemoveFromContainer();	
	}
}


