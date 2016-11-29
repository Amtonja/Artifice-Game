namespace Artiface.Interfaces {
    public interface ICombatable {
        Characters.CombatStats Stats { get; }
        bool InCombat { get; set; }
    }
}
