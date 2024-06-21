using System.Numerics;
using System.Text.RegularExpressions;
using Raylib_cs;

public class Player(int x, int y, MainScene parentScene, float movementInterval) : CellEntity(x, y, parentScene) {
    
    private float _movementInterval = movementInterval;
    private float _movementIntervalCount = movementInterval;
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
    private Color color = Color.Blue;
    private List<Vector2> sections = [new(x, y+1), new(x, y+2)];

    public override void Update() {
        Vector2 newMovement = new((float) (Raylib.IsKeyPressed(KeyboardKey.Right) - Raylib.IsKeyPressed(KeyboardKey.Left)), 0);
        if(newMovement.X == 0f)
            newMovement.Y = (float) (Raylib.IsKeyPressed(KeyboardKey.Down) - Raylib.IsKeyPressed(KeyboardKey.Up));
        movement = newMovement;

        _movementIntervalCount -= Raylib.GetFrameTime();

        if(_movementIntervalCount > 0f) return;

        int newX = x + (int) movement.X;
        int newY = y + (int) movement.Y;
        CellEntity? collision = scene.GetEntityInCell(newX, newY);
        bool solidCollision = false;
        if(collision != null) {
            if(collision is Apple) {
                sections.Add(sections[sections.Count - 1]);
                scene.Destroy(collision);
            }else if(collision == this) {
                solidCollision = true;
                Game.LoadScene(new GameOverScene(0));
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
                Game.LoadScene(new GameOverScene(0));
            }
        }
        _lastMovement = _movement;
        _movementIntervalCount = Math.Clamp(_movementInterval - (sections.Count - 2) * 0.01f, 0.025f, _movementInterval);
    }   

    public override void Render() {
        Raylib.DrawRectangle(x * MainScene.TileSize, y * MainScene.TileSize, MainScene.TileSize, MainScene.TileSize, Color.Blue);
        foreach(Vector2 section in sections) {
            Raylib.DrawRectangleV(section * MainScene.TileSize, Vector2.One * MainScene.TileSize, Color.SkyBlue);
        }
    
    }

    public override bool IsFillingCell(int x, int y)
    {
        if(this.x == x && this.y == y) return true;
        return sections.Contains(new Vector2(x, y));
    }
}