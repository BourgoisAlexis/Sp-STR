using System;
using System.Linq;
using PlayerIO.GameLibrary;

namespace MushroomsUnity3DExample 
{
	public class Player : BasePlayer 
	{
		public int team;
	}


	[RoomType("Lobby")]
	public class LobbyCode : Game<Player>
	{

		// This method is called when an instance of your the game is created
		public override void GameStarted()
		{
			Console.WriteLine("Game is started: " + RoomId);
		}

		// This method is called when the last player leaves the room, and it's closed down.
		public override void GameClosed()
		{
			Console.WriteLine("RoomId: " + RoomId);
		}

		// This method is called whenever a player joins the game
		public override void UserJoined(Player player)
		{
			foreach (Player pl in Players)
				if (pl.ConnectUserId != player.ConnectUserId)
					pl.Send("PlayerJoined", Players.Count());
		}

		// This method is called when a player leaves the game
		public override void UserLeft(Player player)
		{
			Broadcast("PlayerLeft", player.ConnectUserId);
		}

		// This method is called when a player sends a message into the server code
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

		// This method is called when an instance of your the game is created
		public override void GameStarted() 
		{
			Console.WriteLine("Game is started: " + RoomId);
		}

		// This method is called when the last player leaves the room, and it's closed down.
		public override void GameClosed() 
		{
			Console.WriteLine("RoomId: " + RoomId);
		}

		// This method is called whenever a player joins the game
		public override void UserJoined(Player player) 
		{
			player.Send("SetupPlayer", Players.Count());

			foreach (Player pl in Players)
				if (pl.ConnectUserId != player.ConnectUserId)
					pl.Send("PlayerJoined", Players.Count());
		}

		// This method is called when a player leaves the game
		public override void UserLeft(Player player) 
		{
			Broadcast("PlayerLeft", player.ConnectUserId);
		}

		// This method is called when a player sends a message into the server code
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
}