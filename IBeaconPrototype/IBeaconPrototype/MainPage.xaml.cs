using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IBeaconPrototype
{
	public partial class MainPage : ContentPage
	{
	    private BeaconIPhone _beaconIPhone;
	    private BeaconLocatorIPhone _beaconLocatorIPhone;

	    public MainPage()
		{
			InitializeComponent();
		}

	    private void IBeaconButtonOn_OnClicked(object sender, EventArgs e)
	    {
	        _beaconIPhone = new BeaconIPhone("6199224340");
            _beaconIPhone.StartAdvertising();
            BackgroundColor = Color.Green;
	    }

	    private void IBeaconButtonOff_OnClicked(object sender, EventArgs e)
	    {
	        _beaconIPhone?.StopAdvertising();
            BackgroundColor= Color.White;
	    }

	    private void ListenButtonOn_OnClicked(object sender, EventArgs e)
	    {
	        _beaconLocatorIPhone = new BeaconLocatorIPhone(new List<string> { { "6199224340"}});
	        _beaconLocatorIPhone.BeaconRangedEvent += (o, args) =>
	        {
	            SetBackgroundColor(args);
	        };
	        _beaconLocatorIPhone.RegionEnteredEvent += (o, args) =>
	        {
	            SetBackgroundColor(args);
	        };

            BackgroundColor = Color.Blue;
	    }

	    private void SetBackgroundColor(BeaconEventArgs args)
	    {
	        switch (args.CurrentProximity)
	        {
	            case CrossProximity.Far:
	                BackgroundColor = Color.Red;
	                break;
	            case CrossProximity.Near:
	                BackgroundColor = Color.Yellow;
	                break;
	            case CrossProximity.Immediate:
	                BackgroundColor = Color.Green;
	                break;
	            default:
	                BackgroundColor = Color.Teal;
	                break;
	        }
	    }

	    private void ListenButtonOff_OnClicked(object sender, EventArgs e)
	    {
            _beaconLocatorIPhone.Stop();
	        _beaconLocatorIPhone = null;
            BackgroundColor=Color.White;
	    }
	}
}
