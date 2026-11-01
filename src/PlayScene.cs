using Vortice.Direct2D1;

class PlayScene
{
    private G2Texture _background;
    private Player _player;
    private float _cameraX;
    private const float StageWidth = 1920;

    public PlayScene()
    {
        string backgroundDir = "resource/background/";
        _background = new G2Texture(backgroundDir + "stage_background.png");
        _player = new Player();
    }

    public void Update(G2InputContext input, float deltaTime)
    {
        _player.Update(input, deltaTime, StageWidth);

        // 플레이어를 따라가되 배경 양 끝을 넘어가지 않는다.
        _cameraX = _player.X + Player.FrameSize / 2 - GameGlobal.ScreenSize.Width / 2;
        if (_cameraX < 0)
        {
            _cameraX = 0;
        }
        if (_cameraX > StageWidth - GameGlobal.ScreenSize.Width)
        {
            _cameraX = StageWidth - GameGlobal.ScreenSize.Width;
        }
    }

    public void Render(ID2D1RenderTarget renderTarget)
    {
        _background.Draw(-_cameraX, 0, interpolationMode: BitmapInterpolationMode.NearestNeighbor);
        _player.Render(renderTarget, _cameraX);
    }

    public void Dispose()
    {
        _player.Dispose();
        _background.Dispose();
    }
}
