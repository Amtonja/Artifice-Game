namespace Artifice.Interfaces {
    public interface ICombatable {
        void EnterCombat();
        void ExitCombat();

        Characters.CombatStats Stats { get; }
        bool InCombat { get; set; }
    }
}
