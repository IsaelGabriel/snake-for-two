using Raylib_cs;

public class GameOverScene(int winnerID) : IScene
{
    private int _winnerID = winnerID;
    public Color ClearColor => Color.Black;

    public void Render()
    {
        Raylib.DrawText($"Player {_winnerID + 1} has lost.", 80, Raylib.GetScreenHeight() / 2 - 50, 50, Color.Orange);
        Raylib.DrawText($"Press SPACE to try again.", 140, Raylib.GetScreenHeight() / 2 + 35, 20, Color.RayWhite);
    }
}