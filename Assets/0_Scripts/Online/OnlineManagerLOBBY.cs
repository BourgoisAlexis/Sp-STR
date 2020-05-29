using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerIOClient;
using UnityEngine.SceneManagement;

public class OnlineManagerLOBBY : MonoBehaviour
{
	#region Variables
	private string userid;
	private string roomID;
	private bool create;
	
	private Client client;
	private List<Message> messages = new List<Message>();
	private Connection _connection;
	private AsyncOperation loadingScene;
	#endregion


	void Start()
	{
		Application.runInBackground = true;

		// Create a random userid 
		System.Random random = new System.Random();
		userid = "Guest" + random.Next(0, 10000);

		Debug.Log("Starting");

		PlayerIO.Authenticate
		(
			"str-e3p7vepld0ogaemgqgc7q",                                    //Your game id
			"public",                                                       //Your connection id
			new Dictionary<string, string> { { "userId", userid }, },       //Authentication arguments
			null,                                                           //PlayerInsight segments
			AuthentSuccess(userid),
			AuthentFail()
		);
	}

	void FixedUpdate()
	{
		foreach (Message m in messages)
		{
			switch (m.Type)
			{
				case "PlayerJoined":
					Debug.Log("Un joueur a rejoind");
					break;

				case "PlayerLeft":
					GameObject playerd = GameObject.Find(m.GetString(0));
					Destroy(playerd);
					break;
			}
		}

		// clear message queue after it's been processed
		messages.Clear();
	}


	private Callback<Client> AuthentSuccess(string userid)
	{
		return delegate (Client _client)
		{
			Debug.Log("Successfully connected to Player.IO");
			client = _client;

			Debug.Log("Create ServerEndpoint");
			// Comment out the line below to use the live servers instead of your development server
			client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

			Debug.Log("CreateJoinRoom");
			//Create or join the room
			client.Multiplayer.CreateJoinRoom
			(
				"Lobby",								//Room id. If set to null a random roomid is used
				"Lobby",								//The room type started on the server
				true,                                   //Should the room be visible in the lobby?
				null,
				null,
				delegate (Connection connection)
				{
					Debug.Log("Joined Room.");
					// We successfully joined a room so set up the message handler
					_connection = connection;
					_connection.OnMessage += handlemessage;
				}
			);
		};
	}

	private Callback<PlayerIOError> AuthentFail()
	{
		return delegate (PlayerIOError error)
		{
			Debug.Log("Error connecting: " + error.ToString());
		};
	}


	private void handlemessage(object sender, Message m)
	{
		messages.Add(m);
	}


	public void CreateRoom(string _roomName)
	{
		roomID = _roomName;
		client.Multiplayer.ListRooms("Versus", null, 10, 0, CreateRoomCheck(), NoRoomList());
	}

	public void JoinRoom(string _roomName)
	{
		roomID = _roomName;
		client.Multiplayer.ListRooms("Versus", null, 10, 0, JoinRoomCheck(), NoRoomList());
	}

	private Callback<RoomInfo[]> CreateRoomCheck()
	{
		return delegate (RoomInfo[] roomInfos)
		{
			int roomNumb = 0;
			foreach (RoomInfo i in roomInfos)
				if (i.Id == roomID)
					roomNumb++;

			if (roomNumb > 0)
			{
				GetComponent<LobbyManager>().RoomError(roomID, true);
				return;
			}

			create = true;
			LoadNextScene();
		};
	}

	private Callback<RoomInfo[]> JoinRoomCheck()
	{
		return delegate (RoomInfo[] roomInfos)
		{
			int roomNumb = 0;
			foreach (RoomInfo i in roomInfos)
				if (i.Id == roomID)
					roomNumb++;

			if (roomNumb <= 0)
			{
				GetComponent<LobbyManager>().RoomError(roomID, false);
				return;
			}

			create = false;
			LoadNextScene();
		};
	}

	private Callback<PlayerIOError> NoRoomList()
	{
		return delegate (PlayerIOError error)
		{
			Debug.Log("Couldn't access room list");
		};
	}


	//First
	public void LoadNextScene()
	{
		StartCoroutine(ActualLoadScene(1));
	}
	//Second
	private IEnumerator ActualLoadScene(int _sceneIndex)
	{
		loadingScene = SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Single);
		while (!loadingScene.isDone)
			yield return null;

		GlobalManager.Instance.OnlineManager.Connect(userid, roomID, create);
		Destroy(gameObject);
	}
}
