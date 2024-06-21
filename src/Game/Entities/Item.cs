using System.Numerics;
using Raylib_cs;

public enum ItemType {
    Apple
}

public class Item(ItemType itemType, int x, int y, MainScene parentScene) : CellEntity(x, y, parentScene)
{
    public readonly ItemType type = itemType;

    public override void Start()
    {
        base.Start();
    }

    public override void Render()
    {
        Vector2 position = new Vector2(x, y) * MainScene.TileSize + Vector2.One * 4;
        Vector2 size = Vector2.One * (MainScene.TileSize - 8);
        Color color = Color.Gray;
        switch(type) {
            case ItemType.Apple:
                color = Color.Red;
                break;
            default: break;
        }
        Raylib.DrawRectangleV(position, size, color);
    }
}