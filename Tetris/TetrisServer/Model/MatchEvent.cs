using Newtonsoft.Json;
using TetrisServer.Data.Entities;

namespace TetrisServer.Model;

[JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
public record MatchEvent(Player Player1, Player Player2, int seed)
{
}
