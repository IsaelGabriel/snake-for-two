using Raylib_cs;

public class Game {
    private static Game _instance;
    
    private const string _windowTitle = "Snake for Two";   
    public static int WindowWidth => 600;
    public static int WindowHeight => 600;
    public static int TargetFPS => 30;
    
    private IScene _currentScene;

    private Game() {
        _instance = this;
        Raylib.InitWindow(WindowWidth, WindowHeight, _windowTitle);
        Raylib.SetTargetFPS(TargetFPS);
        Raylib.SetExitKey(KeyboardKey.Null);
    }

    static void Main() {
        new Game();
        _instance.Start();
        _instance.GameLoop();
        _instance.End();

    }

    public void LoadScene(IScene scene) {
        _currentScene.End();
        _currentScene = scene;
        _currentScene.Start();
    }

    private void Start() {
        _currentScene = new MainScene(5, 5);
        _currentScene.Start();
    }

    private void GameLoop() {
        while(!Raylib.WindowShouldClose()) {
            Update();
            Render();
        }
    }

    private void Update() {
        _currentScene.Update();
    }

    private void Render() {
        Raylib.BeginDrawing();
            Raylib.ClearBackground(_currentScene.ClearColor);
            _currentScene.Render();
        Raylib.EndDrawing();
    }

    private void End() {
        _currentScene.End();
        Raylib.CloseWindow();
    }

}