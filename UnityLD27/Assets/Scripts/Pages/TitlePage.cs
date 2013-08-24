using UnityEngine;
using System.Collections;

public class TitlePage : BasePage
{
	private FButton _startButton;
	private FLabel _titleLabel;
	
	public TitlePage()
	{
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
	}
	
	override public void Start()
	{		
		_titleLabel = new FLabel("Emulogic", "Save Them");
		_titleLabel.color = Color.black; // new Color(0.173f, 0.722f, 0.976f, 1.0f);
//		_titleLabel.x = 0; 
//		_titleLabel.y = Futile.screen.halfHeight / 2; 
		_titleLabel.scale = 1.0f;
		_titleLabel.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight + Futile.screen.halfHeight / 2);
		
		AddChild(_titleLabel);
		
		_startButton = new FButton("button");
		_startButton.AddLabel("Emulogic","Play!",Color.black);  //new Color(0.45f,0.25f,0.0f,1.0f)
		_startButton.label.scale = 0.25f;
		_startButton.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight);
		AddChild(_startButton);

		_startButton.SignalRelease += HandleStartButtonRelease;
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
}



