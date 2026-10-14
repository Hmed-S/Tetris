using Newtonsoft.Json;

namespace TetrisServer.Data.Entities;

[JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
public class Player
{
    public string Id { get; set; }
    public string TimeJoined { get; set; }
    public string Name { get; set; }
    public Player Opponent { get; set; }
}
