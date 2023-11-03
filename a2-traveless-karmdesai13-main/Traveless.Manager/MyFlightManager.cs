using System;
using System.Collections.Generic;
using System.IO;
using Traveless.Manager.Abstract;
using Traveless.Manager.Entities;

namespace Traveless.Manager
{
    /// <summary>
    /// Manages flights
    /// </summary>
    public class MyFlightManager : FlightManager
    {
        /// <summary>
        /// Path to flights.csv file
        /// </summary>
        public static readonly string FLIGHTS_FILE = "Data/flights.csv";

        /// <summary>
        /// Populates list with Flight instances from file
        /// </summary>
        protected override void LoadFlights()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FLIGHTS_FILE);

            // Open StreamReader at path
            StreamReader reader = File.OpenText(path);
            string? eachLine = reader.ReadLine();

            // Loop through each line in the file
            //  Transform line into cells using a comma (,)
            while(eachLine != null)
            {
                string[] splitLine = eachLine.Split(',');

                if(splitLine.Length != 7 )
                {
                    continue;

                }
                Flight _flight = new Flight();
                _flight.Code = splitLine[0].ToString();
                _flight.To= splitLine[1].ToString();
                _flight.From= splitLine[2].ToString();
                _flight.WeekDay = splitLine[3].ToString();
                _flight.Time = splitLine[4].ToString();
                _flight.TotalSeats = Convert.ToInt32(splitLine[5]);
                _flight.CostPerSeat = decimal.Parse(splitLine[6]);
                _flights.Add(_flight);

                eachLine = reader.ReadLine();
            }
            reader.Close();
            //  Check number of cells is not 7
            //      Do next iteration of loop if incorrect number of cells

            //  Create Flight instance from cells
            //  Add Flight instance to _flights list

        }

        /// <summary>
        /// Finds flight with code
        /// </summary>
        /// <param name="code">Flight code argument</param>
        /// <returns>Flight instance (or null if not found)</returns>
        public override Flight? FindFlightByCode(string code)
        {
            // Loop through each flight in Flights
            //  Check current flight code exactly matches code argument
            //      Return current Flight instance to calling method
            foreach(Flight flight in Flights)
            {
                if(flight.Code == code)
                {
                    return flight;
                }
            }

            // Return null to calling method if no flight with code is found.
            return null;
        }
    }
}
