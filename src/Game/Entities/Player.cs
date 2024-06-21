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

    private float _movementInterval = movementInterval;
    private float _movementIntervalCount = movementInterval;
    private readonly uint _ID = ID;
    private Vector2 _lastMovement = new(0, -1);
    private Vector2 _movement = new(0, -1);
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
        for(int i = 1; i <= _defaultLength; i++) {
            sections.Add(new(x, y+i));
        }
    }

    public override void Update() {
        Vector2 newMovement = new(Input.GetAxisPress(_ID, Action.Left, Action.Right), 0);
        if(newMovement.X == 0f)
            newMovement.Y = Input.GetAxisPress(_ID, Action.Up, Action.Down);
        movement = newMovement;

        _movementIntervalCount -= Raylib.GetFrameTime();

        if(_movementIntervalCount > 0f) return;

        int newX = x + (int) movement.X;
        int newY = y + (int) movement.Y;
        CellEntity? collision = scene.GetEntityInCell(newX, newY);
        bool solidCollision = false;
        if(collision != null) {
            if(collision is Item) {
                switch(((Item)collision).type) {
                    case ItemType.Apple:
                        sections.Add(sections[sections.Count - 1]);
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
                    if(collision.x == x && collision.y == y) Game.LoadScene(new GameOverScene(_ID));
                    else {
                        Player enemy = (Player) collision;
                        int index = enemy.IndexOfSection(newX, newY);
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
            if((x != oldX || y != oldY) && sections.Count > 0) {
                sections.RemoveAt(sections.Count - 1);
                sections.Insert(0, new Vector2(oldX, oldY));
            }else {
                Game.LoadScene(new GameOverScene(_ID));
            }
        }
        _lastMovement = _movement;
        _movementIntervalCount = Math.Clamp(_movementInterval - (sections.Count - 2) * 0.01f, 0.025f, _movementInterval);
        if(item == ItemType.PowerUp) _movementIntervalCount *= 0.5f;
    }   

    public override void Render() {
        Vector2 position = new Vector2(x, y) * MainScene.TileSize;
        Vector2 size = Vector2.One * MainScene.TileSize;
        Raylib.DrawRectangleV(position, size, _headColor);
        if(item == ItemType.PowerUp) {
            Raylib.DrawRectangleV(position + Vector2.One * 3, size - Vector2.One * 6, _powerUpColor);
        }
        foreach(Vector2 section in sections) {
            Raylib.DrawRectangleV(section * MainScene.TileSize, Vector2.One * MainScene.TileSize, _bodyColor);
        }
    
    }

    public override bool IsFillingCell(int x, int y)
    {
        if(this.x == x && this.y == y) return true;
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
}