using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class RelayManager : MonoBehaviour
{
    [SerializeField]
    private string environment = "production";

    [SerializeField]
    private int maxconnections = 10;

    public string joinCode;
    public bool IsRelayEnabled => Transport != null &&
         Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();


    public async Task<RelayHostData> SetupRelay()
    {
        Debug.Log("Connecting to Relay Server");    

        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Debug.Log("Connected to Relay Server"); 

        Debug.Log($"Relay Sever is starting with max connection {maxconnections}");

        Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxconnections);

        RelayHostData relayHostData = new RelayHostData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            IPv4Address = allocation.RelayServer.IpV4,
            ConnectionData = allocation.ConnectionData
        };

        Debug.Log("A");

        relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

        Debug.Log("B");

        Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes,
            relayHostData.Key, relayHostData.ConnectionData);

        Debug.Log("C");

        Debug.Log($"Relay Sever generated a join code '{relayHostData.JoinCode}'");
        joinCode = relayHostData.JoinCode;
        return relayHostData;
    }


    public async Task<RelayJoinData> JoinRelay(string joinCode)
    {
        Debug.Log("Connecting to Relay Server");    

        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Debug.Log("Connected to Relay Server"); 

        Debug.Log($"Relay Sever is connecting with join code '{joinCode}'");

        JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);

        RelayJoinData relayJoinData = new RelayJoinData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            IPv4Address = allocation.RelayServer.IpV4,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            JoinCode = joinCode
        };

        Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes,
            relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);

        Debug.Log($"Client joined game with join code '{joinCode}'");

        return relayJoinData;
    }

    // private async void ConnectToRelayServer()
    // {
    //     Debug.Log("Connecting to Relay Server");    

    //     InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

    //     //Initialize the Unity Services engine
    //     await UnityServices.InitializeAsync(options);

    //     if (!AuthenticationService.Instance.IsSignedIn)
    //     {
    //         //If not already logged, log the user in
    //         await AuthenticationService.Instance.SignInAnonymouslyAsync();
    //     }

    //     Debug.Log("Connected to Relay Server"); 
    // }

}
