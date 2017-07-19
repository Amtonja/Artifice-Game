namespace Artifice.Interfaces {
    public interface ICombatable {
        void EnterCombat();
        void ExitCombat();

        Characters.CharacterStats Stats { get; }
        bool InCombat { get; set; }
    }
}
