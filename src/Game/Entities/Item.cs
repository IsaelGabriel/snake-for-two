using System.Numerics;
using Raylib_cs;

public enum ItemType {
    Apple,
    PowerUp,
}

public class Item(ItemType itemType, int x, int y, MainScene parentScene) : CellEntity(x, y, parentScene)
{
    private const float _maxOffsetY = (float) 0.75f;
    private const float _animationDuration = 1f;
    public readonly ItemType type = itemType;
    private float _animationCount = Raylib.GetFrameTime();

    public override void Start()
    {
        base.Start();
    }

    public override void Render()
    {
        Vector2 size = Vector2.One * (MainScene.TileSize - 8);
        Color color = Color.Gray;
        switch(type) {
            case ItemType.Apple:
                color = Color.Red;
                break;
            case ItemType.PowerUp:
                color = Color.Gold;
                break;
            default: break;
        }
        Raylib.DrawRectangleV(GetRenderPosition(), size, color);
    }

    private Vector2 GetRenderPosition() {
        _animationCount += Raylib.GetFrameTime();
        if(_animationCount > _animationDuration) _animationCount = 0f;

        float t = _animationCount / _animationDuration;

        Vector2 position = new Vector2(x, y) * MainScene.TileSize + Vector2.One * 4;
        
        position.Y -= _maxOffsetY * (float) Math.Sin(t  * (float) Math.PI * 2f) * (1 - t / 2);

        return position;
    }
}