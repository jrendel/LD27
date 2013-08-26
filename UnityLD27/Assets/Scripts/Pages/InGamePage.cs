using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InGamePage : BasePage, FSingleTouchableInterface
{	
	private FButton _closeButton;
	
	private int _crewSpawned = 0;
	private float _lastCycle = 0;
	private int _meltdownSegmentCount = 0;
	
	private LevelManager _levelManager;
	private List<Crew> _crewMembers = new List<Crew>();
	private MoveTile[] _inventory = new MoveTile[4];
	private int _invNumSelected = -1;
	
	private FLabel crewSavedLabel;
	private FSprite meltdownBar;
	private FSprite selectedInventory;
	
	public InGamePage()
	{		
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
		
		EnableSingleTouch();
	}
	
	override public void Start()
	{
		// once play navigates away from home screen, don't play the intro when they return
		Main.instance.playIntro = false;
		
		Main.instance.crewSaved = 0;
		
		FSprite background = new FSprite("mainGameViewport");
		background.SetAnchor(0.0f, 0.0f);
		AddChild(background);
		
		_levelManager = new LevelManager();
		
		AddChild(_levelManager);
				
		this.shouldSortByZ = true;
		
		meltdownBar = new FSprite("meltdown");
		meltdownBar.SetPosition(Futile.screen.halfWidth, 672);
		AddChild(meltdownBar);
		
		_closeButton = new FButton("button");
		_closeButton.AddLabel("Emulogic","Quit",Color.black);
		_closeButton.label.scale = 0.25f;
		_closeButton.sortZ = 1;
		_closeButton.SignalRelease += HandleCloseButtonRelease;
		
		_closeButton.x = Futile.screen.width - 125.0f;
		_closeButton.y = Futile.screen.height -75.0f;
		
		AddChild(_closeButton);
		
		Clock clock = new Clock();
		clock.SetPosition( 75.0f, Futile.screen.height - 75.0f);
		AddChild(clock);
		
		crewSavedLabel = new FLabel("Emulogic", "Crew Members Saved: 0");
		crewSavedLabel.SetPosition( Futile.screen.halfWidth, Futile.screen.height - 45.0f);
		crewSavedLabel.scale = 0.25f;
		AddChild(crewSavedLabel);
		
		selectedInventory = new FSprite("inventorySelected");
		selectedInventory.isVisible = false;
		AddChild(selectedInventory); 
		
		//spawnInventory();
		
		spawnCrew(2);
		
		_lastCycle = Main.GameTime;
	}
	
	private void spawnCrew(int crewCount){
		// spawn some crew
		for (int i=0; i<crewCount; i++){
			Crew crew = new Crew();
			crew.sortZ = 10;
			crew.SetPosition(_levelManager.randomSpawnPosition());
			//crew.direction = VectorDirection.Right;
			_crewMembers.Add(crew);
			//_levelContainer.AddChild(crew);
			AddChild(crew);
			_crewSpawned++;
		}
	}
	
	protected void HandleResize(bool wasOrientationChange)
	{
		//this will scale the background up to fit the screen
		//but it won't let it shrink smaller than 100%
//		_background.scale = Math.Max (Math.Max(1.0f,Futile.screen.height/_background.textureRect.height),Futile.screen.width/_background.textureRect.width);
//		 
//		_closeButton.x = -Futile.screen.halfWidth + 30.0f;
//		_closeButton.y = -Futile.screen.halfHeight + 30.0f;
//		
//		_scoreLabel.x = -Futile.screen.halfWidth + 10.0f;
//		_scoreLabel.y = Futile.screen.halfHeight - 10.0f;
//		
//		_timeLabel.x = Futile.screen.halfWidth - 10.0f;
//		_timeLabel.y = Futile.screen.halfHeight - 10.0f;
	}

	private void HandleCloseButtonRelease (FButton button)
	{
		GameOver();
	}
	
	private void GameOver(){
		Main.instance.gameFinished = true;
		Main.instance.GoToPage(PageType.ScorePage);	
	}
	
	public bool HandleSingleTouchBegan(FTouch touch) {
	    //Vector2 touchPosition = touch.position;
		
		int rowTouched = (int)touch.position.y / 64;
		int colTouched = (int)touch.position.x / 64;
				
		if ( colTouched == 1){
			bool invSelected = false;
			// could be inventory	
			if (rowTouched == 8){
				// inventory 1 (top)
				invSelected = true;
				_invNumSelected = 0;
			} else if (rowTouched == 6){
				// inventory 2	
				invSelected = true;
				_invNumSelected = 1;
			} else if (rowTouched == 3){
				// inventory 3 	
				invSelected = true;
				_invNumSelected = 2;
			} else if (rowTouched == 1){
				// inventory 4 (bottom)	
				invSelected = true;
				_invNumSelected = 3;
			}
			
			if (invSelected){	
				// so the user selected a valid inventory slot, 
				// now make sure there is something in that slot
				if (_inventory[_invNumSelected] != null){
					selectedInventory.SetPosition(colTouched * 64 + 32, rowTouched * 64 + 32);
					selectedInventory.isVisible = true;
				} else {
					// they clicked on an empty inventory
					invSelected = false;
					_invNumSelected = -1;
				}
			}
		}else {
			if (_invNumSelected != -1 && _inventory[_invNumSelected] != null){
				// they have an inventory item selected
				
				// check that they touched a valid tile in the game
				bool validTouch = false;
				if (colTouched >= 3 && colTouched <= 15 && rowTouched >= 1 && rowTouched <= 8){
					// main section of ship
					// now check its not in the escape pod room
					if (colTouched >= 7 || rowTouched >= 3){
						validTouch = true;
					}
				} else if (colTouched >= 16 && colTouched <= 18 && rowTouched >= 2 && rowTouched <= 7) {
					// spawn rooms	
					validTouch = true;
				}
				
				if (validTouch){
					MoveTile moveTile = _inventory[_invNumSelected];
					moveTile.SetPosition(colTouched * 64 + 32, rowTouched * 64 + 32);
					moveTile.PlaceTile();
										
					_levelManager.moveTileAdded(moveTile);
					
					_inventory[_invNumSelected] = null;
					selectedInventory.isVisible = false;
					_invNumSelected = -1;
				}
				
			} else {
				_levelManager.handleUserTouch(touch);
			}
		}
		
		return true;
	}
	
	public void HandleSingleTouchMoved(FTouch touch){
	    
	}
	 
	public void HandleSingleTouchEnded(FTouch touch){
	 
	}
	 
	public void HandleSingleTouchCanceled(FTouch touch){
	 
	}
	
	private void spawnInventory(){
		//Debug.Log("Spawn Inv - " + _inventory.Length);
		for (int i = 0; i < _inventory.Length; i++){
			if (_inventory[i] == null){
				//Debug.Log("Empty Slot - " + i); 
				MoveTile moveTile = new MoveTile();
				moveTile.sortZ = 5;				
				moveTile.SetPosition(positionForInventory(i));
				
				_inventory[i] = moveTile;
				
				AddChild(moveTile);
			}
		}
	}
	
	public Vector2 positionForInventory(int inventoryNumber){
		int row = 8;
		int col = 1;
		
		if (inventoryNumber == 0){
			row = 8;
		} else if (inventoryNumber == 1){
			row = 6;
		} else if (inventoryNumber == 2){
			row = 3;
		} else if (inventoryNumber == 3){
			row = 1;
		}
		
		return new Vector2(col * 64 + 32, row * 64 + 32);
	}
	
	
	protected void HandleUpdate ()
	{
		float dt = Time.deltaTime;

		if (Main.GameTime - _lastCycle > 10){
			// spawn some new crew every 10 seconds
			spawnInventory();
			//if(_crewSpawned < 20){
			if (_crewMembers.Count < 10){ // keep spawning crew so long as there arae not more than 10 currently out
				spawnCrew(3);
			}
			_levelManager.scrambleDoors();
			
			int totalMeltdownSegments = 20; 
			//  use 20 for shorter game play (about 3.3 minutes)
			//  use 32 for shorter game play (about 5.3 minutes)
			if (_meltdownSegmentCount < totalMeltdownSegments){
				
				float segmentWidth = 640 / totalMeltdownSegments;
				
				// draw another segment on the meltdown timer
				FSprite meltdownSegment = new FSprite("meltdownSegment");
				meltdownSegment.width = segmentWidth;
				// calculate position
				float x = Futile.screen.halfWidth - (meltdownBar.width / 2) + 4 + (segmentWidth / 2); // 4 is to get the spacing right
				x += _meltdownSegmentCount * segmentWidth;
				meltdownSegment.SetPosition(x, 672);
				AddChild(meltdownSegment);								
				_meltdownSegmentCount++;
			} else {
				GameOver();
			}
			
			_lastCycle = Main.GameTime;
		}
		
		// handle input
		bool invKeyPressed = false;
		
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			invKeyPressed = true;
			_invNumSelected = 0;
		} else if (Input.GetKeyDown(KeyCode.Alpha2)){
			invKeyPressed = true;
			_invNumSelected = 1;
		} else if (Input.GetKeyDown(KeyCode.Alpha3)){
			invKeyPressed = true;
			_invNumSelected = 2;
		} else if (Input.GetKeyDown(KeyCode.Alpha4)){
			invKeyPressed = true;
			_invNumSelected = 3;
		} else if (Input.GetKeyDown (KeyCode.Escape)){
			invKeyPressed = false;
			_invNumSelected = -1;
			selectedInventory.isVisible = false;
		}
		
		if (invKeyPressed){	
				// so the user selected a valid inventory slot, 
				// now make sure there is something in that slot
				if (_inventory[_invNumSelected] != null){
					selectedInventory.SetPosition(positionForInventory(_invNumSelected));
					selectedInventory.isVisible = true;
				} else {
					// they clicked on an empty inventory
					_invNumSelected = -1;
				}
		}
		
		// update crew member's movement direction
		//loop backwards so if we remove a crew member from the list, it won't cause problems
		for (int i = _crewMembers.Count-1; i >= 0; i--) 
		{
			Crew crewMember = _crewMembers[i];
			VectorDirection currentDirection = crewMember.direction;
			Vector2 newCrewPosition = crewMember.calculateNewPosition(dt);
			if (!_levelManager.crewMemberIsPassingThruDoor(crewMember)){ // passing through a door takes precidence
				if (_levelManager.willObeyFloorTile(crewMember, newCrewPosition)){
					
				} else if (_levelManager.willDirectCrewToDoor(crewMember)) { // check if crew member is next to an open door, if so they will be directed to it	
					if (crewMember.direction != currentDirection){
						// direction was updated, don't adjust position	
						newCrewPosition = crewMember.GetPosition();
					}
				} else if (!_levelManager.checkCrewHeadingOk(crewMember, newCrewPosition)){
					// crew member heading into a wall, so heading was adjusted, don't update position	
					newCrewPosition = crewMember.GetPosition();
				}
			}
			// update crew member's position (having taken into account all previous checks)
			crewMember.SetPosition(newCrewPosition);
			
			// check if crew member is in the escape pod room
			int escapePodXMax = (64 * 7) - 32;
			int escapePodYMax = (64 * 3) - 32;
			if (newCrewPosition.x <= escapePodXMax && newCrewPosition.y <= escapePodYMax){
				// you saved this crew member!
				FSoundManager.PlaySound("crewSaved1", 0.5f);
				Main.instance.crewSaved++;
				crewSavedLabel.text = "Crew Members Saved: " + Main.instance.crewSaved;
				
				_crewMembers.Remove(crewMember);
				crewMember.shouldDestroy = true;
			}
		}
		
		
		
	}
	
	
}

