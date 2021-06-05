using Bus_ClassLibrary;
using Bus_ClassLibrary.Services;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BusStop_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CenterMap();
            LoadBusStops();
        }

        private async void CenterMap()
        {
            // Get Approximated User location based on IP address
            CurrentLocationServices currentLocationSvc = new CurrentLocationServices();
            CurrentLocationModel currentLocation = new CurrentLocationModel();

            currentLocation = await currentLocationSvc.GetCurrentLocation();

            // Center the Map based on approximated User Location
            Location mapCenter = new Location();

            mapCenter.Latitude = float.Parse(currentLocation.lat);
            mapCenter.Longitude = float.Parse(currentLocation.lon);

            myMap.Center = mapCenter;

            // Declare MapLayer to contain pushpins
            MapLayer pushpinLayer = new MapLayer();

            // Declare Pushpin for Approximated User Location
            Pushpin pushpin = new Pushpin();
            pushpin.Background = new SolidColorBrush(Colors.Blue);
            pushpin.Location = mapCenter;

            pushpinLayer.AddChild(pushpin, pushpin.Location);

            // Add PushPinLayer to Map
            myMap.Children.Add(pushpinLayer);
        }

        private async void LoadBusStops()
        {
            // Get all Bus Stops
            BusStopServices busStopSvc = new BusStopServices();
            List<BusStopModel> BusStops = new List<BusStopModel>();
            BusStops = await busStopSvc.GetBusStops();

            // Declare MapLayer to contain pushpins
            MapLayer pushpinLayer = new MapLayer();

            // Declare PushPins List
            List<Pushpin> pushpins = new List<Pushpin>();

            // Parse PushPins
            foreach (BusStopModel busStop in BusStops)
            {
                Location pp_loc = new Location();
                pp_loc.Longitude = double.Parse(busStop.Longitude);
                pp_loc.Latitude = double.Parse(busStop.Latitude);

                Pushpin pp = new Pushpin();
                pp.Background = new SolidColorBrush(Colors.Red);
                pp.Location = pp_loc;

                pushpins.Add(pp);

                pushpinLayer.AddChild(pp, pp.Location);
            }

            // Add PushPinLayer to Map
            myMap.Children.Add(pushpinLayer);
        }
    }
}
