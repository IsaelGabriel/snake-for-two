using System.Numerics;
using Raylib_cs;

public class SquarePatternBackground(int squareSize, float scaleFactor) : IBackground
{
    private float _offset = 0f;
    private readonly int _squareSize = squareSize;
    private readonly float _scaleFactor = scaleFactor;
    public Color color = Color.DarkPurple;
    private readonly float _columns = Raylib.GetScreenWidth() / squareSize * scaleFactor;
    private readonly float _rows = Raylib.GetScreenHeight() / squareSize * scaleFactor;

    public void Render()
    {
        /*_offset += Raylib.GetFrameTime() * 0.25f;
        if(_offset > 2f) _offset = 0f;*/
        Vector2 size = Vector2.One * _squareSize;
        for(int i = 0; i < _rows + 2; i++) {
            for(int j = 0; j < _columns + 2; j++) {
                if((i+j) % 2 != 0) continue;
                Vector2 position = new(j * _squareSize, i * _squareSize);
                position.X -= _squareSize * _offset;
                position.Y -= _squareSize * _offset;
                Raylib.DrawRectangleV(position * _scaleFactor, size * _scaleFactor, color);
            }
        }
    }
}