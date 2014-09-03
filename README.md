## Introduction

_This code sample shows how to create a compass custom control and use compass sensor to set it's value, following the MVVM pattern._

## Building the Sample

_To build this sample decompress the .zip file and open solution in Visual Studio 2010. Select Build from project's context menu._

_This project uses MVVM Light Toolkit ([http://mvvmlight.codeplex.com/](http://mvvmlight.codeplex.com/)) to implement MVVM pattern. If you have problems with references to the toolkit while building the project, either add the .dll assemblies included in Lib directory to the project's references, or download the toolkit from it's website._

_If you run the sample on the Emulator, since it does not have a magnetometer (compass sensor), the pointer will remain static._

## Description

_This project shows how can we easily make a compass application by implementing our own compass control, and following MVVM pattern._

This sample implements a custom control with a DependencyProperty called heading.

C#

    public class CompassControl : Control 
    { 
        public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register("Heading", typeof(double), typeof(CompassControl), null); 
 
        public CompassControl() 
        { 
            DefaultStyleKey = typeof(CompassControl); 
        } 
 
        public double Heading 
        { 
            get { return (double)base.GetValue(HeadingProperty); } 
            set { base.SetValue(HeadingProperty, value); } 
        } 
    }

The heading property of the compass control is binded to a property in the ViewModel.

XAML

    <my:CompassControl Width="300" Height="300" Heading="{Binding CurrentHeading}"/>

C#


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

 Every time compass sensor returns a new value, CurrentHeading property is set and the custom compass control in the user interface reflects it's value.

C#

    myCompass.CurrentValueChanged += new System.EventHandler<SensorReadingEventArgs<CompassReading>>((s, e) => 
                    { 
                        // This will update the current heading value. We have to put it in correct direction 
                        Deployment.Current.Dispatcher.BeginInvoke(() => { this.CurrentHeading = 360 - e.SensorReading.TrueHeading; }); 
                    });

 

## Source Code Files

- _CompassControl.cs - Implements custom compass control_
- _Generic.xaml - Defines the visual template of the compass control_
- _MainPage.xaml - Implements the View of the application__ _
- _MainViewModel.cs - Implements the ViewModel used by MainPage.xaml_