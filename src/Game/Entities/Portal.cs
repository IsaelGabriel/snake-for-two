using System.Numerics;
using Raylib_cs;

public class Portal(Vector2[] points, MainScene parentScene) : CellEntity(0, 0, parentScene)
{
    public Vector2[] points = points;

    public override void Render()
    {
        Rectangle rect = new(new(0, 0), Vector2.One * MainScene.TileSize);
        foreach(Vector2 point in points) {
            rect.Position = point * MainScene.TileSize;
            Raylib.DrawRectangleLinesEx(rect, 3, Color.RayWhite);
        }
    }

    public override bool IsFillingCell(int x, int y) {
        return points.Contains(new(x, y));
    }
}