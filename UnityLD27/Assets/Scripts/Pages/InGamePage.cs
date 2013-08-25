using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InGamePage : BasePage, FSingleTouchableInterface
{
//	private Level1 _level1;
	
	private FButton _closeButton;
	
	private int _crewSpawned = 0;
	private float _lastCycle = 0;
	
	private LevelManager _levelManager;
	private List<Crew> _crewMembers = new List<Crew>();
	
	private FLabel crewSavedLabel;
	
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
		
		_levelManager = new LevelManager();
		
		AddChild(_levelManager);
				
		this.shouldSortByZ = true;
		
		_closeButton = new FButton("button");
		_closeButton.AddLabel("Emulogic","Quit",Color.black);
		_closeButton.label.scale = 0.25f;
		_closeButton.sortZ = 1;
		_closeButton.SignalRelease += HandleCloseButtonRelease;
		
		_closeButton.x = Futile.screen.width - 100.0f;
		_closeButton.y = Futile.screen.height -50.0f;
		
		AddChild(_closeButton);
		
		Clock clock = new Clock();
		clock.SetPosition( 50.0f, Futile.screen.height - 50.0f);
		AddChild(clock);
		
		crewSavedLabel = new FLabel("Emulogic", "Crew Members Saved: 0");
		crewSavedLabel.SetPosition( Futile.screen.halfWidth, Futile.screen.height - 50.0f);
		crewSavedLabel.scale = 0.25f;
		AddChild(crewSavedLabel);
		
		spawnCrew(2);
		
		_lastCycle = Main.GameTime;
	}
	
	private void spawnCrew(int crewCount){
		// spawn some crew
		for (int i=0; i<crewCount; i++){
			Crew crew = new Crew();
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
		Main.instance.gameFinished = true;
		Main.instance.GoToPage(PageType.ScorePage);
	}
	
	public bool HandleSingleTouchBegan(FTouch touch) {
	    //Vector2 touchPosition = touch.position;
		
		_levelManager.handleUserTouch(touch);
		
		
		return true;
	}
	
	public void HandleSingleTouchMoved(FTouch touch){
	    
	}
	 
	public void HandleSingleTouchEnded(FTouch touch){
	 
	}
	 
	public void HandleSingleTouchCanceled(FTouch touch){
	 
	}
	
	protected void HandleUpdate ()
	{
		float dt = Time.deltaTime;
		
		if (Main.GameTime - _lastCycle > 10 && _crewSpawned < 16){
			// spawn some new crew every 10 seconds
			spawnCrew(2);
			_levelManager.scrambleDoors();
			
			_lastCycle = Main.GameTime;
		}
		
		// update crew member's movement direction
		//loop backwards so if we remove a crew member from the list, it won't cause problems
		for (int i = _crewMembers.Count-1; i >= 0; i--) 
		{
			Crew crewMember = _crewMembers[i];
			VectorDirection currentDirection = crewMember.direction;
			Vector2 newCrewPosition = crewMember.calculateNewPosition(dt);
			if (!_levelManager.crewMemberIsPassingThruDoor(crewMember)){
				if (_levelManager.willDirectCrewToDoor(crewMember)) { // check if crew member is next to an open door, if so they will be directed to it	
					if (crewMember.direction != currentDirection){
						// direction was updated, don't adjust position	
						newCrewPosition = crewMember.GetPosition();
					}
				} else {
					if (!_levelManager.checkCrewHeadingOk(crewMember, newCrewPosition)){
						// crew member heading into a wall, so heading was adjusted, don't update position	
						newCrewPosition = crewMember.GetPosition();
					}
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

