namespace JudgmentOfLostSouls.Manager
{
    public struct Points
    {
        public float karma;
        public float bless;

        public Points(float karma, float bless)
        {
            this.karma = karma;
            this.bless = bless;
        }
    }

    public class PlayerVariables : Node
    {
        public float Karma { get; private set; }
        public float Bless { get; private set; }

        public void AddPlayerValues(Points points)
        {
            Karma += points.karma;
            Bless += points.bless;
        }

        public void ResetValues()
        {
            Karma = 0;
            Bless = 0;
        }
    }
}