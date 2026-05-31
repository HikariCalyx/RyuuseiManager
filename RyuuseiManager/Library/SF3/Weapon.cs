namespace RyuuseiManager.Library.SF3
{
    public class Weapon
    {
        public Weapon()
        {
            Attack = 1;
            Rapid = 1;
            Charge = 1;
        }

        public WeaponType Type { get; set; }
        public int Attack { get; set; }
        public int Rapid { get; set; }
        public int Charge { get; set; }
    }
}
