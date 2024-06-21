using System.Numerics;
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


    public override void Update() {
        Vector2 newMovement = new((float) (Raylib.IsKeyPressed(KeyboardKey.Right) - Raylib.IsKeyPressed(KeyboardKey.Left)), 0);
        if(newMovement.X == 0f)
            newMovement.Y = (float) (Raylib.IsKeyPressed(KeyboardKey.Down) - Raylib.IsKeyPressed(KeyboardKey.Up));
        if(newMovement != Vector2.Zero && newMovement != -movement) {
            movement = newMovement;
        }

        _movementIntervalCount -= Raylib.GetFrameTime();

        if(_movementIntervalCount > 0f) return;

        int newX = x + (int) movement.X;
        int newY = y + (int) movement.Y;
        CellEntity? collision = scene.GetEntityInCell(newX, newY);
        if(collision == null) {
            x = newX;
            y = newY;
        }
        _lastMovement = _movement;
        _movementIntervalCount = _movementInterval;
    }   

    public override void Render() {
        Raylib.DrawRectangle(x * MainScene.TileSize, y * MainScene.TileSize, MainScene.TileSize, MainScene.TileSize, Color.Blue);
    }
}