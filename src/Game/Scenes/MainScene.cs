using System.Numerics;
using Raylib_cs;

public class MainScene : IScene
{
    private const int _defaultRows = 20;
    private const int _defaultColumns = 20;
    private const float _defaultPlayerMovementInterval = 0.75f;

    public const int TileSize = 16;
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

    private List<CellEntity> _cellEntities = [];
    private List<CellEntity> _deleteList = [];
    private Random _rng = new Random();

    private MainScene(int rows, int columns) {
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
    }

    public static MainScene GenerateMainScene(int rows, int columns) {
        return new MainScene(rows, columns);
    }

    public static MainScene GenerateMainScene() {
        return new MainScene(_defaultRows, _defaultColumns);
    }

    public void Start()
    {
        foreach(CellEntity entity in _cellEntities) entity.Start();
        AddEntity(new Player(Columns - 1, Rows / 2 - 1, this, _defaultPlayerMovementInterval));

    }

    public void Update()
    {

        foreach(CellEntity entity in _cellEntities) entity.Update();
        foreach(CellEntity entity in _deleteList) {
            _cellEntities.RemoveAll(e => e == entity);
        }
        _deleteList = [];

        if(!_cellEntities.OfType<Item>().Any(i => i.type == ItemType.Apple)) GenerateApple();
    }

    private void GenerateApple() {
        int x = 0;
        int y = 0;
        do {
            x = _rng.Next() % Columns;
            y = _rng.Next() % Rows;
        } while(GetEntityInCell(x, y) != null);
        Item apple = new Item(ItemType.Apple, x, y, this);
        _cellEntities.Add(apple);
        apple.Start();

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
            foreach(CellEntity entity in _cellEntities) entity.Render();

        Raylib.EndMode2D();

        Raylib.DrawFPS(0, 0);
    }

    public CellEntity? GetEntityInCell(int x, int y) {
        x = Math.Clamp(x, 0, Columns - 1);
        y = Math.Clamp(y, 0, Rows - 1);
        foreach(CellEntity entity in _cellEntities) {
            if(entity.IsFillingCell(x, y)) return entity;
        }
        return null;
    }

    public void AddEntity(CellEntity entity) {
        _cellEntities.Add(entity);
        entity.Start();
    }

    public void Destroy(CellEntity entity) {
        _deleteList.Add(entity);
    }
}