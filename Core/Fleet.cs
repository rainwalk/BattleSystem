using System.Collections.Generic;

public class Fleet
{
    public string Name { get; private set; }
    public List<Ship> Ships { get; private set; }
    public Fleet(string name) {
        Name = name;
        Ships = new List<Ship>();
    }
    public void AddShip(Ship ship) => Ships.Add(ship);
    public bool IsAlive() => Ships.Exists(ship => ship.IsAlive());
    public Ship FindTarget() => Ships.Find(ship => ship.IsAlive());
}
