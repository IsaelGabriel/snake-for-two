using System.Numerics;
using Raylib_cs;

public class Player(uint ID, int x, int y, MainScene parentScene, float movementInterval) : CellEntity(x, y, parentScene) {
    private const int _defaultLength = 5;

    private static readonly Color _powerUpColor = Color.RayWhite;
    private static readonly Color[] _headColors = [
        Color.Purple,
        Color.Yellow,
        Color.Blue,
    ];
    private static readonly Color[] _bodyColors = [
        Color.DarkPurple,
        Color.Orange,
        Color.SkyBlue,
    ];

    private const uint _maxMovementBufferSize = 3;
    private float _movementInterval = movementInterval;
    private float _movementIntervalCount = movementInterval;
    private float _animationCount = 0f;
    private readonly uint _ID = ID;
    private Vector2 _lastMovement = new(0, -1);
    private Vector2 _movement = new(0, -1);
    private List<Vector2> _movementBuffer = [];
    public Vector2 movement {
        get=>_movement;
        set{
            if(value.Equals(Vector2.Zero)) return;
            if(value.X != 0f && value.Y != 0f) return;
            value = Vector2.Normalize(value);
            if(value.Equals(-_lastMovement)) return;
            _movement = value;
            
        }
    }
    private Color _headColor = (_headColors.Length > ID)? _headColors[ID] : Color.Gray;
    private Color _bodyColor = (_bodyColors.Length > ID)? _bodyColors[ID] : Color.DarkGray;
    private List<Vector2> sections = [];
    public ItemType? item = null;

    public override void Start()
    {
        for(int i = 0; i < _defaultLength; i++) {
            sections.Add(new(x, y+i));
        }
        sections.Add(sections.Last());
    }

    public override void Update() {
        x = (int) sections[0].X;
        y = (int) sections[0].Y;

        Vector2 newMovement = new() {
            X = Input.GetAxisPress(_ID, Action.Left, Action.Right),
            Y = Input.GetAxisPress(_ID, Action.Up, Action.Down)
        };

        if(newMovement != Vector2.Zero) {
            if(newMovement.X != 0f && newMovement.Y != 0f) {
                Vector2 moveX = newMovement;
                Vector2 moveY = newMovement;
                moveX.Y = 0f;
                moveY.X = 0f;
                AddToMovementBuffer(moveX);
                AddToMovementBuffer(moveY);
            }else {
                AddToMovementBuffer(newMovement);
            }
        }



        _movementIntervalCount -= Raylib.GetFrameTime();

        if(_movementIntervalCount > 0f) return;
        _animationCount = 0f;

        if(_movementBuffer.Count > 0) {
            movement = _movementBuffer.First();
            _movementBuffer.RemoveAt(0);
        }

        int newX = (int) sections[0].X + (int) movement.X;
        int newY = (int) sections[0].Y + (int) movement.Y;
        CellEntity? collision = scene.GetEntityInCell(newX, newY);
        bool solidCollision = false;
        if(collision != null) {
            if(collision is Item) {
                switch(((Item)collision).type) {
                    case ItemType.Apple:
                        sections.Add(sections.Last());
                        scene.Destroy(collision);
                    break;
                    case ItemType.PowerUp:
                        item = ItemType.PowerUp;
                        scene.Destroy(collision);
                    break;
                    default:
                        scene.Destroy(collision);
                    break;
                }
                
            }else if(collision is Player) {
                solidCollision = true;
                if(item == ItemType.PowerUp) {
                    Player enemy = (Player) collision;
                    int index = enemy.IndexOfSection(newX, newY);
                    if(index == 0) Game.LoadScene(new GameOverScene(_ID));
                    else {
                        enemy.sections.RemoveAt(index);
                        while(index < enemy.sections.Count) {
                            scene.AddEntity(new Item(ItemType.Apple, (int) enemy.sections[index].X, (int) enemy.sections[index].Y, scene));
                            enemy.sections.RemoveAt(index);
                        }
                        solidCollision = false;
                    }
                }else {
                    Game.LoadScene(new GameOverScene(_ID));
                }
                item = null;
            }
        }
        if(!solidCollision) {
            int oldX = x;
            int oldY = y;
            x = newX;
            y = newY;
            if((x != oldX || y != oldY) && sections.Count > 2) {
                sections.Insert(0, new Vector2(x, y));
                sections.RemoveAt(sections.Count - 1);
            }else {
                Game.LoadScene(new GameOverScene(_ID));
            }
        }
        _lastMovement = _movement;
        _movementIntervalCount = Math.Clamp(_movementInterval - (sections.Count - 2) * 0.01f, 0.025f, _movementInterval);
        if(item == ItemType.PowerUp) _movementIntervalCount *= 0.5f;
        x = (int) sections[0].X;
        y = (int) sections[0].Y;
    }   

    public override void Render() {
        _animationCount += Raylib.GetFrameTime();
        if(_animationCount > _movementInterval / 2) _animationCount = _movementInterval / 2;
        float t = _animationCount / (_movementInterval / 2);
        Vector2 position = GetAnimationPosition(sections[1], sections[0], t);
        Vector2 size = Vector2.One * MainScene.TileSize;


        for(int i = 1; i < sections.Count - 1; i++) {
            Raylib.DrawRectangleV(GetAnimationPosition(sections[i+1], sections[i], t), Vector2.One * MainScene.TileSize, _bodyColor);
        }
        Raylib.DrawRectangleV(position, size, _headColor);
        if(item == ItemType.PowerUp) {
            Raylib.DrawRectangleV(position + Vector2.One * 3, size - Vector2.One * 6, _powerUpColor);
        }
    
    }

    /// <summary>
    /// Calculates the position of an object based on the animation time and the trajectory.
    /// </summary>
    /// <param name="from">Start position.</param>
    /// <param name="to">End position.</param>
    /// <param name="t">Time of the animation, considering that the start of it is in t = 0, and the end in t = 1.</param>
    /// <returns>The position where the object needs to be.</returns>
    private Vector2 GetAnimationPosition(Vector2 from, Vector2 to, float t) {
        Vector2 movement = (to - from) * (float) Math.Sin(t * Math.PI/2);
        return (from + movement) * MainScene.TileSize;
    }

    public override bool IsFillingCell(int x, int y)
    {
        return sections.Contains(new Vector2(x, y));
    }
    
    public int IndexOfSection(int x, int y) {
        int i = 0;
        foreach(Vector2 section in sections) {
            if((int) section.X == x && (int) section.Y == y) return i;
            i++;
        }
        return -1;
    }

    private void AddToMovementBuffer(Vector2 vector) {
        if(_movementBuffer.Count == 0 && (vector == -movement || vector == movement)) {
            return;
        }
        if(_movementBuffer.Count != 0) {
            if(_movementBuffer.Last() == vector || _movementBuffer.Last() == -vector) {
                return;
            }
        }
        if(_movementBuffer.Count < _maxMovementBufferSize) {
            _movementBuffer.Add(vector);
        }
    }
}