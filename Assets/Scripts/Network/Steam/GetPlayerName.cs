using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!SteamManager.Initialized) { return; }

        string name = Steamworks.SteamFriends.GetPersonaName();
        print(name);
    }

}
