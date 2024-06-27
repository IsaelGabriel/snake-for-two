using System.Numerics;
using Raylib_cs;

public class Portal(Vector2[] points, MainScene parentScene) : CellEntity(0, 0, parentScene)
{
    private readonly Color[] _colors = [
        Color.Blue,
        Color.Orange,
        Color.Green,
        Color.Purple
    ];

    public Vector2[] points = points;

    public override void Render()
    {
        Rectangle rect = new(new(0, 0), Vector2.One * MainScene.TileSize);
        for(int i = 0; i < points.Length; i++) {
            rect.Position = points[i] * MainScene.TileSize;
            Raylib.DrawRectangleLinesEx(rect, 3, _colors[i % _colors.Length]);
        }
    }

    public override bool IsFillingCell(int x, int y) {
        return points.Contains(new(x, y));
    }
}