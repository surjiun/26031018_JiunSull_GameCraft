using System.Drawing;
using System.Numerics;
using Vortice.Direct2D1;
using Vortice.Mathematics;

enum PlayerAction
{
    Idle,
    Move,
    Attack,
    Parry,
    ParryRecovery
}

enum AttackResult
{
    Miss,
    Blocked,
    Hit
}

enum DefenseResult
{
    Miss,
    Hit,
    Parried
}

class Player
{
    public const int FrameSize = 192;
    public float X { get; private set; } = 384;
    public PlayerAction Action { get; private set; } = PlayerAction.Idle;
    public float ParryCooldown { get; private set; }
    public const int AttackDamage = 1;

    private const float Speed = 260;
    private const float Y = 426 - 176;
    private const float FrameDuration = 0.1f;
    private const float AttackDuration = 0.6f;
    private const float AttackActiveStart = 0.2f;
    private const float AttackActiveEnd = 0.4f;
    private const float ParryDuration = 0.2f;
    private const float ParryRecoveryDuration = 0.12f;
    private const float ParryCooldownDuration = 0.8f;

    private G2Texture _idleTexture;
    private G2Texture _moveTexture;
    private G2Texture _attackTexture;
    private G2Texture _parryTexture;
    private G2Audio _swingSound;
    private G2Audio _parryStartSound;
    private G2Audio _parrySuccessSound;
    private bool _attackSoundPlayed;
    private bool _facingLeft;
    private int _frame;
    private float _frameTime;
    private float _actionTime;
    private HashSet<int> _hitEnemyIds = new HashSet<int>();

    public Player()
    {
        string sfxDir = "resource/audio/sfx/";
        _swingSound = new G2Audio(sfxDir + "sword_swing.wav");
        _parryStartSound = new G2Audio(sfxDir + "parry_start.wav");
        _parrySuccessSound = new G2Audio(sfxDir + "parry_success.wav");
        string playerDir = "resource/player/";
        _idleTexture = new G2Texture(playerDir + "player_idle.png");
        _moveTexture = new G2Texture(playerDir + "player_move.png");
        _attackTexture = new G2Texture(playerDir + "player_attack.png");
        _parryTexture = new G2Texture(playerDir + "player_parry.png");
    }

    public void Update(G2InputContext input, float deltaTime, float stageWidth)
    {
        UpdateParryCooldown(deltaTime);

        // 한 프레임에는 한 행동만 처리한다. 공격·패링 중에는 이동 함수를 호출하지 않는다.
        if (Action == PlayerAction.Attack)
        {
            UpdateAttack(deltaTime);
        }
        else if (Action == PlayerAction.Parry || Action == PlayerAction.ParryRecovery)
        {
            UpdateParry(input, deltaTime);
        }
        else if (input.IsKeyDown(Keys.K) && ParryCooldown == 0)
        {
            ChangeAction(PlayerAction.Parry);
            UpdateParry(input, 0);
        }
        else if (input.IsKeyDown(Keys.J))
        {
            _hitEnemyIds.Clear();
            ChangeAction(PlayerAction.Attack);
            UpdateAttack(0);
        }
        else
        {
            UpdateMovement(input, deltaTime, stageWidth);
        }

        UpdateAnimation(deltaTime);
    }

    private void UpdateParryCooldown(float deltaTime)
    {
        ParryCooldown -= deltaTime;
        if (ParryCooldown < 0)
        {
            ParryCooldown = 0;
        }
    }

    private void UpdateMovement(G2InputContext input, float deltaTime, float stageWidth)
    {
        int direction = 0;
        if (input.IsKeyDown(Keys.A) || input.IsKeyPress(Keys.A))
        {
            direction--;
        }
        if (input.IsKeyDown(Keys.D) || input.IsKeyPress(Keys.D))
        {
            direction++;
        }

        float previousX = X;
        X += direction * Speed * deltaTime;
        if (X < 0)
        {
            X = 0;
        }
        if (X > stageWidth - FrameSize)
        {
            X = stageWidth - FrameSize;
        }
        if (direction < 0)
        {
            _facingLeft = true;
        }
        else if (direction > 0)
        {
            _facingLeft = false;
        }

        if (X != previousX)
        {
            ChangeAction(PlayerAction.Move);
        }
        else
        {
            ChangeAction(PlayerAction.Idle);
        }
    }

    private void UpdateAttack(float deltaTime)
    {
        _actionTime += deltaTime;
        if (_actionTime >= AttackDuration)
        {
            ChangeAction(PlayerAction.Idle);
            return;
        }
        if (_actionTime >= AttackActiveStart && _attackSoundPlayed == false)
        {
            _swingSound.Play();
            _attackSoundPlayed = true;
        }
    }

    private void UpdateParry(G2InputContext input, float deltaTime)
    {
        if (Action == PlayerAction.ParryRecovery)
        {
            // 성공 직후에는 마무리 자세 중에도 새 패링을 시작할 수 있다.
            if (input.IsKeyDown(Keys.K) && ParryCooldown == 0)
            {
                ChangeAction(PlayerAction.Parry);
                deltaTime = 0;
            }
            else
            {
                _actionTime += deltaTime;
                if (_actionTime >= ParryRecoveryDuration)
                {
                    ChangeAction(PlayerAction.Idle);
                }
                return;
            }
        }

        _actionTime += deltaTime;
        if (_actionTime >= ParryDuration)
        {
            ParryCooldown = ParryCooldownDuration;
            ChangeAction(PlayerAction.ParryRecovery);
            return;
        }

        // 임시 테스트: 패링 중 L을 누르면 정면 공격을 받은 것으로 처리한다.
        // TODO: 적의 공격에 ReceiveAttack을 연결한 뒤 이 블록을 삭제한다.
        if (input.IsKeyDown(Keys.L))
        {
            float attackerCenterX = X + FrameSize;
            if (_facingLeft)
            {
                attackerCenterX = X;
            }
            ReceiveAttack(GetBodyBounds(), attackerCenterX);
        }
    }

