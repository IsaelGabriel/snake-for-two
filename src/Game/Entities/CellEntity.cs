public abstract class CellEntity(int x, int y, MainScene parentScene) {
    private int _x = x, _y = y;
    protected readonly MainScene scene = parentScene;
    public int x {get=>_x; set=>_x=Math.Clamp(value, 0, scene.Columns - 1); }
    public int y {get=>_y; set=>_y=Math.Clamp(value, 0, scene.Rows - 1); }


    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void Render() {}
    public virtual bool IsFillingCell(int x, int y) {
        return (this.x == x) && (this.y == y);
    }

}