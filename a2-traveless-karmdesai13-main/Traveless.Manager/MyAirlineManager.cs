using System;
using System.Collections.Generic;
using System.IO;
using Traveless.Manager.Abstract;
using Traveless.Manager.Entities;

namespace Traveless.Manager
{
    /// <summary>
    /// Manages airlines
    /// </summary>
    public class MyAirlineManager : AirlineManager
    {
        /// <summary>
        /// Path to airlines.csv file
        /// </summary>
        public static readonly string AIRLINES_FILE = "Data/airlines.csv";

        /// <summary>
        /// Populate list with Airline instances from CSV files
        /// </summary>
        protected override void LoadAirlines()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AIRLINES_FILE);

            // Open StreamReader at path
            StreamReader reader=File.OpenText(path);
            string? eachLine=reader.ReadLine();

            // Loop through each line in the file
            //  Transform line into cells using a comma (,)
            while (eachLine != null) 
            {
                string[] splitLine = eachLine.Split(',');

                if (splitLine.Length != 2 )
                {
                    continue;
                }

                Airline _airline =new Airline(splitLine[0], splitLine[1]);
                _airline.Code= splitLine[0].ToString();
                _airline.Name= splitLine[1].ToString();
                _airlines.Add(_airline);

                eachLine = reader.ReadLine();
            }

            reader.Close();

            //  Check number of cells is not 2
            //      Do next iteration of loop if incorrect number of cells

            //  Create Airline instance from cells

            //  Add Airline instance to _airlines list

            // Close StreamReader
        }
    }
}
