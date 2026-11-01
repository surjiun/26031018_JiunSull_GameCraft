using Vortice.Mathematics;

class GameMain : G2AppBase
{
    private enum Scene
    {
        MainMenu,
        Play
    }

    private G2Audio _backgroundMusic;
    private MainMenu _mainMenu;
    private PlayScene _playScene;
    private Scene _scene = Scene.MainMenu;

    public override System.Drawing.Size ScreenSize => GameGlobal.ScreenSize;
    public override string GameName => GameGlobal.GameName;

    public GameMain()
    {
        string bgmDir = "resource/audio/bgm/";
        _backgroundMusic = new G2Audio(bgmDir + "background.wav");
        _mainMenu = new MainMenu();
        _playScene = new PlayScene();
    }

    protected override void Initialize()
    {
        ClearColor = new Color4(0.06f, 0.08f, 0.11f, 1.0f);
        _backgroundMusic.Play(true);
    }

    protected override void Update()
    {
        if (_scene == Scene.MainMenu)
        {
            int clickedButton = _mainMenu.Update(Input);
            if (clickedButton == 0)
            {
                _scene = Scene.Play;
            }
            else if (clickedButton == 1)
            {
                Close();
            }
        }
        else if (_scene == Scene.Play)
        {
            _playScene.Update(Input, (float)DeltaTime);
        }
    }

    protected override void Render()
    {
        if (_scene == Scene.MainMenu)
        {
            _mainMenu.Render(Input);
        }
        else if (_scene == Scene.Play)
        {
            _playScene.Render(RenderTarget);
        }
    }

    public override void Dispose()
    {
        _backgroundMusic.Stop();
        _backgroundMusic.Dispose();
        _playScene.Dispose();
        _mainMenu.Dispose();
        base.Dispose();
    }
}
