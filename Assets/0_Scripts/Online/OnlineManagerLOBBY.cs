﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerIOClient;
using UnityEngine.SceneManagement;

public class OnlineManagerLOBBY : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject login;
	[SerializeField] private GameObject lobby;

	private string userID;
	private string passWord;
	private DatabaseObject account;

	private bool createAccount;
	private string roomID;
	private bool createRoom;
	private List<Message> messages = new List<Message>();
	private Client _client;
	private Connection _connection;
	private AsyncOperation loadingScene;
	#endregion


	private void Start()
	{
		login.SetActive(true);
		lobby.SetActive(false);
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


	public void Connect(string _userID, string _passWord, bool _createAccount)
	{
		Application.runInBackground = true;

		userID = _userID;
		passWord = _passWord;
		createAccount = _createAccount;

		PlayerIO.Authenticate
		(
			"str-e3p7vepld0ogaemgqgc7q",                                    //Your game id
			"public",                                                       //Your connection id
			new Dictionary<string, string> { { "userId", userID }, },       //Authentication arguments
			null,                                                           //PlayerInsight segments
			AuthentSuccess(),
			AuthentFail()
		);
	}

	private Callback<Client> AuthentSuccess()
	{
		return delegate (Client client)
		{
			Debug.Log("Successfully connected to Player.IO");

			_client = client;

			if (createAccount)
			{
				_client.BigDB.Load("AccountObjects", userID, CreateAccount());
				return;
			}

			_client.BigDB.Load("AccountObjects", userID, Login());
		};
	}

	private Callback<PlayerIOError> AuthentFail()
	{
		return delegate (PlayerIOError error)
		{
			Debug.Log("Error connecting: " + error.ToString());
		};
	}


	private Callback<DatabaseObject> CreateAccount()
	{
		return delegate (DatabaseObject _account)
		{
			if(_account != null)
			{
				GetComponent<LobbyError>().SetText("This username already exists");
				return;
			}

			DatabaseObject newAccount = new DatabaseObject();
			newAccount.Set("PassWord", passWord);
			newAccount.Set("Victories", 0);
			newAccount.Set("Defeats", 0);

			_client.BigDB.CreateObject("AccountObjects", userID, newAccount, null);
			GetComponent<LoginManager>().Base();
		};
	}

	private Callback<DatabaseObject> Login()
	{
		return delegate (DatabaseObject _account)
		{
			if (_account == null)
			{
				GetComponent<LobbyError>().SetText("This username does not exist");
				return;
			}
			if (passWord != _account.GetString("PassWord"))
			{
				GetComponent<LobbyError>().SetText("Your username or password is incorrect");
				return;
			}

			Debug.Log("Connected with correct Logins");
			account = _account;

			Debug.Log("Create ServerEndpoint");
			// Comment out the line below to use the live servers instead of your development server
			//_client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

			lobby.SetActive(true);
			GetComponent<LobbyManager>().Setup(userID, _account.GetInt("Victories"), _account.GetInt("Defeats"));
			login.SetActive(false);

			_client.Multiplayer.CreateJoinRoom
			(
				"Lobby",                                //Room id. If set to null a random roomid is used
				"Lobby",                                //The room type started on the server
				true,                                   //Should the room be visible in the lobby?
				null,
				null,
				delegate (Connection connection)
				{
					Debug.Log("Joined Room.");
					_connection = connection;
					_connection.OnMessage += handlemessage;
				}
			);
		};
	}


	public void CreateRoom(string _roomName)
	{
		roomID = _roomName;
		_client.Multiplayer.ListRooms("Versus", null, 10, 0, CreateRoomCheck(), NoRoomList());
	}

	public void JoinRoom(string _roomName)
	{
		roomID = _roomName;
		_client.Multiplayer.ListRooms("Versus", null, 10, 0, JoinRoomCheck(), NoRoomList());
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

			createRoom = true;
			LoadNextScene();
		};
	}

	private Callback<RoomInfo[]> JoinRoomCheck()
	{
		return delegate (RoomInfo[] roomInfos)
		{
			int roomNumb = 0;
			foreach (RoomInfo i in roomInfos)
				if (i.Id == roomID && i.OnlineUsers < 2)
					roomNumb++;

			if (roomNumb <= 0)
			{
				GetComponent<LobbyManager>().RoomError(roomID, false);
				return;
			}

			createRoom = false;
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


	private void handlemessage(object sender, Message m)
	{
		messages.Add(m);
	}
	
	
	public void LoadNextScene()
	{
		StartCoroutine(ActualLoadScene(1));
	}

	private IEnumerator ActualLoadScene(int _sceneIndex)
	{
		loadingScene = SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Single);
		_connection.Disconnect();

		while (!loadingScene.isDone)
			yield return null;

		GlobalManager.Instance.OnlineManager.Connect(userID, roomID, createRoom, account);

		Destroy(gameObject);
	}
}
