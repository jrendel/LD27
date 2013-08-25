using UnityEngine;
using System.Collections;

public enum VectorDirection
{
	Up,
	Down,
	Left,
	Right
}

public class Crew : FSprite
{
	public float defaultVelocity;   // Default velocity
    public float currentVelocity;   // Current maximum velocity
    public VectorDirection direction;
	
	public bool shouldDestroy = false;
	
	private int _frameIndex = 0;
	private bool _directionChanged = false;
	
	private FAtlasElement[] _leftFrameElements;
	private FAtlasElement[] _rightFrameElements;
	private FAtlasElement[] _upFrameElements;
	private FAtlasElement[] _downFrameElements;
	
	public MoveTile lastMoveTileUsed;
	
    public Crew() : base("walkRight0")  {
         
        this.defaultVelocity = 100; 
        this.currentVelocity = this.defaultVelocity;
		
		int randomDirection = RXRandom.Range(0, 4);
		this.direction = VectorDirection.Up; // (VectorDirection)randomDirection;
		
		if (randomDirection == 1){
			this.direction = VectorDirection.Left;
		} else if (randomDirection == 2){
			this.direction = VectorDirection.Right;
		} else if (randomDirection == 3){
			this.direction = VectorDirection.Down;
		}
		
		
		_leftFrameElements = new FAtlasElement[4];
		_rightFrameElements = new FAtlasElement[4];
		_upFrameElements = new FAtlasElement[4];
		_downFrameElements = new FAtlasElement[4];
		
		FAtlasManager am = Futile.atlasManager;
		
		_rightFrameElements[0] = am.GetElementWithName("walkRight0");
		_rightFrameElements[1] = am.GetElementWithName("walkRight1");
		_rightFrameElements[2] = am.GetElementWithName("walkRight2");
		_rightFrameElements[3] = am.GetElementWithName("walkRight3");
		
		_leftFrameElements[0] = am.GetElementWithName("walkLeft0");
		_leftFrameElements[1] = am.GetElementWithName("walkLeft1");
		_leftFrameElements[2] = am.GetElementWithName("walkLeft2");
		_leftFrameElements[3] = am.GetElementWithName("walkLeft3");
		
		_upFrameElements[0] = am.GetElementWithName("walkUp0");
		_upFrameElements[1] = am.GetElementWithName("walkUp1");
		_upFrameElements[2] = am.GetElementWithName("walkUp2");
		_upFrameElements[3] = am.GetElementWithName("walkUp3");
		
		_downFrameElements[0] = am.GetElementWithName("walkDown0");
		_downFrameElements[1] = am.GetElementWithName("walkDown1");
		_downFrameElements[2] = am.GetElementWithName("walkDown2");
		_downFrameElements[3] = am.GetElementWithName("walkDown3");
		
		
		ListenForUpdate(HandleUpdate);
    }
	
	public void ChangeDirection(VectorDirection direction){
		this.direction = direction;
		// reset out animation frame counter
		_frameIndex = 0;
		_directionChanged = true;
	}
	
	public Vector2 calculateNewPosition(float dt){
		float x = this.x;
		float y = this.y;
		
		float movement = dt * this.currentVelocity;
		
		if (this.direction == VectorDirection.Up){
			y += movement;
		} else if (this.direction == VectorDirection.Down){
			y -= movement;
		} if (this.direction == VectorDirection.Left){
			x -= movement;
		} if (this.direction == VectorDirection.Right){
			x += movement;
		} 	
		
		Vector2 newPosition = new Vector2(x, y);
		
		return newPosition;
	}
	
	public void HandleUpdate()
	{		
		if (!this.shouldDestroy){
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
			
			if(Time.frameCount % 10 == 0 || _directionChanged) //update every other 10th frame or when the direction changes
			{
				_directionChanged = false;
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
		this.RemoveFromContainer();	
	}
}

