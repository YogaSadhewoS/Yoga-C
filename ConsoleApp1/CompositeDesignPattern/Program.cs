using System;

//Tanpa Composite
public class Item
{
    public string Name { get; set; }
    public void Display() => Console.WriteLine(Name);
}

public class Group
{
    private List<Item> items = new List<Item>();
    
    public void Add(Item item) => items.Add(item);
    public void Display()
    {
        foreach (var item in items)
        {
            item.Display();
        }
    }
}

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public void Display() => 
        Console.WriteLine($"Weapon: {Name}, Damage: {Damage}");
}

public class MainGame
{
    public static void Main(string[] args)
    {
        Item scroll = new Item { Name = "Ancient Scroll" };
        Weapon axe = new Weapon { Name = "Axe", Damage = 1500 };
        Group inventory = new Group();
        
        inventory.Add(scroll);
        // Tidak bisa karena Weapon bukan Item
        // inventory.Add(axe);
        

        // Harus dipanggil secara terpisah
        axe.Display();
        inventory.Display();
    }
}

//Dengan composite
/*
public abstract class Component
{
    public string Name { get; set; }
    public abstract void Display();
}

public class Item : Component
{
    public override void Display() => Console.WriteLine($"Item: {Name}");
}

public class Group : Component
{
    private List<Component> components = new List<Component>();
    
    public void Add(Component component) => components.Add(component);
    public void Remove(Component component) => components.Remove(component);

    public override void Display()
    {
        Console.WriteLine($"Group: {Name}");
        foreach (var component in components)
        {
            component.Display();
        }
    }
}

public class Weapon : Component
{
    public int Damage { get; set; }
    public override void Display() => 
        Console.WriteLine($"Weapon: {Name}, Damage: {Damage}");
}

public class MainGame
{
    public static void Main(string[] args)
    {
        Item scroll = new Item { Name = "Ancient Scroll" };
        Weapon axe = new Weapon { Name = "Axe", Damage = 1500 };
        Group inventory = new Group { Name = "Hero's Equipment" };
        
        inventory.Add(scroll);
        inventory.Add(axe);

        inventory.Display();
    }
}*/

