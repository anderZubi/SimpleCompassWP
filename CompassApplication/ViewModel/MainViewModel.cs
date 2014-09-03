// © aZubi
using GalaSoft.MvvmLight;
using Microsoft.Devices.Sensors;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace CompassApplication.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region public properties

        public string ApplicationTitle
        {
            get
            {
                return "Compass Application";
            }
        }

        public string PageName
        {
            get
            {
                return "My Compass";
            }
        }

        /// <summary>
        /// The <see cref="CurrentHeading" /> property's name.
        /// </summary>
        public const string CurrentHeadingPropertyName = "CurrentHeading";

        private double _currentHeading = 0.0;

        /// <summary>
        /// Gets the Heading property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double CurrentHeading
        {
            get
            {
                return _currentHeading;
            }

            set
            {
                if (_currentHeading == value)
                {
                    return;
                }

                var oldValue = _currentHeading;
                _currentHeading = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CurrentHeadingPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsCalibrationNeeded" /> property's name.
        /// </summary>
        public const string IsCalibrationNeededPropertyName = "IsCalibrationNeeded";

        private bool _isCalibrationNeeded = false;

        /// <summary>
        /// Gets the IsCalibrationNeeded property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsCalibrationNeeded
        {
            get
            {
                return _isCalibrationNeeded;
            }

            set
            {
                if (_isCalibrationNeeded == value)
                {
                    return;
                }

                var oldValue = _isCalibrationNeeded;
                _isCalibrationNeeded = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(IsCalibrationNeededPropertyName);
            }
        }

        #endregion

        #region commands

        // This command handles the Calibration OK button click.
        public RelayCommand CalibrationOKButtonClickCommand { get; private set; }

        private object CalibrationOKButtonClick()
        {
            this.IsCalibrationNeeded = false;
            return null;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                if (Compass.IsSupported)
                {
                    // If compass sensor is supported create new compass object and attach event handlers
                    Compass myCompass = new Compass();
                    myCompass.TimeBetweenUpdates = System.TimeSpan.FromMilliseconds(100); // This defines how often heading is updated
                    myCompass.Calibrate += new System.EventHandler<CalibrationEventArgs>((s, e) =>
                    {
                        // This will show the calibration screen
                        this.IsCalibrationNeeded = true;
                    });
                    myCompass.CurrentValueChanged += new System.EventHandler<SensorReadingEventArgs<CompassReading>>((s, e) =>
                    {
                        // This will update the current heading value. We have to put it in correct direction
                        Deployment.Current.Dispatcher.BeginInvoke(() => { this.CurrentHeading = 360 - e.SensorReading.TrueHeading; });
                    });

                    // Start receiving data from compass sensor
                    myCompass.Start();
                }
                else
                {
                    //MessageBox.Show("Compass is not supported in this device."); // This line produces error if no compass present.
                }
            }

            // Initialize commands
            this.CalibrationOKButtonClickCommand = new RelayCommand(() => CalibrationOKButtonClick());
        }
    }
}