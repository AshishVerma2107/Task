using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android;
using static Android.Gms.Maps.GoogleMap;
using Android.Locations;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskAppWithLogin
{
    [Activity(Label = "Google_Map")]
    public class Map_Activity : Activity, IOnMapReadyCallback
    {

        private GoogleMap GMap;
        Marker marker;
        bool isPeerLocation;
        string currentLocation;
        double Lat,Long;
        string addressText;
        Button getaddress;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Map);

            // Get our button from the layout resource,
            // and attach an event to it
            getaddress = FindViewById<Button>(Resource.Id.getadd);
            getaddress.Click += AddressButton_OnClick;
            SetUpMap();

            
        }

        private void SetUpMap()
        {
            if (GMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.googlemap).GetMapAsync(this);

            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;
            GMap.MapClick += (object sender, GoogleMap.MapClickEventArgs e) =>
            {

                using (var markerOption = new MarkerOptions())
                {
                    if(isPeerLocation)
                    {
                        GMap.Clear();

                        markerOption.SetPosition(e.Point);
                        markerOption.SetTitle(e.Point.Latitude.ToString() + e.Point.Longitude.ToString());
                        // save the "marker" variable returned if you need move, delete, update it, etc...
                        marker = GMap.AddMarker(markerOption);
                        currentLocation = e.Point.ToString();
                        Lat = e.Point.Latitude;
                        Long = e.Point.Longitude;
                        isPeerLocation = true;
                    }
                    else
                    {
                        
                        markerOption.SetPosition(e.Point);
                        markerOption.SetTitle(e.Point.Latitude.ToString() + e.Point.Longitude.ToString());
                        // save the "marker" variable returned if you need move, delete, update it, etc...
                        marker = GMap.AddMarker(markerOption);
                        currentLocation = e.Point.ToString();
                        Lat = e.Point.Latitude;
                        Long = e.Point.Longitude;
                        isPeerLocation = true;
                    }
                   
                }
            };


            GMap.UiSettings.ZoomControlsEnabled = true;

        }

        async void AddressButton_OnClick(object sender, EventArgs e)
        {
            if (currentLocation == null)
            {
                Toast.MakeText(this, "Can't determine the current address. Try again in a few minutes", ToastLength.Long).Show();
                //addressText.Text = "Can't determine the current address. Try again in a few minutes";
                return;
            }
            Address address = await ReverseGeocodeCurrentLocation();
            DisplayAddress(address);
        }

        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geo = new Geocoder(this);
            IList<Address> addressList =
                await geo.GetFromLocationAsync(Lat, Long, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        private void DisplayAddress(Address address)
        {
            if (address != null)
            {
                string sub = address.SubLocality;
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                addressText = deviceAddress.ToString();
            }
            else
            {
                addressText = "LOPLOPLOP";
            }
        }
    }
}

