using System.Numerics;
using Raylib_cs;

public class Player(uint ID, int x, int y, MainScene parentScene, float movementInterval) : CellEntity(x, y, parentScene) {
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
    private List<Vector2> sections = [new(x, y+1), new(x, y+2)];

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
                    default:
                        scene.Destroy(collision);
                    break;
                }
                
            }else if(collision is Player) {
                solidCollision = true;
                Game.LoadScene(new GameOverScene(_ID));
            }
        }
        if(!solidCollision) {
            int oldX = x;
            int oldY = y;
            x = newX;
            y = newY;
            if(x != oldX || y != oldY) {
                sections.RemoveAt(sections.Count - 1);
                sections.Insert(0, new Vector2(oldX, oldY));
            }else {
                Game.LoadScene(new GameOverScene(_ID));
            }
        }
        _lastMovement = _movement;
        _movementIntervalCount = Math.Clamp(_movementInterval - (sections.Count - 2) * 0.01f, 0.025f, _movementInterval);
    }   

    public override void Render() {
        Raylib.DrawRectangle(x * MainScene.TileSize, y * MainScene.TileSize, MainScene.TileSize, MainScene.TileSize, _headColor);
        foreach(Vector2 section in sections) {
            Raylib.DrawRectangleV(section * MainScene.TileSize, Vector2.One * MainScene.TileSize, _bodyColor);
        }
    
    }

    public override bool IsFillingCell(int x, int y)
    {
        if(this.x == x && this.y == y) return true;
        return sections.Contains(new Vector2(x, y));
    }
}