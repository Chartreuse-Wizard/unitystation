using System.Collections;
using PlayGroup;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
///     Tells client to 
/// </summary>
public class PlayerMoveMessage : ServerMessage
{
	public static short MessageType = (short) MessageTypes.PlayerMoveMessage;
	public PlayerState State;
	public NetworkInstanceId Recipient;
	//Reset client's prediction queue
	public bool ResetQueue;

	///To be run on client
	public override IEnumerator Process()
	{
//		Debug.Log("Processed " + ToString());
		yield return WaitFor(Recipient);
		var playerSync = NetworkObject.GetComponent<IPlayerSync>();
		playerSync.UpdateClientState(State);
		if (ResetQueue)
		{
			playerSync.ResetClientQueue();
		}
		
	}

	public static PlayerMoveMessage Send(GameObject recipient, PlayerState state, bool resetQueue = false)
	{
		var msg = new PlayerMoveMessage
		{
			Recipient = recipient != null ? recipient.GetComponent<NetworkIdentity>().netId : NetworkInstanceId.Invalid,
			State = state,
			ResetQueue = resetQueue
		};
		msg.SendTo(recipient);
		return msg;
	}

	public static PlayerMoveMessage SendToAll(GameObject recipient, PlayerState state, bool resetQueue = false)
	{
		var msg = new PlayerMoveMessage
		{
			Recipient = recipient != null ? recipient.GetComponent<NetworkIdentity>().netId : NetworkInstanceId.Invalid,
			State = state,
			ResetQueue = resetQueue
		};
		msg.SendToAll();
		return msg;
	}

	public override string ToString()
	{
		return $"[PlayerMoveMessage State={State} Recip={Recipient} Reset]";
	}
}