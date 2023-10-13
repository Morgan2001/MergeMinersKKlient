using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FBInit : MonoBehaviour
{

    public string Device_ID;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>

        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });


        Device_ID = SystemInfo.deviceUniqueIdentifier;
        Debug.Log(Device_ID);



        BaseTenjin instance = Tenjin.getInstance("SEJQLXWSXFH6CECDB4JY7BIP6KWY3NN3");

        instance.SetAppStoreType(AppStoreType.googleplay);

        instance.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
