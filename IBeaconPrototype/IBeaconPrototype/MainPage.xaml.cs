using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreBluetooth;
using Xamarin.Forms;

namespace IBeaconPrototype
{
	public partial class MainPage : ContentPage
	{
#if __IOS__
        private BeaconIPhone _beaconIPhone;
	    private BeaconLocatorIPhone _beaconLocatorIPhone;
#endif

        public MainPage()
		{
			InitializeComponent();
		}

	    private void IBeaconButtonOn_OnClicked(object sender, EventArgs e)
	    {
#if __IOS__
            _beaconIPhone = new BeaconIPhone("6199224340");
            _beaconIPhone.StartAdvertising();
            BackgroundColor = Color.Green;
#endif
        }

        private void IBeaconButtonOff_OnClicked(object sender, EventArgs e)
	    {
#if __IOS__
            _beaconIPhone?.StopAdvertising();
            BackgroundColor= Color.White;
#endif
        }

        private void ListenButtonOn_OnClicked(object sender, EventArgs e)
	    {
#if __IOS__
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
#endif
        }

#if __IOS__
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
#endif
        private void ListenButtonOff_OnClicked(object sender, EventArgs e)
	    {
#if __IOS__
         //   _beaconLocatorIPhone.Stop();
	        //_beaconLocatorIPhone = null;
         //   BackgroundColor=Color.White;
#endif

            var state = _beaconIPhone.GetState();
            var isAdvertising = _beaconIPhone.GetIsAdvertising();

            if (state == CBPeripheralManagerState.PoweredOn && isAdvertising)
            {
                BackgroundColor = Color.Aqua;
            }
            else
            {
                BackgroundColor= Color.Coral;
            }
	    }
    }
}
