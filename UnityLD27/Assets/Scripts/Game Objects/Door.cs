using UnityEngine;
using System.Collections;

public class Door : FSprite
{
	public bool shouldDestroy = false;
	
	private FAtlasElement[] _frameElements;
	
	public bool doorIsHorizontal;
	public bool doorIsOpen = false;
	
	public int startRow;
	public int startCol;	
	
	private int doorHorzOpen = 0;
	private int doorHorzClosed = 1;
	private int doorVertOpen = 2;
	private int doorVertClosed = 3;
	
    public Door(bool isHorizontal, int startRow, int startCol) : base("doorVertClosed") {			
		doorIsHorizontal = isHorizontal;
		
		this.startRow = startRow;
		this.startCol = startCol;
		
		SetDoorPosition();
		
		_frameElements = new FAtlasElement[4];
		
		FAtlasManager am = Futile.atlasManager;
		
		_frameElements[doorHorzOpen] = am.GetElementWithName("doorHorzOpen");
		_frameElements[doorHorzClosed] = am.GetElementWithName("doorHorzClosed");
		_frameElements[doorVertOpen] = am.GetElementWithName("doorVertOpen");
		_frameElements[doorVertClosed] = am.GetElementWithName("doorVertClosed");
		
		if (isHorizontal){
			this.element = _frameElements[doorHorzClosed];	
		}
		
		SetDoorStatus(false);
		
		ListenForUpdate(HandleUpdate);
    }
	
	private void SetDoorPosition(){
		int x;
		int y;
		if (doorIsHorizontal){ // horizontal door as tile above and below
			x = (startCol * 64 ) + 32;
			y = (startRow + 1) * 64;
		} else { // vertical door - tile on left and right
			x = (startCol + 1) * 64;
			y = (startRow * 64 ) + 32;
		}
		
		this.SetPosition(x, y);
	}
	
	public void SetDoorStatus(bool open){
		doorIsOpen = open;	
		
		// set image
		int frameIndex = 0;
			
		if(doorIsOpen){			
			if (doorIsHorizontal){ 
				frameIndex = doorHorzOpen;
			}else { // door is vertical
				frameIndex = doorVertOpen;
			}
		} else { // door is closed
			if (doorIsHorizontal){
				frameIndex = doorHorzClosed;
			}else { // door is vertical
				frameIndex = doorVertClosed;
			}
		}
		
		this.element = _frameElements[frameIndex];
	}
	
	public void HandleUpdate()
	{		
		if (!this.shouldDestroy){
//			int frameIndex = 0;
//			
//			if(doorIsOpen){			
//				if (doorIsHorizontal){ 
//					frameIndex = doorHorzOpen;
//				}else { // door is vertical
//					frameIndex = doorVertOpen;
//				}
//			} else { // door is closed
//				if (doorIsHorizontal){
//					frameIndex = doorHorzClosed;
//				}else { // door is vertical
//					frameIndex = doorVertClosed;
//				}
//			}
//			
//			this.element = _frameElements[frameIndex];	
		} else {	
			//this.Destroy();
		}
	}

}




