using System;
using System.Collections.Generic;
using System.Linq;
using CoreLocation;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Foundation;
using Microsoft.Azure.Mobile.Distribute;
using UIKit;

namespace IBeaconPrototype.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
	    private CLLocationManager _locationManager;
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
            MobileCenter.Start("27ddf720-97e5-4eea-839d-4ee03e8e8264", typeof(Analytics), typeof(Crashes), typeof(Distribute));

            _locationManager = new CLLocationManager();
            _locationManager.RequestAlwaysAuthorization();

            global::Xamarin.Forms.Forms.Init ();
			LoadApplication (new IBeaconPrototype.App ());

            return base.FinishedLaunching (app, options);
		}
	}
}
