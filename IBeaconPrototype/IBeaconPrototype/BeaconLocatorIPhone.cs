using System;
using System.Collections.Generic;
using System.Linq;
using CoreLocation;
using Foundation;

namespace IBeaconPrototype
{
#if __IOS__
    public delegate void BeaconRangedEventHandler(object sender, BeaconEventArgs e);
    public delegate void RegionEnteredEventHandler(object sender, BeaconEventArgs e);



    public enum CrossProximity
    {
        Unknown,
        Immediate,
        Near,
        Far
    }

    public class BeaconInfo
    {
        public static string BeaconUuid = "E4C8A4FC-F68B-470D-959F-29382AF72CE7"; //"E4C8A4FC-F68B-470D-959F-29";//

        public string PhoneNumber { get; set; }
        public NSUuid BeaconIdNsUuid { get; set; }
        public CLBeaconRegion BeaconRegion { get; set; }
        public CLProximity CurrentProximity { get; set; }
        public CLProximity PreviousProximity { get; set; }
    }

    public class BeaconEventArgs : EventArgs
    {
        public string PhoneNumber { get; set; }
        public CrossProximity CurrentProximity { get; set; }
        public CrossProximity PreviousProximity { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
        public double Accuracy { get; set; }
    }

    public class BeaconLocatorIPhone
    {
        public event BeaconRangedEventHandler BeaconRangedEvent;
        public event RegionEnteredEventHandler RegionEnteredEvent;

        private CLLocationManager _locationMgr;

        private readonly List<BeaconInfo> _beaconInfoList = new List<BeaconInfo>();

        private readonly List<string> _phoneNumerList;

        public BeaconLocatorIPhone(List<string> phoneNumbers)
        {
            Initialize();
            _phoneNumerList = phoneNumbers;

            foreach (var phoneNumber in phoneNumbers)
            {
                var beaconInfo = GetBeaconInfo(phoneNumber);
                beaconInfo.BeaconRegion.NotifyEntryStateOnDisplay = true;
                beaconInfo.BeaconRegion.NotifyOnEntry = true;
                beaconInfo.BeaconRegion.NotifyOnExit = true;

                _locationMgr.StartMonitoring(beaconInfo.BeaconRegion);
                _locationMgr.StartRangingBeacons(beaconInfo.BeaconRegion);

                _beaconInfoList.Add(beaconInfo);
            }
            
        }

        private static BeaconInfo GetBeaconInfo(string phoneNumber)
        {
            var beaconIdNsUuid = new NSUuid(BeaconInfo.BeaconUuid + phoneNumber);
            var beaconInfo = new BeaconInfo
            {
                PhoneNumber = phoneNumber,
                BeaconIdNsUuid = beaconIdNsUuid, //monkeyUUid
                BeaconRegion = new CLBeaconRegion(beaconIdNsUuid, phoneNumber)
            };
            return beaconInfo;
        }

        public void Stop()
        {
            foreach (var phoneNumber in _phoneNumerList)
            {
                var beaconInfo = GetBeaconInfo(phoneNumber);
                _locationMgr?.StopMonitoring(beaconInfo.BeaconRegion);
                _locationMgr?.StopRangingBeacons(beaconInfo.BeaconRegion);
            }
            _locationMgr = null;
        }

        private void Initialize()
        {

            _locationMgr = new CLLocationManager();
            _locationMgr.RequestWhenInUseAuthorization();

            _locationMgr.RegionEntered += (object sender, CLRegionEventArgs e) =>
            {
                var storedBeacon = _beaconInfoList.FirstOrDefault(b => b.PhoneNumber == e.Region.Identifier);

                if (storedBeacon == null) return;
                
                RegionEnteredEvent?.Invoke(this, new BeaconEventArgs
                {
                    PhoneNumber = storedBeacon.PhoneNumber,
                    Latitude = e.Region.Center.Latitude,
                    Longitude = e.Region.Center.Longitude,
                    Radius = e.Region.Radius
                });
            };

            _locationMgr.DidRangeBeacons += (object sender, CLRegionBeaconsRangedEventArgs e) =>
            {
                if (e.Beacons.Length <= 0) return;

                foreach (var beacon in e.Beacons)
                {
                    var storedBeacon =
                        _beaconInfoList.FirstOrDefault(b => Equals(b.BeaconRegion.ProximityUuid, beacon.ProximityUuid));

                    if(storedBeacon == null) continue;
                    if (storedBeacon.PreviousProximity == beacon.Proximity) continue;
                    
                    //beacon Moved, fire event
                    storedBeacon.CurrentProximity = beacon.Proximity;
                    BeaconRangedEvent?.Invoke(this, new BeaconEventArgs
                    {
                        PhoneNumber = storedBeacon.PhoneNumber,
                        CurrentProximity = beacon.Proximity == CLProximity.Far ? CrossProximity.Far : beacon.Proximity== CLProximity.Near ? CrossProximity.Near : beacon.Proximity == CLProximity.Immediate ? CrossProximity.Immediate : CrossProximity.Unknown,
                        PreviousProximity = storedBeacon.PreviousProximity == CLProximity.Far ? CrossProximity.Far : storedBeacon.PreviousProximity == CLProximity.Near ? CrossProximity.Near : storedBeacon.PreviousProximity == CLProximity.Immediate ? CrossProximity.Immediate : CrossProximity.Unknown,
                    });
                    storedBeacon.PreviousProximity = beacon.Proximity;
                }
            };
        }


    }

#endif
}
