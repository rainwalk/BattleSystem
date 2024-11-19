using System.Collections.Generic;

public enum WeaponType
{
    Cannon,
    Missile,
    Beam,
    Fighter
}

public class Weapon
{
    private static readonly Dictionary<WeaponType, (int damage, float fireRate)> weaponStats = new Dictionary<WeaponType, (int, float)>
    {
        { WeaponType.Cannon, (3, 2) },
        { WeaponType.Missile, (6, 3) },
        { WeaponType.Beam, (9, 4) },
        { WeaponType.Fighter, (12, 5) }
    };

    public WeaponType Type { get; private set; }
    public float FireRate { get; private set; }
    public int Damage { get; private set; }

    public Weapon(WeaponType type) {
        Type = type;
        (Damage, FireRate) = weaponStats[type];
    }
}