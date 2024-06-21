using Raylib_cs;

public interface IScene
{
    public Color ClearColor { get; }
    public IBackground? Background { get; }

    public void Start() { }
    public void Update() { }
    public void End() { }
    public void Render() { }

}
