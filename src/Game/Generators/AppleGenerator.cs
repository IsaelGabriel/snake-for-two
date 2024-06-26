public class AppleGenerator(MainScene parentScene) : Generator(parentScene)
{
    private const int _powerUpChance = 5;

    public override void Update() {
        if(!scene.CellEntities.OfType<Item>().Any(e => e.type == ItemType.Apple || e.type == ItemType.PowerUp)) {
            GenerateApple();
        }
    }

    private void GenerateApple() {
        int x = 0;
        int y = 0;
        do {
            x = Global.RNG.Next() % scene.Columns;
            y = Global.RNG.Next() % scene.Rows;
        } while(scene.GetEntityInCell(x, y) != null);
        ItemType type = (Global.RNG.Next() % _powerUpChance != 0)? ItemType.Apple : ItemType.PowerUp;

        Item apple = new Item(type, x, y, scene);
        scene.AddEntity(apple);
        apple.Start();

    }
}