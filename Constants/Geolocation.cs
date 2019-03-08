
using Android.Content;
using Android.Locations;

namespace TaskAppWithLogin.Constants
{
    public class Geolocation
    {
        LocationManager _locationManager;


        public string GetGeoLocation(Context context)
        {
            string geoLocation = "";
            _locationManager = (LocationManager)context.GetSystemService(LocationManager.KeyLocationChanged);

            if (_locationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                var criteria = new Criteria { PowerRequirement = Power.Medium, Accuracy = Accuracy.Coarse };

                var bestProvider = _locationManager.GetBestProvider(criteria, true);
                var location = _locationManager.GetLastKnownLocation(bestProvider);

                if (location != null)

                {
                    geoLocation = location.Latitude.ToString() + "," + location.Longitude.ToString();
                    //add = string.Format("{0:f6},{1:f6}", location.Latitude.ToString() + "," + location.Longitude.ToString());
                    //Address address =  ReverseGeocodeCurrentLocation();
                }

            }
            else if(_locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                //var criteria = new Criteria { PowerRequirement = Power.High, Accuracy = Accuracy.Medium };

                var bestProvider = _locationManager.GetBestProvider(null, true);
                var location = _locationManager.GetLastKnownLocation(bestProvider);

                if (location != null)
                {
                    geoLocation = location.Latitude.ToString() + "," + location.Longitude.ToString();
                    //add = string.Format("{0:f6},{1:f6}", location.Latitude.ToString() + "," + location.Longitude.ToString());
                    //Address address =  ReverseGeocodeCurrentLocation();
                }

            }
            return geoLocation;
        }


    }
}