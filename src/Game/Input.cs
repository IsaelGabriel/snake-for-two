using Raylib_cs;

public enum Action: int {
    Up,
    Down,
    Left,
    Right
}

public static class Input {
    private static readonly Dictionary<Action,KeyboardKey>[] Keys = [
        new(){
            { Action.Up, KeyboardKey.W},
            { Action.Down, KeyboardKey.S},
            { Action.Left, KeyboardKey.A},
            { Action.Right, KeyboardKey.D}
        },
        new(){
            { Action.Up, KeyboardKey.Up},
            { Action.Down, KeyboardKey.Down},
            { Action.Left, KeyboardKey.Left},
            { Action.Right, KeyboardKey.Right}
        }
    ];

    public static bool IsActionPressed(uint ID, Action action) {
        if(ID < 0 || ID >= Keys.Length) return false; 
        if(!Keys[ID].ContainsKey(action)) return false;
        return Raylib.IsKeyPressed(Keys[ID][action]);
    }

    public static int GetAxisPress(uint ID, Action negativeAction, Action positiveAction) {
        int positive = IsActionPressed(ID, positiveAction)? 1 : 0;
        int negative = IsActionPressed(ID, negativeAction)? 1 : 0;
        return positive - negative;
    }

}