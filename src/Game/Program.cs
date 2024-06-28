using Raylib_cs;

public static class Game {
    
    private const string _windowTitle = "Snake for Two";   
    public static int WindowWidth => 600;
    public static int WindowHeight => 600;
    public static int TargetFPS => 120;
    
    
    private static IScene? _sceneToLoad = null;
    private static IScene _currentScene = new GameOverScene(0);

    static void Main() {
        TraceLogLevel traceLog = TraceLogLevel.Fatal;
        #if DEBUG
            traceLog = TraceLogLevel.All;
        #endif
        Raylib.SetTraceLogLevel(traceLog);
        Raylib.InitWindow(WindowWidth, WindowHeight, _windowTitle);
        Raylib.SetTargetFPS(TargetFPS);
        Raylib.SetExitKey(KeyboardKey.Null);
        Start();
        GameLoop();
        End();
    }

    public static void LoadScene(IScene scene) {
        if(_sceneToLoad != null) return;
        _sceneToLoad = scene;
    }

    private static void Start() {
        _currentScene.Start();
    }

    private static void GameLoop() {
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

    private static void Update() {
        _currentScene.Update();
    }

    private static void Render() {
        Raylib.BeginDrawing();
            Raylib.ClearBackground(_currentScene.ClearColor);
            _currentScene.Background?.Render();
            _currentScene.Render();
        Raylib.EndDrawing();
    }

    private static void End() {
        _currentScene.End();
        Raylib.CloseWindow();
    }

}