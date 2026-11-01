using System.Drawing;
using Vortice.Direct2D1;

class MainMenu
{
	private static readonly PointF[] ButtonPositions =
	[
		new(544, 208), // 위쪽: 게임 화면 호출
		new(544, 304)  // 아래쪽: 종료
	];

	// 256×128 버튼 이미지에서 투명 여백을 제외한 판넬 영역이다.
	private static readonly RectangleF ButtonPanel = new(26, 32, 204, 64);

	private G2Texture _menuBackground;
	private G2Texture _titleLogo;
	private G2Texture _buttonNormal;
	private G2Texture _buttonSelected;
	private G2Texture _buttonPressed;
    private G2Audio _clickSound;
    private int _pressedButton = -1;

    public MainMenu()
    {
        string sfxDir = "resource/audio/sfx/";
        _clickSound = new G2Audio(sfxDir + "button_click.wav");
		string backgroundDir = "resource/background/";
		string menuDir = "resource/menu/";

		_menuBackground = new G2Texture(backgroundDir + "title_background.png");
		_titleLogo = new G2Texture(menuDir + "title_logo.png");
		_buttonNormal = new G2Texture(menuDir + "menu_button_normal.png");
		_buttonSelected = new G2Texture(menuDir + "menu_button_selected.png");
		_buttonPressed = new G2Texture(menuDir + "menu_button_pressed.png");
    }

    // 0: 시작, 1: 종료, -1: 클릭하지 않음
    public int Update(G2InputContext input)
    {
        int hoveredButton = FindButton(input.MousePosition);
        if (input.IsButtonDown(MouseButtons.Left))
        {
            _pressedButton = hoveredButton;
            if (hoveredButton >= 0)
            {
                _clickSound.Play();
            }
        }
        if (input.IsButtonUp(MouseButtons.Left))
        {
            int clickedButton = _pressedButton;
            _pressedButton = -1;
            if (clickedButton >= 0 && clickedButton == hoveredButton)
            {
                return clickedButton;
            }
        }
        return -1;
    }

	private static int FindButton(PointF mousePosition)
	{
		for (int i = 0; i < ButtonPositions.Length; i++)
		{
			PointF position = ButtonPositions[i];
			RectangleF bounds = new(position.X + ButtonPanel.X, position.Y + ButtonPanel.Y,
				ButtonPanel.Width, ButtonPanel.Height);
			if (bounds.Contains(mousePosition))
			{
				return i;
			}
		}
		return -1;
	}

    public void Render(G2InputContext input)
    {
		_menuBackground.Draw(0, 0, interpolationMode: BitmapInterpolationMode.NearestNeighbor);
		_titleLogo.Draw(480, 80, interpolationMode: BitmapInterpolationMode.NearestNeighbor);
		int hoveredButton = FindButton(input.MousePosition);
		for (int i = 0; i < ButtonPositions.Length; i++)
		{
			bool isPressed = _pressedButton == i && hoveredButton == i;
			G2Texture texture;
			if (isPressed)
			{
				texture = _buttonPressed;
			}
			else if (i == hoveredButton)
			{
				texture = _buttonSelected;
			}
			else
			{
				texture = _buttonNormal;
			}
			PointF position = ButtonPositions[i];
			texture.Draw(position.X, position.Y, interpolationMode: BitmapInterpolationMode.NearestNeighbor);
		}
	}

    public void Dispose()
    {
        _clickSound.Dispose();
        _buttonPressed.Dispose();
        _buttonSelected.Dispose();
        _buttonNormal.Dispose();
        _titleLogo.Dispose();
        _menuBackground.Dispose();
    }
}
