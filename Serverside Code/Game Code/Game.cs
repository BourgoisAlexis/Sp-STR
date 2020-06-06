using System;
using System.Linq;
using PlayerIO.GameLibrary;


public class Player : BasePlayer
{
}

[RoomType("Lobby")]
public class LobbyCode : Game<Player>
{
	public override void GameStarted()
	{
		Console.WriteLine("Game is started: " + RoomId);
	}

	public override void GameClosed()
	{
		Console.WriteLine("RoomId: " + RoomId);
	}


	public override void UserJoined(Player player)
	{
		foreach (Player pl in Players)
			if (pl.ConnectUserId != player.ConnectUserId)
				pl.Send("PlayerJoined", Players.Count());
	}

	public override void UserLeft(Player player)
	{
		Broadcast("PlayerLeft", player.ConnectUserId);
	}


	public override void GotMessage(Player player, Message m)
	{
		switch (m.Type)
		{
			case "PlayerJoined":
				Broadcast("PlayerJoined", Players.Count());
				break;
		}
	}
}


[RoomType("Versus")]
public class GameCode : Game<Player>
{
	public override void GameStarted() 
	{
		Console.WriteLine("Game is started: " + RoomId);
	}

	public override void GameClosed() 
	{
		Console.WriteLine("RoomId: " + RoomId);
	}


	public override void UserJoined(Player player) 
	{
		player.Send("SetupPlayer", Players.Count());

		foreach (Player pl in Players)
			if (pl.ConnectUserId != player.ConnectUserId)
				pl.Send("PlayerJoined", Players.Count());
	}

	public override void UserLeft(Player player) 
	{
		Broadcast("PlayerLeft", player.ConnectUserId);
	}


	public override void GotMessage(Player player, Message m) {
		switch(m.Type) 
		{
			case "PlayerJoined":
				Broadcast("PlayerJoined", Players.Count());
				break;

			case "SetUnitDestination":
				foreach (Player pl in Players)
					if (pl.ConnectUserId != player.ConnectUserId)
						pl.Send("SetUnitDestination", m.GetInt(0), m.GetFloat(1), m.GetFloat(2), m.GetFloat(3));
				break;

			case "SetUnitTarget":
				foreach (Player pl in Players)
					if (pl.ConnectUserId != player.ConnectUserId)
						pl.Send("SetUnitTarget", m.GetInt(0), m.GetInt(1));
				break;

			case "SpawnUnit":
				foreach (Player pl in Players)
					if (pl.ConnectUserId != player.ConnectUserId)
						pl.Send("SpawnUnit", m.GetInt(0), m.GetFloat(1), m.GetFloat(2), m.GetFloat(3), m.GetInt(4));
				break;

			case "DestroyUnit":
				foreach (Player pl in Players)
					if (pl.ConnectUserId != player.ConnectUserId)
						pl.Send("DestroyUnit", m.GetInt(0));
				break;

			case "UpdateHealth":
				foreach (Player pl in Players)
					if (pl.ConnectUserId != player.ConnectUserId)
						pl.Send("UpdateHealth", m.GetInt(0), m.GetInt(1));
				break;
		}
	}
}
