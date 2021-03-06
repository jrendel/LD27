using UnityEngine;
using System.Collections;

public class ScorePage : BasePage
{
	private FLabel _scoreLabel;
	private FLabel _textLabel;
	private FButton _againButton;
	private FButton _quitButton;
	private FLabel _aboutLabel;
	
	public ScorePage()
	{
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
	}
	
	override public void Start()
	{
		// once play navigates away from home screen, don't play the intro when they return
		Main.instance.playIntro = false;
		
		FSprite background = new FSprite("viewport");
		background.SetAnchor(0.0f, 0.0f);
		AddChild(background);
		
		string backButtonText = "Back";
		string titleText = "About The Game";
		if (Main.instance.gameFinished){
			// Display end game messages
			titleText = "The Ship Went Nuclear!";
			
			_scoreLabel = new FLabel("Emulogic", "You Saved " + Main.instance.crewSaved + " Crew Members");
			_scoreLabel.color = Color.black; // new Color(0.173f, 0.722f, 0.976f, 1.0f);
			_scoreLabel.scale = 0.25f;
			_scoreLabel.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight + Futile.screen.halfHeight / 2);
			
			AddChild(_scoreLabel);
			
			_againButton = new FButton("buttonWide");
			_againButton.AddLabel("Emulogic","Play Again",Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
			_againButton.label.scale = 0.25f;
			_againButton.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight + 80);
			AddChild(_againButton);
	
			_againButton.SignalRelease += HandleAgainButtonRelease;
			
			backButtonText = "Home";
		} else {
			// diplay generic message	
		}
		
		_textLabel = new FLabel("Emulogic", titleText);
		_textLabel.color = Color.black; 
		_textLabel.scale = 0.5f;
		_textLabel.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight + Futile.screen.halfHeight * 2 / 3);
		
		AddChild(_textLabel);
		
		_quitButton = new FButton("buttonWide");
		_quitButton.AddLabel("Emulogic",backButtonText,Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
		_quitButton.label.scale = 0.25f;
		_quitButton.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight);
		AddChild(_quitButton);

		_quitButton.SignalRelease += HandleQuitButtonRelease;
		
		string about = "Game Designed and created by Jason Rendel,\nfor Ludum Dare 27. My first game jam ever!\nCreated for the 48 hour competition.\n\n ---- Tools ----\n Unity 3d - Futile - Gimp \nTexture Packer - Glyph Designer - CFXR\n\nFollow me @jasonrendel\nCheck out what else I'm up to at www.jasonrendel.com";
		
		_scoreLabel = new FLabel("Emulogic", about);
		_scoreLabel.color = Color.black;
		_scoreLabel.scale = 0.20f;
		_scoreLabel.SetPosition(Futile.screen.halfWidth, 200);
		
		AddChild(_scoreLabel);
	}
	
	private void HandleAgainButtonRelease (FButton button)
	{
		Main.instance.GoToPage(PageType.InGamePage);
	}
	
	private void HandleQuitButtonRelease (FButton button)
	{
		Main.instance.GoToPage(PageType.TitlePage);
	}
	
		protected void HandleResize(bool wasOrientationChange)
	{
		
	}
	
	protected void HandleUpdate ()
	{
		
	}
	
}

