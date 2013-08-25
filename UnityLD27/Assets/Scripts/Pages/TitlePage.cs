using UnityEngine;
using System.Collections;

public class TitlePage : BasePage
{
	private FButton _startButton;
	private FButton _creditsButton;
	private FLabel _titleLabel;
	private FLabel _directionsLabel;
	
	public TitlePage()
	{
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
	}
	
	override public void Start()
	{	
		Main.instance.crewSaved = 0;
		Main.instance.gameFinished = false;
		
		_titleLabel = new FLabel("Emulogic", "Save Them");
		_titleLabel.color = Color.black; // new Color(0.173f, 0.722f, 0.976f, 1.0f);
		_titleLabel.scale = 1.0f;
		_titleLabel.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight + Futile.screen.halfHeight / 2);
		
		AddChild(_titleLabel);
		
		_startButton = new FButton("buttonWide");
		_startButton.AddLabel("Emulogic","Play!",Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
		_startButton.label.scale = 0.25f;
		_startButton.SetPosition(Futile.screen.halfWidth, 180);
		AddChild(_startButton);

		_startButton.SignalRelease += HandleStartButtonRelease;
		
		_creditsButton = new FButton("buttonWide");
		_creditsButton.AddLabel("Emulogic","About",Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
		_creditsButton.label.scale = 0.25f;
		_creditsButton.SetPosition(Futile.screen.halfWidth, 100);
		AddChild(_creditsButton);

		_creditsButton.SignalRelease += HandleCreditsButtonRelease;
	}
	
	protected void HandleResize(bool wasOrientationChange)
	{
		
	}
	
	protected void HandleUpdate ()
	{
		
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



