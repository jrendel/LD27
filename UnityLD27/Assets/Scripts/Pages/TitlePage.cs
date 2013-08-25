using UnityEngine;
using System.Collections;

public class TitlePage : BasePage
{
	private FButton _startButton;
	private FButton _creditsButton;
	
	private FLabel _introLabel;
	private FLabel _titleLabel;
	private FLabel _finalLabel;
	
	
	private float _introSpeed = 0.03f;
	private float _lastTextUpdate;
	private int _introCharIndex = 0;
	private int _titleCharIndex = 0;
	private int _finalCharIndex = 0;
	
	private string _introText = "Welcome.\nSomething has gone wrong.\nThe ship is malfunctioning.\nThe crew are awakening from stasis.\nThey are disoriented.\nYou need to guide them to the escape pods.\nI can't control the doors.\nThey need your help.\n";
	private string _titleText = "SAVE THEM\n";
	private string _finalText = "Please.";
	
	
	public TitlePage()
	{
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
	}
	
	override public void Start()
	{	
		Main.instance.crewSaved = 0;
		Main.instance.gameFinished = false;
		
		FSprite background = new FSprite("viewport");
		background.SetAnchor(0.0f, 0.0f);
		AddChild(background);
		
		_introLabel = new FLabel("Emulogic", "");
		_introLabel.color = Color.black; //Color.green; // new Color(0.173f, 0.722f, 0.976f, 1.0f);
		_introLabel.scale = 0.2f;
		_introLabel.SetAnchor(0.0f, 1.0f);
		_introLabel.SetPosition(60, Futile.screen.height - 14);
		
		AddChild(_introLabel);
		
		_titleLabel = new FLabel("Emulogic", "");
		_titleLabel.color = Color.black; 
		_titleLabel.scale = 1.0f;
		_titleLabel.SetAnchor(0.0f, 1.0f);
		_titleLabel.SetPosition(200, 550);
		
		AddChild(_titleLabel);
		
		_finalLabel = new FLabel("Emulogic", "");
		_finalLabel.color = Color.black; //Color.green; 
		_finalLabel.scale = 0.2f;
		_finalLabel.SetAnchor(0.0f, 1.0f);
		_finalLabel.SetPosition(575, 400);
		
		AddChild(_finalLabel);

		if (!Main.instance.playIntro){
			_introLabel.text = _introText;
			_titleLabel.text = _titleText;
			_finalLabel.text = _finalText;
		}
		
		_startButton = new FButton("buttonWide");
		_startButton.AddLabel("Emulogic","Play!",Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
		_startButton.label.scale = 0.25f;
		_startButton.SetPosition(Futile.screen.width - 225, 155);
		AddChild(_startButton);

		_startButton.SignalRelease += HandleStartButtonRelease;
		
		_creditsButton = new FButton("buttonWide");
		_creditsButton.AddLabel("Emulogic","About",Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
		_creditsButton.label.scale = 0.25f;
		_creditsButton.SetPosition(Futile.screen.width - 225, 75);
		AddChild(_creditsButton);

		_creditsButton.SignalRelease += HandleCreditsButtonRelease;
		
		_lastTextUpdate = Main.GameTime;
		
		
		FSprite escapePod = new FSprite("escapePod");
		FSprite doors = new FSprite("doorsIcon");
		FSprite crew = new FSprite("floatingCharacter");
		FSprite clock = new FSprite("moveTileIcon");
		//FSprite clock = new FSprite("clock4");
		
		escapePod.scale = 0.5f;
		doors.scale = 0.5f;
		
		escapePod.SetPosition(140, 305);
		doors.SetPosition(140, 230);
		crew.SetPosition(140, 155);
		clock.SetPosition(140, 80);
		
		AddChild(escapePod);
		AddChild(doors);
		AddChild(crew);
		AddChild(clock);
		
		FLabel escapePodLabel = new FLabel("Emulogic", "Get the crew to \nthe escape pods!");
		escapePodLabel.color = Color.black; //Color.green; 
		escapePodLabel.scale = 0.15f;
		escapePodLabel.SetAnchor(0.0f, 0.5f);
		escapePodLabel.SetPosition(200, 305);		
		AddChild(escapePodLabel);
		
		FLabel doorsLabel = new FLabel("Emulogic", "Touch doors to \nopen and close them.");
		doorsLabel.color = Color.black; //Color.green; 
		doorsLabel.scale = 0.15f;
		doorsLabel.SetAnchor(0.0f, 0.5f);
		doorsLabel.SetPosition(200, 230);		
		AddChild(doorsLabel);
		
		FLabel crewLabel = new FLabel("Emulogic", "The Crew. They will walk \nthrough open doors, \nbut don't expect much else.");
		crewLabel.color = Color.black; //Color.green; 
		crewLabel.scale = 0.15f;
		crewLabel.SetAnchor(0.0f, 0.5f);
		crewLabel.SetPosition(200, 155);		
		AddChild(crewLabel);
		
		FLabel clockLabel = new FLabel("Emulogic", "Place movement tiles \nto redirect the crew.");
		//FLabel clockLabel = new FLabel("Emulogic", "10 Seconds.");
		clockLabel.color = Color.black; //Color.green; 
		clockLabel.scale = 0.15f;
		clockLabel.SetAnchor(0.0f, 0.5f);
		clockLabel.SetPosition(200, 80);		
		AddChild(clockLabel);
	}
	
	protected void HandleResize(bool wasOrientationChange)
	{
		
	}
	
	protected void HandleUpdate ()
	{
		if (Main.GameTime - _lastTextUpdate >= _introSpeed){
			// update text
			if (_introLabel.text != _introText){
				// type out intro text
				string character = _introText.Substring(_introCharIndex,1);
				_introLabel.text += character;
				if (character == "\n"){
					// add a delay after a newline
					_introSpeed = 0.75f;
				} else {
					// resume normal speed
					_introSpeed = 0.03f;	
				}
				_introCharIndex++;
			} else if (_titleLabel.text != _titleText){
				// type out title
				string character = _titleText.Substring(_titleCharIndex,1 );
				_titleLabel.text += character;
				_introSpeed = 0.1f;
				_titleCharIndex++;
				
				if (character == "\n"){
					// add a delay after a newline
					_introSpeed = 0.75f;
				}
			} else if (_finalLabel.text != _finalText) {
				// type final message
				string character = _finalText.Substring(_finalCharIndex,1 );
				_finalLabel.text += character;
				_introSpeed = 0.03f;
				_finalCharIndex++;
			}
			_lastTextUpdate = Main.GameTime;
		}
	}
	
	private void HandleStartButtonRelease (FButton button)
	{
		Main.instance.GoToPage(PageType.InGamePage);
	}
	
	private void HandleCreditsButtonRelease (FButton button)
	{
		Main.instance.GoToPage(PageType.ScorePage);
	}
}



