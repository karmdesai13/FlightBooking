using Traveless.Manager.Abstract;
using Traveless.Manager.Entities;

namespace Traveless.Manager
{
    /// <summary>
    /// Manages airports
    /// </summary>
    public class MyAirportManager : AirportManager
    {
        /// <summary>
        /// Path to airports.csv file
        /// </summary>
        public static readonly string AIRPORTS_FILE = "Data/airports.csv";

        /// <summary>
        /// Populates list with Airport instances from .csv file.
        /// </summary>
        protected override void LoadAirports()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AIRPORTS_FILE);

            // Open StreamReader at path
            StreamReader reader = File.OpenText(path);
            string? eachLine = reader.ReadLine();


            // Loop through each line in the file
            //  Transform line into cells using a comma (,)
            while (eachLine != null)
            {
                string[] splitLine = eachLine.Split(',');


                if (splitLine.Length != 2)
                {
                    continue;

                }
                Airport _airport = new Airport(splitLine[0], splitLine[1]);
                _airport.Code = splitLine[0];
                _airport.Name = splitLine[1];
                _airports.Add(_airport);

                eachLine= reader.ReadLine();

            }   //  Check number of cells is not 2
                //      Do next iteration of loop if incorrect number of cells

            //  Create Airport instance from cells


            //  Add Airport instance to _airports list
            reader.Close();
            // Close StreamReader
        }
    }
}
