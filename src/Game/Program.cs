using Raylib_cs;

public class Game {
    private static Game _instance;
    
    private const string _windowTitle = "Snake for Two";   
    public static int WindowWidth => 600;
    public static int WindowHeight => 600;
    public static int TargetFPS => 30;


    private Game() {
        _instance = this;
        Raylib.InitWindow(WindowWidth, WindowHeight, _windowTitle);
        Raylib.SetTargetFPS(TargetFPS);
    }

    static void Main() {
        new Game();
        _instance.GameLoop();
        _instance.End();

    }

    private void GameLoop() {
        while(!Raylib.WindowShouldClose()) {
            Update();
            Render();
        }
    }

    private void Update() {

    }

    private void Render() {
        Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Beige);
        Raylib.EndDrawing();
    }

    private void End() {
        Raylib.CloseWindow();
    }

}