using UnityEngine;
using System.Collections;

public enum PageType {
	None,
	TitlePage,
	InGamePage,
	ScorePage
}

public class Main : MonoBehaviour {
	public static Main instance;
	public static float GameTime;
	
	private PageType _currentPageType = PageType.None;
	private BasePage _currentPage = null;
	
	private FStage _stage;
	
	public bool playIntro = true;
	public int crewSaved = 0;
	public bool gameFinished = false;
	
	void Start () {
	instance = this;
		GameTime = 0.0f;
		
		Go.defaultEaseType = EaseType.Linear;
		Go.duplicatePropertyRule = DuplicatePropertyRuleType.RemoveRunningProperty;
		
		bool landscape = true;
		bool portrait = false;
		
		// iOS support later!
		bool isIPad = SystemInfo.deviceModel.Contains("iPad");
		bool shouldSupportPortraitUpsideDown = isIPad && portrait; //only support portrait upside-down on iPad
		
		FutileParams fparams = new FutileParams(landscape, landscape, portrait, shouldSupportPortraitUpsideDown);
		
		fparams.backgroundColor = Color.grey; //new Color(0.0745f,0.0745f,0.0745f,1.0f); // RXColor.GetColorFromHex(0x74CBFF); //light blue 0x94D7FF or 0x74CBFF
		
		fparams.AddResolutionLevel(1280.0f, 1.0f, 1.0f, "");
		
		fparams.origin = new Vector2(0.0f,0.0f);

		Futile.instance.Init (fparams);
		
		Futile.atlasManager.LoadAtlas("Atlases/GameAtlas");
		Futile.atlasManager.LoadAtlas("Atlases/LevelAtlas");
		Futile.atlasManager.LoadAtlas("Atlases/BackgroundAtlas");
		//Futile.atlasManager.LoadAtlas("Atlases/FontAtlas");
		//Futile.atlasManager.LoadAtlas("Atlases/BackgroundAtlas");
		//Futile.atlasManager.LoadFont("emulogic64", "emulogic64", "Atlases/FontAtlas", 0, 0);
		Futile.atlasManager.LoadFont("Emulogic", "emulogic", "Atlases/emulogic", 0, 0);
		
		//FLabel test = new FLabel("Emulogic", "Hello World");

		_stage = Futile.stage;
		//_stage.AddChild(test);
		
		GoToPage(PageType.TitlePage);
		
		_stage.ListenForUpdate(HandleUpdate);
		_stage.ListenForResize(HandleResize);
	}
	
	void HandleResize(bool wasResizedDueToOrientationChange)
	{
		for(int s = 0;s<Futile.GetStageCount();s++)
		{
			Futile.GetStageAt(s).scale = Futile.screen.width / 1280.0f; //keep it full screen always!
		}
	}

	void HandleUpdate ()
	{
		float dt = Time.deltaTime;
		Main.GameTime += dt;
 
//		if (Input.GetKeyDown (KeyCode.Escape)) 
//		{
//			if(Screen.fullScreen)
//			{
//				Screen.fullScreen = false;
//			}
//			else 
//			{
//				Application.Quit();
//			}
//		}
//
//		if (Input.GetKeyDown (KeyCode.M)) 
//		{
//			FSoundManager.isMuted = !FSoundManager.isMuted;
//		}
//
//		if (Input.GetKeyDown (KeyCode.F)) 
//		{
//			Screen.fullScreen = !Screen.fullScreen;
//		}
	}
	
	public void GoToPage (PageType pageType)
	{
		if(_currentPageType == pageType) return; //we're already on the same page, so don't bother doing anything
		
		BasePage pageToCreate = null;
		
		if(pageType == PageType.TitlePage)
		{
			pageToCreate = new TitlePage();
		}
		if(pageType == PageType.InGamePage)
		{
			pageToCreate = new InGamePage();
		}
		else if (pageType == PageType.ScorePage)
		{
			pageToCreate = new ScorePage();
		} 
		
		if(pageToCreate != null) //destroy the old page and create a new one
		{
			_currentPageType = pageType;	
			
			if(_currentPage != null)
			{
				_currentPage.Destroy();
				_stage.RemoveChild(_currentPage);
			}
			 
			_currentPage = pageToCreate;
			_stage.AddChild(_currentPage);
			_currentPage.Start();
		}
		
	}
}
