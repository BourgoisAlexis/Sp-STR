using UnityEngine;
using System.Collections.Generic;
using PlayerIOClient;

public class OnlineManagerVERSUS : MonoBehaviour
{
	#region Variables
	[SerializeField] private UnitMenu _unitMenu;

	private string userid;
	private string roomID;
	private bool create;

	private List<Message> messages = new List<Message>();
	private Connection _connection;
	private EntityManager _entityManager;
	#endregion


	private void Awake()
	{
		_entityManager = GetComponent<EntityManager>();
	}

	public void Connect(string _id, string _roomID, bool _create)
	{
		Application.runInBackground = true;
		userid = _id;
		roomID = _roomID;
		create = _create;

		Debug.Log("Starting");

		PlayerIO.Authenticate
		(
			"str-e3p7vepld0ogaemgqgc7q",
			"public",
			new Dictionary<string, string> { { "userId", userid }, },
			null,
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
				case "SetupPlayer":
					SetupPlayer(m.GetInt(0));
					break;

				case "PlayerJoined":
					Debug.Log("Un joueur a rejoind");
					GetComponent<GlobalManager>().SetupPlayerNumber(m.GetInt(0));
					break;

				case "SetUnitDestination":
					Vector3 desti = new Vector3(m.GetFloat(1), m.GetFloat(2), m.GetFloat(3));
					_entityManager.SetUnitDestination(m.GetInt(0), desti);
					break;

				case "SetUnitTarget":
					_entityManager.SetUnitTarget(m.GetInt(0), m.GetInt(1));
					break;

				case "SpawnUnit":
					Vector3 spawnPoint = new Vector3(m.GetFloat(1), m.GetFloat(2), m.GetFloat(3));
					GetComponent<UnitSpawner>().SpawnUnit(m.GetInt(0), spawnPoint, m.GetInt(4), true);
					break;

				case "DestroyUnit":
					GetComponent<EntityManager>().DestroyUnit(m.GetInt(0));
					break;

				case "UpdateHealth":
					GetComponent<EntityManager>().UpdateHealth(m.GetInt(0), m.GetInt(1));
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
		return delegate (Client client)
		{
			Debug.Log("Successfully connected to Player.IO");

			Debug.Log("Create ServerEndpoint");
			// Comment out the line below to use the live servers instead of your development server
			//client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);
			
			if(create)
			{
				client.Multiplayer.CreateJoinRoom
				(
					roomID,								//Room id. If set to null a random roomid is used
					"Versus",								//The room type started on the server
					true,									//Should the room be visible in the lobby?
					null,
					null,
					delegate (Connection connection)
					{
						Debug.Log("Joined Room.");
						_connection = connection;
						_connection.OnMessage += handlemessage;
					}
				);
			}
			else
			{
				client.Multiplayer.JoinRoom
				(
					roomID,
					null,
					delegate (Connection connection)
					{
						Debug.Log("Joined Room.");
						_connection = connection;
						_connection.OnMessage += handlemessage;

					}
				);
			}
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

	
	//Setup
	private void SetupPlayer(int _index)
	{
		GetComponent<EntityManager>().SetTeam(_index);
		_unitMenu.SetTeam(_index);
		
		GetComponent<GlobalManager>().SetupPlayerNumber(_index);
	}


	//Actions

	public void SetUnitDestination(int _index, Vector3 _desti)
	{
		object[] toSend = new object[] { _index, _desti.x, _desti.y, _desti.z };

		if (_connection != null)
			_connection.Send("SetUnitDestination", toSend);
	}

	public void SetUnitTarget(int _index, int _targetIndex)
	{
		object[] toSend = new object[] { _index, _targetIndex };

		if (_connection != null)
			_connection.Send("SetUnitTarget", toSend);
	}

	public void SpawnUnit(int _index, Vector3 _spawnPoint, int _tmIndex)
	{
		object[] toSend = new object[] { _index, _spawnPoint.x, _spawnPoint.y, _spawnPoint.z, _tmIndex };

		if (_connection != null)
			_connection.Send("SpawnUnit", toSend);
	}

	public void DestroyUnit(int _index)
	{
		if (_connection != null)
			_connection.Send("DestroyUnit", _index);
	}

	public void UpdateHealth(int _index, int _value)
	{
		object[] toSend = new object[] { _index, _value };

		if (_connection != null)
			_connection.Send("UpdateHealth", toSend);
	}

	public void Disconnect()
	{
		_connection.Disconnect();
	}
}