    private void UpdateAnimation(float deltaTime)
    {
        // 마무리 중에는 칼을 회수한 시트 1번 자세를 유지한다. 패링 판정은 없다.
        if (Action == PlayerAction.ParryRecovery)
        {
            _frame = 0;
            return;
        }

        int frameCount = 4;
        if (Action == PlayerAction.Move || Action == PlayerAction.Attack)
        {
            frameCount = 6;
        }
        else if (Action == PlayerAction.Parry)
        {
            frameCount = 2;
        }

        if (Action == PlayerAction.Attack || Action == PlayerAction.Parry)
        {
            _frame = (int)(_actionTime / FrameDuration);
            if (_frame >= frameCount)
            {
                _frame = frameCount - 1;
            }
            // 패링은 앞으로 쳐내는 시트 2번부터, 회수하는 시트 1번 순서로 재생한다.
            if (Action == PlayerAction.Parry)
            {
                _frame = frameCount - 1 - _frame;
            }
        }
        else
        {
            _frameTime += deltaTime;
            while (_frameTime >= FrameDuration)
            {
                _frameTime -= FrameDuration;
                _frame = (_frame + 1) % frameCount;
            }
        }
    }

    private void ChangeAction(PlayerAction action)
    {
        if (Action == action)
        {
            return;
        }
        Action = action;
        if (action == PlayerAction.Attack)
        {
            _attackSoundPlayed = false;
        }
        else if (action == PlayerAction.Parry)
        {
            _parryStartSound.Play();
        }
        _frame = 0;
        _frameTime = 0;
        _actionTime = 0;
    }

    public RectangleF GetBodyBounds()
    {
        return new RectangleF(X + 68, Y + 24, 56, 152);
    }

    public RectangleF GetAttackBounds()
    {
        if (Action != PlayerAction.Attack || _actionTime < AttackActiveStart || _actionTime >= AttackActiveEnd)
        {
            return RectangleF.Empty;
        }

        float centerX = X + FrameSize / 2;
        if (_facingLeft)
        {
            return new RectangleF(centerX - 110, Y + 48, 110, 112);
        }
        return new RectangleF(centerX, Y + 48, 110, 112);
    }

    // 적 번호는 중복되지 않게 부여한다. 체력 감소는 적 쪽에서 Hit 결과를 받아 처리한다.
    public AttackResult TryHitEnemy(int enemyId, RectangleF enemyBounds, bool isDefending)
    {
        if (_hitEnemyIds.Contains(enemyId))
        {
            return AttackResult.Miss;
        }
        RectangleF attackBounds = GetAttackBounds();
        if (attackBounds.IsEmpty || !attackBounds.IntersectsWith(enemyBounds))
        {
            return AttackResult.Miss;
        }

        _hitEnemyIds.Add(enemyId);
        if (isDefending)
        {
            return AttackResult.Blocked;
        }
        return AttackResult.Hit;
    }

    // 적 구현 후 공격마다 한 번만 호출한다. 피해·공격 소모는 반환값에 따라 호출 쪽에서 처리한다.
    public DefenseResult ReceiveAttack(RectangleF attackBounds, float attackerCenterX)
    {
        if (!GetBodyBounds().IntersectsWith(attackBounds))
        {
            return DefenseResult.Miss;
        }

        bool isFront = attackerCenterX >= X + FrameSize / 2;
        if (_facingLeft)
        {
            isFront = attackerCenterX <= X + FrameSize / 2;
        }
        if (Action == PlayerAction.Parry && _actionTime < ParryDuration && isFront)
        {
            ParryCooldown = 0;
            ChangeAction(PlayerAction.ParryRecovery);
            _parryStartSound.Stop();
            _parrySuccessSound.Play();
            return DefenseResult.Parried;
        }
        return DefenseResult.Hit;
    }

    public void Render(ID2D1RenderTarget renderTarget, float cameraX)
    {
        G2Texture texture = _idleTexture;
        if (Action == PlayerAction.Move)
        {
            texture = _moveTexture;
        }
        else if (Action == PlayerAction.Attack)
        {
            texture = _attackTexture;
        }
        else if (Action == PlayerAction.Parry || Action == PlayerAction.ParryRecovery)
        {
            texture = _parryTexture;
        }

        float screenX = X - cameraX;
        Rect destination = new(screenX, Y, FrameSize, FrameSize);
        Rect source = new(_frame * FrameSize, 0, FrameSize, FrameSize);

        Matrix3x2 originalTransform = renderTarget.Transform;
        if (_facingLeft)
        {
            Vector2 center = new(screenX + FrameSize / 2, 0);
            renderTarget.Transform = Matrix3x2.CreateScale(-1, 1, center) * originalTransform;
        }
        texture.Draw(destination, source, interpolationMode: BitmapInterpolationMode.NearestNeighbor);
        renderTarget.Transform = originalTransform;
    }

    public void Dispose()
    {
        _parrySuccessSound.Dispose();
        _parryStartSound.Dispose();
        _swingSound.Dispose();
        _parryTexture.Dispose();
        _attackTexture.Dispose();
        _moveTexture.Dispose();
        _idleTexture.Dispose();
    }
}
