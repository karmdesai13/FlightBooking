﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Traveless.Manager;
using Traveless.Manager.Abstract;
using Traveless.Manager.Entities;
using Traveless.Manager.Exceptions;
using Traveless.ViewModels;

namespace Traveless
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        /// <summary>
        /// Holds instance of airline manager.
        /// </summary>
        private readonly AirlineManager AirlineManager;

        /// <summary>
        /// Holds instance of airport manager.
        /// </summary>
        private readonly AirportManager AirportManager;

        /// <summary>
        /// Holds instance of flight manager.
        /// </summary>
        private readonly FlightManager FlightManager;

        /// <summary>
        /// Holds instance of reservation manager.
        /// </summary>
        private readonly ReservationManager ReservationManager;

        #endregion
        #region Properties
        /// <summary>
        /// Holds flight view models for listview.
        /// </summary>
        public ObservableCollection<FlightViewModel> FlightViewModels { get; }

        /// <summary>
        /// Holds airport view models for combo boxes.
        /// </summary>
        public AirportViewModels AirportViewModels { get; }

        /// <summary>
        /// Holds reservation view models for listview.
        /// </summary>
        public ObservableCollection<ReservationViewModel> ReservationViewModels { get; }

        /// <summary>
        /// Holds week day view models for combo boxes.
        /// </summary>
        public WeekDayViewModels WeekdayViewModels { get; }

        #endregion
        #region Constructor
        /// <summary>
        /// Constructors MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            AirlineManager = new MyAirlineManager();
            AirportManager = new MyAirportManager();
            FlightManager = new MyFlightManager();
            ReservationManager = new MyReservationManager();

            FlightViewModels = new ObservableCollection<FlightViewModel>();
            ReservationViewModels = new ObservableCollection<ReservationViewModel>();
            AirportViewModels = new AirportViewModels(AirportManager.Airports);
            WeekdayViewModels = new WeekDayViewModels();

            PopulateFlights(null, null, null);
            PopulateReservations(null, null, null);
        }
        #endregion


        /// <summary>
        /// Performs a search when a user clicks the 'Search' button in the Flight tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlightsSearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Gets the from/to airport codes and weekday that the user selected
            int formSelectedIndex = this.searchFrom.SelectedIndex;
            Airport fromAirport = AirportManager.Airports[formSelectedIndex];

            string fromAirportCode = fromAirport.Code;
            // Get from airport code that user selected
           int  toSelectedIndex = this.searchTo.SelectedIndex;
            Airport toAirport = AirportManager.Airports[toSelectedIndex];
            string toAirportCode = toAirport.Code;
            // Get to airport code that user selected
            string weekDay=searchDay.Text;
            // Get the weekday the user selected (as a string)
            SearchFlights(fromAirportCode,toAirportCode, weekDay);
            // Pass from, to, and weekDay to SearchFlights method
        }

        /// <summary>
        /// Populates inputs for reserving flight
        /// </summary>
        /// <param name="fvw">Selected FlightViewModel instance</param>
        private void PopulateFlightInputs(FlightViewModel? fvw)
        {
            if (fvw != null)
            {
                // Set Flight tab "Flight" input value to selected flight code
                flightCode.Text = fvw.Code;

                var airline = AirlineManager.FindAirline(fvw.Flight.AirlineCode);

                // Set Flight tab "Airline" input value to selected airline name
                flightAirline.Text = airline.Name;
                // Set Flight tab "Day" input to selected weekday
                flightDay.Text = fvw.WeekDay;
                flightTime.Text=fvw.Time;

                // Set Flight tab "Time" input to selected time

                // Create costPerSeatFormatted variable
                // Assign CostPerSeat in currency format to costPerSeatFormatted
                string costPerSeatFormatted=fvw.CostPerSeat;
                flightCost.Text = costPerSeatFormatted.ToString();
                // Set Flight tab "Cost" input to costPerSeatFormatted
            }
        }

        /// <summary>
        /// Tries to make a reservation when the user clicks the 'Reserve' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlightsReserveButton_Click(object sender, RoutedEventArgs e)
        {
            // fvw has information on selected flight
            FlightViewModel? fvw = FlightsListView.SelectedItem as FlightViewModel;

            var flight = fvw?.Flight;

            if (flight != null)
            {
                // Get Flight tab "Name" input value
                string flightNAme=flightName.Text;

                string flightCItizenship = flightCitizenship.Text;
                // Get Flight tab "Citizenship" input value
                ReserveFlight(flight,flightNAme,flightCItizenship);
                // Call ReserveFlight with flight, name, and citizenship

                // Set Flight tab "Name" input value to empty string
            }
        }

        /// <summary>
        /// Search reservations when user clicks the 'Search' button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReservationsSearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get reservation code search input value
            string reservationCode= searchReservationCode.Text;
            //int resrvationSelectedIndex = this.searchReservationCode.SelectedIndex;

            // Get airline code search input value
            string airLineCode = searchAirlineCode.Text;

            // Get name search input value
            string  searchName=reserveSearchName.Text;

            // Call SearchReservations with reservation code, airline code, and name
            SearchReservations(reservationCode,airLineCode,searchName);
        }


        /// <summary>
        /// Populates reservation inputs
        /// </summary>
        /// <param name="rvm">ReservationViewModel instance</param>
        private void PopulateReserveInputs(ReservationViewModel? rvm)
        {
            if (rvm != null)
            {
                // Set "Code" input value to reservation code
                reserveCode.Text = rvm.Code;

                // Set "Flight" input value to flight code
                reserveFlight.Text = rvm.Flight;    


                var airline = AirlineManager.FindAirline(rvm.Reservation.Flight.AirlineCode);

                // Set "Airline" input value to airline name (not code)
                reserveAirline.Text = airline.Name;
                // Declare variable costPerSeatFormatted and assign the cost per seat in currency format.
                decimal costPerSeatFormatted = rvm.Reservation.Flight.CostPerSeat;
                reserveCost.Text = costPerSeatFormatted.ToString();
                // Set "Cost" input value to costPerSeatFormatted

                // Set "Name" input value to name on reservation
                reserveName.Text = rvm.Name;

                // Set "Citizenship" input value to citizenship on reservation
                reserveCitizenship.Text=rvm.Citizenship;
                // Check if reservation is active
                // Select "Active" option in "Status" combobox
                // Otherwise
                // Select "Inactive" option in "Status" combobox

                if(rvm.Reservation.IsActive)
                {
                    reserveStatus.Text = "Active";
                }
                else
                {
                    reserveStatus.Text="Inactive";
                }
            }
        }

        /// <summary>
        /// Updates a reservation when user clicks the 'Update' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReservationsUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Get reservation code input value
            string reservationCode=reserveCode.Text;

            // Get "Name" input value
            string reservationName = reserveName.Text;

            // Get "Citizenship" input value
            string reservationCitizenship = reserveCitizenship.Text;

            // Create bool variable called "isReservationActive"

            bool isReservationActive;

            // Check if "Status" combobo has "Active" selected
            // Assign true to "isReservationActive"
            // Otherwise
            // Assign false to "isReservationActive"
            if(reserveStatus.Text == "Active")
            {
                isReservationActive = true;
            }
            else
            {
                isReservationActive = false;
            }
            // Call UpdateReservation method with code, name, citizenship, and isActive
            UpdateReservation(reservationCode,reservationName,reservationCitizenship,isReservationActive);
        }


        #region Provided Methods
        /**
         * DO NOT MODIFY THE CODE IN THIS REGION!
         * IF YOU'RE HAVING TO MODIFY SOMETHING, YOU'RE DOING IT WRONG.
         */

        /// <summary>
        /// Populates the flight view models by converting Flight instances into FlightViewModel instances.
        /// </summary>
        /// <param name="from">Include flights from this airport</param>
        /// <param name="to">Include flights going to this airport</param>
        /// <param name="weekDay">Include flights on this weekday, or, null to include any weekday</param>
        /// <remarks>If all parameters are null, all flights are added to listview</remarks>
        private void PopulateFlights(string? from, string? to, string? weekDay)
        {
            // Clear out any existing flight view models
            // Not clearing the list will cause items to be appended to the bottom of the list.
            FlightViewModels.Clear();

            foreach (var flight in FlightManager.Flights)
            {
                // Include flight if from, to, and weekDay is null
                bool noSearchCriteria = (from == null && to == null && weekDay == null) ? true : false;

                // Check flight from / to matches what selected
                bool matchingSearchCriteria = (flight.From == from && flight.To == to && (weekDay == null || flight.WeekDay == weekDay)) ? true : false;

                if (noSearchCriteria || matchingSearchCriteria)
                {
                    FlightViewModel flightViewModel = new FlightViewModel(flight, ReservationManager.AvailableSeats(flight));
                    FlightViewModels.Add(flightViewModel);
                }
            }

            // Displays message to user if no flights were found (as a courtesy)
            if (FlightViewModels.Count == 0)
            {
                MessageBox.Show(this, "No flights found.", "Traveless", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

        }

        /// <summary>
        /// Searches for flights with matching criteria
        /// </summary>
        /// <param name="from">From airport code</param>
        /// <param name="to">To airport code</param>
        /// <param name="weekDay">Weekday (one of WeekDayViewModels.WEEKDAY_*)</param>
        private void SearchFlights(string from, string to, string? weekDay)
        {
            // Makes sure there is a from, to, and weekday selected.
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) ||
                string.IsNullOrEmpty(weekDay))
            {
                MessageBox.Show(this, "One or more combo boxes is not selected.", "Traveless", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (weekDay == WeekDayViewModels.WEEKDAY_ANY)
            {
                weekDay = null;
            }

            PopulateFlights(from, to, weekDay);
        }

        /// <summary>
        /// Populates the appropriate textboxes with the flight the user selected from the listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlightsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlightViewModel? fvw = FlightsListView.SelectedItem as FlightViewModel;

            PopulateFlightInputs(fvw);
        }

        /// <summary>
        /// Reserves a flight
        /// </summary>
        /// <param name="flight">Flight instance</param>
        /// <param name="name">Name of reservation</param>
        /// <param name="citizenship">Persons citizenship</param>
        private void ReserveFlight(Flight flight, string name, string citizenship)
        {
            try
            {
                Reservation reservation = ReservationManager.MakeReservation(flight, name, citizenship);

                PopulateReservations(null, null, null);

                MessageBox.Show(this, string.Format("Reservation created.\nConfirmation code: {0}", reservation.Code),
                    "Traveless", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Traveless", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Populates the reservation view models (from existing Reservation instances)
        /// </summary>
        /// <param name="code">Reservation code to look for</param>
        /// <param name="airlineCode">Code for airline to search for</param>
        /// <param name="name">Name to search</param>
        /// <remarks>If all parameters are null, all reservations are added.</remarks>
        private void PopulateReservations(string? code, string? airlineCode, string? name)
        {
            // Clear out any existing reservation view models
            ReservationViewModels.Clear();

            foreach (var reservation in ReservationManager.Reservations)
            {
                //bool noSearchCriteria = code == null && airlineCode == null && name == null ? true : false;
                //bool criteriaMatches = reservation.Code.Contains(code) && reservation.Flight.AirlineCode.Contains(airlineCode) && reservation.Name.Contains(name) ? true : false;

                if (code == null && airlineCode == null && name == null ? true : false || reservation.Code.Contains(code) && reservation.Flight.AirlineCode.Contains(airlineCode) && reservation.Name.Contains(name) ? true : false)
                {
                    var rvm = new ReservationViewModel(reservation);
                    ReservationViewModels.Add(rvm);
                }
            }
        }

        /// <summary>
        /// Searches reservations using criteria
        /// </summary>
        /// <param name="code">Reservation code to search for</param>
        /// <param name="airlineCode">Airline code to search for</param>
        /// <param name="name">Name to search for</param>
        private void SearchReservations(string code, string airlineCode, string name)
        {
            PopulateReservations(code, airlineCode, name);

            if (ReservationViewModels.Count == 0)
            {
                MessageBox.Show(this, "No reservations found.", "Traveless", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Populates the corresponding text boxes when user selects an item from the reservations listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReservationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var rvm = ReservationsListView.SelectedItem as ReservationViewModel;

            PopulateReserveInputs(rvm);
        }

        /// <summary>
        /// Updates existing reservation
        /// </summary>
        /// <param name="code">Reservation code</param>
        /// <param name="name">New name</param>
        /// <param name="citizenship">New citizenship</param>
        /// <param name="isActive">New is active</param>
        private void UpdateReservation(string code, string name, string citizenship, bool isActive)
        {
            try
            {
                ReservationManager.Update(code, name, citizenship, isActive);

                PopulateReservations(null, null, null);

                MessageBox.Show(this, "Reservation has been updated.", "Traveless", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (UpdateReservationException ex)
            {
                MessageBox.Show(this, ex.Message, "Traveless", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Handles a Window Closing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ReservationManager.Save();
        }



        #endregion

        
    }
}
