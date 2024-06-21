using Raylib_cs;

public enum Action: int {
    Up,
    Down,
    Left,
    Right
}

public static class PlayerInputManager {
    private static readonly Dictionary<Action,KeyboardKey>[] Keys = [
        new(){
            { Action.Up, KeyboardKey.Up},
            { Action.Down, KeyboardKey.Down},
            { Action.Left, KeyboardKey.Left},
            { Action.Right, KeyboardKey.Right}
        },
        new(){
            { Action.Up, KeyboardKey.W},
            { Action.Down, KeyboardKey.S},
            { Action.Left, KeyboardKey.A},
            { Action.Right, KeyboardKey.D}
        }
    ];

    public static bool IsActionPressed(int ID, Action action) {
        if(ID < 0 || ID >= Keys.Length) return false; 
        if(!Keys[ID].ContainsKey(action)) return false;
        return Raylib.IsKeyPressed(Keys[ID][action]);
    }

}