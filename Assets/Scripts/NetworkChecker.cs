using UnityEngine;

public class NetworkChecker
{
    public static bool CheckInternetConnection()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }

}
