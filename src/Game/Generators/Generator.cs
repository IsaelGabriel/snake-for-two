public abstract class Generator(MainScene parentScene) {
    protected readonly MainScene scene = parentScene;

    public virtual void Start() {}
    public virtual void Update() {}
}