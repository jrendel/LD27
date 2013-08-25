using UnityEngine;
using System.Collections;

public class MoveTile : FSprite
{
	public bool shouldDestroy = false;
	public bool placed = false;
	
	private FAtlasElement[] _leftFrameElements;
	private FAtlasElement[] _rightFrameElements;
	private FAtlasElement[] _upFrameElements;
	private FAtlasElement[] _downFrameElements;
	
	public VectorDirection direction;
	
	private int _frameIndex = 0;
	private float _placedAt;
	
	public MoveTile() : base("moveTileUp"){
				
		_leftFrameElements = new FAtlasElement[1];
		_rightFrameElements = new FAtlasElement[1];
		_upFrameElements = new FAtlasElement[1];
		_downFrameElements = new FAtlasElement[1];
		
		FAtlasManager am = Futile.atlasManager;
		
		_leftFrameElements[0] = am.GetElementWithName("moveTileLeft");
		_rightFrameElements[0] = am.GetElementWithName("moveTileRight");
		_upFrameElements[0] = am.GetElementWithName("moveTileUp");
		_downFrameElements[0] = am.GetElementWithName("moveTileDown");
	
		_placedAt = 0.0f;
		
		int randomDirection = RXRandom.Range(0, 4);
		this.direction = VectorDirection.Up; // (VectorDirection)randomDirection;
		
		if (randomDirection == 1){
			this.direction = VectorDirection.Left;
			this.element = _leftFrameElements[0];
		} else if (randomDirection == 2){
			this.direction = VectorDirection.Right;
			this.element = _rightFrameElements[0];
		} else if (randomDirection == 3){
			this.direction = VectorDirection.Down;
			this.element = _downFrameElements[0];
		}
		
		ListenForUpdate(HandleUpdate);
	}
	
	public void PlaceTile(){
		_placedAt = Main.GameTime;
		this.placed = true;
	}
	
	public void HandleUpdate()
	{		
		if (!this.shouldDestroy){
			if (this.placed && Main.GameTime - _placedAt > 10){
				this.Destroy();	
			}
//			float dt = Time.deltaTime;
//			float movement = dt * this.currentVelocity;
//			
//			if (this.direction == VectorDirection.Up){
//				this.y += movement;
//			} else if (this.direction == VectorDirection.Down){
//				this.y -= movement;
//			} if (this.direction == VectorDirection.Left){
//				this.x -= movement;
//			} if (this.direction == VectorDirection.Right){
//				this.x += movement;
//			} 	
			
			if(Time.frameCount % 10 == 0) //update every other 10th frame or when the direction changes
			{
				FAtlasElement[] _frameElements = _rightFrameElements;
				
				if (this.direction == VectorDirection.Left){
					_frameElements = _leftFrameElements;
				} else if (this.direction == VectorDirection.Up){
					_frameElements = _upFrameElements;
				} else if (this.direction == VectorDirection.Down){
					_frameElements = _downFrameElements;
				} 
				
				// update animation frame			
				if (_frameIndex >= _frameElements.Length){
					_frameIndex = 0;	
				}
				
				this.element = _frameElements[_frameIndex];		
				
				_frameIndex++;
			}
		} else {	
			this.Destroy();
		}
	}
	
	public void Destroy(){
		this.shouldDestroy = true;
		this.RemoveFromContainer();	
	}
}

