using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InGamePage : BasePage
{
	private FSprite _background;
	
	private FButton _closeButton;
	
//	public Player _player;
//	//private FContainer _playerProjectilesContainer;
//	private List<Projectile> _playerProjectiles = new List<Projectile>();
	
	public InGamePage()
	{		
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
	}
	
	override public void Start()
	{
//		_background = new FSprite("Background");
//		AddChild(_background);
		
		this.shouldSortByZ = true;
		
		_closeButton = new FButton("button");
		_closeButton.AddLabel("emulogic64","X",Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
		_closeButton.label.scale = 1.0f;
		_closeButton.sortZ = 1;
		_closeButton.SignalRelease += HandleCloseButtonRelease;
		
		_closeButton.x = Futile.screen.halfWidth - 100.0f;
		_closeButton.y = Futile.screen.halfHeight -85.0f;
		
		AddChild(_closeButton);
		
//		_player = new Player("TestSmallCharacter");  // Create player1
//		_player.sortZ = 10;
//        AddChild(_player);
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
		Main.GameTime += dt;
		
//		float newPlayerX = _player.x;
//		float newPlayerY = _player.y;
//	     
//	    // Handle Input
//		
//		// player movement
//		if (Input.GetKey("w")) { 
//		    newPlayerY += dt * _player.currentVelocity;
//		}
//		if (Input.GetKey("s")) { 
//		    newPlayerY -= dt * _player.currentVelocity; 
//		}
//		
//		if (Input.GetKey("d")) { 
//		    newPlayerX += dt * _player.currentVelocity;
//		}
//		if (Input.GetKey("a")) { 
//		    newPlayerX -= dt * _player.currentVelocity; 
//		}
//		
//		// projectiles
//		if (Input.GetKey("up")) { 
//			createPlayerProjectile(VectorDirection.Up);
//		}
//		if (Input.GetKey("down")) { 
//			createPlayerProjectile(VectorDirection.Down);
//		}		
//		if (Input.GetKey("left")) { 
//			createPlayerProjectile(VectorDirection.Left);
//		}
//		if (Input.GetKey("right")) { 
//			createPlayerProjectile(VectorDirection.Right);
//		}
//		
//		// check for player moving off screen
////		if ( (newPlayerX + (_player.width/2) >= Futile.screen.halfWidth) ||
////			(newPlayerX - (_player.width/2) <= -Futile.screen.halfWidth) ){
////			newPlayerX = _player.x;	
////		}
////		
////		if ( (newPlayerY + (_player.height/2) >= Futile.screen.halfHeight) ||
////			(newPlayerY - (_player.height/2) <= -Futile.screen.halfHeight) ){
////			newPlayerY = _player.y;	
////		}
//		
//		if (newPlayerX + (_player.width/2) >= 486) {
//			newPlayerX = 486 - (_player.width/2);
//		}
//		if (newPlayerX - (_player.width/2) <= -486){
//			newPlayerX = -486 + (_player.width/2);	
//		}
//		
//		if (newPlayerY + (_player.height/2) >= 284) {
//			newPlayerY = 284 - (_player.height/2);
//		}
//		if (newPlayerY - (_player.height/2) <= -232){
//			newPlayerY = -232 + (_player.height/2);	
//		}
//		
//		// render the player in its new position
//		_player.x = newPlayerX;
//		_player.y = newPlayerY;
//		
//		// update projectile positions
//		//loop backwards so that if we remove a projectile from _playerProjectiles it won't cause problems
//		for (int i = _playerProjectiles.Count-1; i >= 0; i--) 
//		{
//			Projectile projectile = _playerProjectiles[i];
//			
//			//remove a projectile if it moves off screen
//			if(projectile.y < -200 - projectile.height ||
//				projectile.y > 200 + projectile.height ||
//				projectile.x < -445 - projectile.width ||
//				projectile.x > 445 + projectile.width)
//			{
//				_playerProjectiles.Remove(projectile);
//				projectile.shouldDestroy = true;
//				//this.RemoveChild(projectile);
//			}
//		}
	}
	
	
}

