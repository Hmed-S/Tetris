using Newtonsoft.Json;
using System;

namespace TetrisServer.Data.Entities;

[JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
public class Player
{
    public string Id { get; set; }
    public DateTime TimeJoined { get; set; }
    public string Name { get; set; }
    public Player Opponent { get; set; }
}
