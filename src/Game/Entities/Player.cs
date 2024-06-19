using System.Numerics;
using Raylib_cs;

public class Player(int x, int y, MainScene parentScene) : CellEntity(x, y, parentScene) {
    
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
        int newX = x + (int) movement.X;
        int newY = y + (int) movement.Y;
        CellEntity? collision = scene.GetEntityInCell(newX, newY);
        if(collision == null) {
            x = newX;
            y = newY;
        }
        _lastMovement = _movement;
    }   

    public override void Render() {
        Raylib.DrawRectangle(x * MainScene.TileSize, y * MainScene.TileSize, MainScene.TileSize, MainScene.TileSize, Color.Blue);
    }
}