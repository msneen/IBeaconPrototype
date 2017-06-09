using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using CoreBluetooth;
using CoreFoundation;
using CoreLocation;
using AVFoundation;
using MultipeerConnectivity;
using UIKit;

namespace IBeaconPrototype
{
    public interface IBeacon
    {
        void StartAdvertising();
        void StopAdvertising();
    }

#if __IOS__

    public class BeaconIPhone : IBeacon
    {
        private string _phoneNumber;
        
        private CBPeripheralManager _peripheralMgr;
        private BTPeripheralDelegate _peripheralDelegate;
        private CLLocationManager _locationMgr;
        private CLProximity _previousProximity;
        private CLBeaconRegion _beaconRegion;
        private NSMutableDictionary _peripheralData;

        public BeaconIPhone(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            //BeaconInfo.BeaconUuid += phoneNumber;
            //_iBeaconId = phoneNumber;

            Initialize();
        }

        private void Initialize()
        {
            var beaconId = BeaconInfo.BeaconUuid;// + _phoneNumber;
            var beaconUUID = new NSUuid(beaconId);
            var beaconRegion = new CLBeaconRegion(beaconUUID, _phoneNumber);

            //power - the received signal strength indicator (RSSI) value (measured in decibels) of the beacon from one meter away
            var power = new NSNumber(-59);
            _peripheralData = beaconRegion.GetPeripheralData(power);
            _peripheralDelegate = new BTPeripheralDelegate();
            _peripheralMgr = new CBPeripheralManager(_peripheralDelegate, DispatchQueue.DefaultGlobalQueue);
            //_peripheralMgr.StartAdvertising(_peripheralData);
            
        }

        public void StartAdvertising()
        {
            _peripheralMgr.StartAdvertising(_peripheralData);
        }

        public void StopAdvertising()
        {
            _peripheralMgr.StopAdvertising();
        }

        public CBPeripheralManagerState GetState()
        {
            return _peripheralDelegate.BeaconState;
        }
    }
    public class BTPeripheralDelegate : CBPeripheralManagerDelegate
    {
        public CBPeripheralManagerState BeaconState { get; set; }

        public override void StateUpdated(CBPeripheralManager peripheral)
        {
            if (peripheral.State == CBPeripheralManagerState.PoweredOn)
            {
                BeaconState = peripheral.State;
                Console.WriteLine("powered on");
            }
        }
    }

#endif
}
