using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InGamePage : BasePage
{
	private Level1 _level1;
	
	private FButton _closeButton;
	
	private int _crewSpawned = 0;
	private float _lastSpawnedAt = 0;
	
	//private FContainer _levelContainer;
	private List<Crew> _crewMembers = new List<Crew>();
	
	public InGamePage()
	{		
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
	}
	
	override public void Start()
	{
		//_levelContainer = new FContainer();
		
		//AddChild(_levelContainer);
		
		
		_level1 = new Level1("ship1");
		_level1.SetPosition(0.0f, 0.0f);
		_level1.SetAnchor(0.0f, 0.0f);
		
		//_levelContainer.AddChild(_level1);
		AddChild(_level1);
		
		//_levelContainer.y = -(Futile.screen.halfHeight - _background.height / 2);
		
		this.shouldSortByZ = true;
		
		_closeButton = new FButton("button");
		_closeButton.AddLabel("Emulogic","X",Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
		_closeButton.label.scale = 0.25f;
		_closeButton.sortZ = 1;
		_closeButton.SignalRelease += HandleCloseButtonRelease;
		
		_closeButton.x = Futile.screen.width - 100.0f;
		_closeButton.y = Futile.screen.height -50.0f;
		
		AddChild(_closeButton);
		
		Clock clock = new Clock();
		clock.SetPosition( 50.0f, Futile.screen.height - 50.0f);
		AddChild(clock);
		
		spawnCrew(4);
	}
	
	private void spawnCrew(int crewCount){
		// spawn some crew
		for (int i=0; i<crewCount; i++){
			Crew crew = new Crew();
			crew.SetPosition(_level1.randomSpawnPosition());
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
		Main.instance.GoToPage(PageType.TitlePage);
	}
	
//	private void createPlayerProjectile (VectorDirection direction){
//		if (GroundZero.GameTime - _player.lastFired >= 0.25){
//			Projectile projectile = new Projectile(direction);
//			projectile.sortZ = 5;
//			projectile.x = _player.x;
//			projectile.y = _player.y;
//			_playerProjectiles.Add(projectile);
//			AddChild(projectile);
//			
//			_player.lastFired = GroundZero.GameTime;
//		}
//	}
	
	protected void HandleUpdate ()
	{
		float dt = Time.deltaTime;
		
		if (Main.GameTime - _lastSpawnedAt > 10 && _crewSpawned < 20){
			// spawn some new crew every 10 seconds
			_lastSpawnedAt = Main.GameTime;
			spawnCrew(2);
		}
		
		// update crew member's movement direction
		//loop backwards in case I decide to remove a crew member from the list, it won't cause problems
		for (int i = _crewMembers.Count-1; i >= 0; i--) 
		{
			Crew crew = _crewMembers[i];
			
			Vector2 newCrewPosition = crew.calculateNewPosition(dt);
			
			if (!checkCrewHeadingOk(crew, newCrewPosition)){
				// crew member heading into a wall, so heading was adjusted, don't update position	
				newCrewPosition = crew.GetPosition();
			}
			
			crew.SetPosition(newCrewPosition);
		}
	}
	
	// check if the crew member is going to run into a wall
	private bool checkCrewHeadingOk(Crew crewMember, Vector2 newPosition){
		bool headingOk = true;
		
		if (crewMember.direction == VectorDirection.Up){
			// check top walls and horizontal walls	
			int maxY; // max height the crew member can travel in this spot
			if (crewMember.y >= 16*64){
				maxY = (8 * 64) - 32;				
			} else {
				maxY = 9 * 64;
			}	
			if (newPosition.y >= maxY){
				headingOk = false;
				
				if (RXRandom.Range(1,2) == 1){
					crewMember.ChangeDirection(VectorDirection.Right);
				} else {
					crewMember.ChangeDirection(VectorDirection.Left);	
				}
			}
		} else if (crewMember.direction == VectorDirection.Down){
			// check bottom walls and horizontal walls	
			int minY; // min y the crew member can travel in this spot
			if (crewMember.x >= 16*64){
				minY = (2 * 64) + 34;				
			} else {
				minY = 64 ;
			}	
			if (newPosition.y <= minY){
				headingOk = false;	
				
				if (RXRandom.Range(1,2) == 1){
					crewMember.ChangeDirection(VectorDirection.Right);
				} else {
					crewMember.ChangeDirection(VectorDirection.Left);	
				}
			}
		} else if (crewMember.direction == VectorDirection.Left){
			// check bottom walls and horizontal walls	
			int minX; // min x the crew member can travel in this spot
			minX = (3 * 64) + 34;				
				
			if (newPosition.x <= minX){
				headingOk = false;	
				
				if (RXRandom.Range(1,2) == 1){
					crewMember.ChangeDirection(VectorDirection.Up);
				} else {
					crewMember.ChangeDirection(VectorDirection.Down);	
				}
			}	
		} else if (crewMember.direction == VectorDirection.Right){
			// check bottom walls and horizontal walls	
			int maxX; // max x the crew member can travel in this spot
			if (crewMember.y >= (2*64) + 34 && crewMember.y <= (8*64) - 34){
				maxX = (19 * 64) - 32;				
			} else {
				maxX = 16 * 64 - 32;
			}	
			if (newPosition.x >= maxX){
				headingOk = false;	
				
				if (RXRandom.Range(0,1) == 1){
					crewMember.ChangeDirection(VectorDirection.Up);
				} else {
					crewMember.ChangeDirection(VectorDirection.Down);	
				}
			}
		}
		
		return headingOk;
		
//			//keep player from walking into walls
//			if(newCrewPosition.y < -100 - crew.height){
//				// bottom of screen
//				newCrewPosition = crew.GetPosition();
//				crew.ChangeDirection(VectorDirection.Right);
//			} 
//			
//			if (newCrewPosition.y > 100 + crew.height){
//				// top of screen
//				newCrewPosition = crew.GetPosition();
//				crew.ChangeDirection(VectorDirection.Left);
//			} 
//			
//			if (newCrewPosition.x > 100 + crew.width){
//				// right side of screen
//				newCrewPosition = crew.GetPosition();
//				crew.ChangeDirection(VectorDirection.Up);
//			} 
//			
//			if (newCrewPosition.x < -100 - crew.width){	
//				// left side of screen
//				newCrewPosition = crew.GetPosition();
//				crew.ChangeDirection(VectorDirection.Down);
//			} 
	}
}

