using System.Numerics;
using Raylib_cs;

public class MainScene : IScene
{
    public const int TileSize = 32;
    private readonly int _rows;
    private readonly int _columns;
    private readonly float _scaleFactor;
    private Camera2D _camera;

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
        // Remove Later
        _players.Add(new Player(4, 2, this, 0.8f));
    }

    public void Start()
    {
        foreach(Player player in _players) player.Start();
    }

    public void Update()
    {

        foreach(Player player in _players) player.Update();
        
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