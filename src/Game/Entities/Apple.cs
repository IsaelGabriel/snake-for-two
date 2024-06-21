using Raylib_cs;

public class Apple(int x, int y, MainScene parentScene) : CellEntity(x, y, parentScene)
{
    public override void Start()
    {
        base.Start();
    }

    public override void Render()
    {
        Raylib.DrawRectangle(x * MainScene.TileSize + 4, y * MainScene.TileSize + 4, MainScene.TileSize - 8, MainScene.TileSize - 8, Color.Red);
    }
}