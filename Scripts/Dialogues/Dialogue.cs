namespace JudgmentOfLostSouls.Dialogues
{
    public abstract class Dialogue : Control
    {
        protected readonly Queue<Dictionary> QueueDialogues = new Queue<Dictionary>();

        [Export(PropertyHint.Dir)] public string CharacterImagesPath;

        [Export(PropertyHint.Dir)] public string JsonPath;

        public abstract string NodePath { get; }
        protected abstract string JsonName { get; }

        protected GameManager GameManager { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            GameManager = GetNode<GameManager>("/root/GameManager");
            AddDialogueDictionary(QueueDialogues, JsonPath, JsonName);
        }


        private void AddDialogueDictionary(Queue<Dictionary> queue, string jsonPath, string dialogueType)
        {
            var dictionary = GameManager.LoadDialogueFromJson(jsonPath, dialogueType);
            for (var i = 0; i < dictionary.Count; i++)
            {
                var dialogue = dictionary[i.ToString()] as Dictionary;
                queue.Enqueue(dialogue);
            }
        }
    }
}