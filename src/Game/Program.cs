using Raylib_cs;

public class Game {
    private static Game _instance;
    
    private const string _windowTitle = "Snake for Two";   
    public static int WindowWidth => 600;
    public static int WindowHeight => 600;
    public static int TargetFPS => 120;
    
    
    private IScene? _sceneToLoad = null;
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

    public static void LoadScene(IScene scene) {
        if(_instance._sceneToLoad != null) return;
        _instance._sceneToLoad = scene;
    }

    private void Start() {
        _currentScene = new GameOverScene(0);
        _currentScene.Start();
    }

    private void GameLoop() {
        while(!Raylib.WindowShouldClose()) {
            Update();
            Render();
            if(_sceneToLoad != null) {
                _currentScene.End();
                _currentScene = _sceneToLoad;
                _sceneToLoad = null;
                _currentScene.Start();
            }
        }
    }

    private void Update() {
        _currentScene.Update();
    }

    private void Render() {
        Raylib.BeginDrawing();
            Raylib.ClearBackground(_currentScene.ClearColor);
            _currentScene.Background?.Render();
            _currentScene.Render();
        Raylib.EndDrawing();
    }

    private void End() {
        _currentScene.End();
        Raylib.CloseWindow();
    }

}