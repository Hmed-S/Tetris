namespace TetrisClient
{
    internal class GameMode
    {
        private string Name { get; set; }    
        private static GameMode _instance  = new() { Name="Singleplayer"};


        public static string GetGameMode() => _instance.Name;
        public static void SetGameMode(string name) => _instance.Name = name;
    }
}
