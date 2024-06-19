using System.Numerics;
using Raylib_cs;

public class MainScene : IScene
{
    public const int TileSize = 32;
    private readonly int _rows;
    private readonly int _columns;
    private readonly float _scaleFactor;
    private Camera2D _camera;
    private float _timeUntilTicks = 0.5f;
    private float _tickTimeCount = 0f;

    public Color ClearColor => Color.DarkGray;
    public int Rows => _rows;
    public int Columns => _columns;

    public static Color[] CellColors =>[
        Color.DarkGreen,
        Color.Green
    ];

    private List<Player> _players = [];

    public MainScene(int rows, int columns) {
        _rows = Math.Max(1, rows);
        _columns = Math.Max(1, columns);
        float height = (_rows + 2f) * TileSize;
        _scaleFactor = Raylib.GetScreenHeight() / height;
        _camera = new Camera2D() {
            Target = -Vector2.One * TileSize,
            Offset = Vector2.Zero,
            Zoom = _scaleFactor,
            Rotation = 0f
        };
        _tickTimeCount = _timeUntilTicks;
        // Remove Later
        _players.Add(new Player(4, 2, this));
    }

    public void Start()
    {
        foreach(Player player in _players) player.Start();
    }

    public void Update()
    {
        Vector2 newMovement = new((float) (Raylib.IsKeyPressed(KeyboardKey.Right) - Raylib.IsKeyPressed(KeyboardKey.Left)), 0);
        if(newMovement.X == 0f) newMovement.Y = (float) (Raylib.IsKeyPressed(KeyboardKey.Down) - Raylib.IsKeyPressed(KeyboardKey.Up));
        _players[0].movement = newMovement;

        _tickTimeCount -= Raylib.GetFrameTime();
        if(_tickTimeCount <= 0f) {
            _tickTimeCount = _timeUntilTicks;
            // Update Objects here
            foreach(Player player in _players) player.Update();
        }
    }

    public void End()
    {
        
    }

    public void Render()
    {
        Raylib.BeginMode2D(_camera);

            for(int i = 0; i < Rows; i++) {
                for(int j = 0; j < Columns; j ++) {
                    int index = (i + j) % CellColors.Length;
                    Raylib.DrawRectangleV(new Vector2(j, i) * TileSize, Vector2.One * TileSize, CellColors[index]);
                }
            }
            foreach(Player player in _players) player.Render();

        Raylib.EndMode2D();

        Raylib.DrawFPS(0, 0);
    }

    public CellEntity? GetEntityInCell(int x, int y) {
        x = Math.Clamp(x, 0, Columns - 1);
        y = Math.Clamp(y, 0, Rows - 1);
        foreach(Player player in _players) {
            if(player.IsFillingCell(x, y)) return player;
        }
        return null;
    }
}