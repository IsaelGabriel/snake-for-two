using System.Numerics;
using Raylib_cs;

public class MainScene : IScene
{
    private const int _defaultRows = 15;
    private const int _defaultColumns = 15;
    private const float _defaultPlayerMovementInterval = 0.75f;
    private const float _margin = 2f;

    public const int TileSize = 16;
    private readonly int _rows;
    private readonly int _columns;
    private readonly float _scaleFactor;
    private Camera2D _camera;
    private List<CellEntity> _deleteList = [];
    private List<CellEntity> _addList = [];
    private readonly Random _rng = new Random();
    private IBackground _background;

    public List<CellEntity> CellEntities = [];
    private readonly List<Generator> _generators; 
    public Color ClearColor => Color.Beige;
    public int Rows => _rows;
    public int Columns => _columns;

    public readonly static Color[] CellColors = [
        new(0x53, 0xA6, 0x5f, 0xFF),
        new(0x07, 0x54, 0x30, 0xFF)
    ];

    public IBackground Background => _background;

    private MainScene(int rows, int columns) {
        _rows = Math.Max(1, rows);
        _columns = Math.Max(1, columns);
        float height = (_rows + _margin * 2) * TileSize;
        _scaleFactor = Raylib.GetScreenHeight() / height;
        _camera = new Camera2D() {
            Target = -Vector2.One * TileSize * _margin,
            Offset = Vector2.Zero,
            Zoom = _scaleFactor,
            Rotation = 0f
        };
        _background = new SquarePatternBackground(TileSize * 2, _scaleFactor);
        _generators = [
            new AppleGenerator(this)
        ];
    }

    public static MainScene GenerateMainScene(int rows, int columns) {
        return new MainScene(rows, columns);
    }

    public static MainScene GenerateMainScene() {
        return new MainScene(_defaultRows, _defaultColumns);
    }

    public void Start()
    {
        foreach(CellEntity entity in CellEntities) entity.Start();
        foreach(Generator generator in _generators) generator.Start();
        AddEntity(new Player(0, Columns - 1, Rows / 2 - 1, this, _defaultPlayerMovementInterval));
        AddEntity(new Player(1, 0, Rows / 2 - 1, this, _defaultPlayerMovementInterval));
        
    }

    public void Update()
    {
        foreach(CellEntity entity in _addList) {
            CellEntities.Add(entity);
            entity.Start();
        }
        _addList = [];
        foreach(CellEntity entity in CellEntities) entity.Update();
        foreach(CellEntity entity in _deleteList) {
            CellEntities.RemoveAll(e => e == entity);
        }
        _deleteList = [];
        
        foreach(Generator generator in _generators) generator.Update();
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
            foreach(CellEntity entity in CellEntities) entity.Render();

        Raylib.EndMode2D();

        Raylib.DrawFPS(0, 0);
    }

    public CellEntity? GetEntityInCell(int x, int y) {
        x = Math.Clamp(x, 0, Columns - 1);
        y = Math.Clamp(y, 0, Rows - 1);
        foreach(CellEntity entity in CellEntities) {
            if(entity.IsFillingCell(x, y)) return entity;
        }
        return null;
    }

    public void AddEntity(CellEntity entity) {
        _addList.Add(entity);
    }

    public void Destroy(CellEntity entity) {
        _deleteList.Add(entity);
    }
}