using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace WageCalculator
{
    class Program
    {   // Global variables, for storing improtant information. These variables are global, so that they can be used in any method
        static double gross;
        static double net;
        static double diff;
        static double hours;
        static double ni_charge;
        static double income_tax;
        static double rate;
        static string title;
        static string name;
        static string last_name;
        static string NI_num;
        //Database global varables
        static private MySqlConnection connection;
        static private string server;
        static private string database;
        static private string uid;
        static private string password;
        // End of global variables


        static void Main(string[] args)
        {
            Initialise(); // Starts the method that allows C# to connect with MySQL Database
            log_in(); // Calls the log in mehtod
            main_menu(); // After the log in is successfull it calls the main menu
            
        }
        static void Initialise()
        {
            server = "localhost"; //local host (WAMP)
            database = "payslip_db"; //database name
            uid = "root"; //database username 
            password = ""; //database password
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }


        static void log_in()
        {
            while (true) { 
            // usernames and password to login
            string username_1 = "edward0310";
            string pass_1 = "truth";

            string username_2 = "korg1010";
            string pass_2 = "rockpaperscissors";

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("________________________________________________________________________________________________________________________");
                Console.WriteLine("---------------------------------------------------- THE PAYSLIP MANAGER ----------------------------------------------");
                Console.WriteLine("________________________________________________________________________________________________________________________");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n---------------------------------------------LOGIN TO START USING THE PROGRAM-------------------------------------------\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Insert your username:");
            string username = Console.ReadLine(); //take user input

            Console.WriteLine("Insert your password");
            string pass = Console.ReadLine();
            
            // checks if login details are correct
            if ((username==username_1 && pass==pass_1) || (username==username_2 && pass==pass_2)) 
            {
                    Console.ForegroundColor = ConsoleColor.Green; //Changes the colour of the text
                    Console.WriteLine("Logged In. Press [ENTER] to access at the main Menu");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    Console.Clear();
                    break;   
            }
            else // if the details are wrong, it restarts the log in process
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR, incorrect details. Press [ENTER] to continue..."); //Error message when login deatils are wrong.
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
                Console.Clear();
            }
            }
        }

        static void main_menu()
        {
            
            bool loopCounter = true;
            while (loopCounter == true)
            {// Start of loop, it allows to back in the menu
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n--------------------------------------------------------MAIN MENU------------------------------------------------------\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Choose an option (type the letter):");
                Console.WriteLine("  A) Create Wage Slip");
                Console.WriteLine("  B) View Wage Slip");
                Console.WriteLine("  C) Delte Wage Slip");
                Console.WriteLine("  D) Help");
                Console.WriteLine("  E) Exit");

                string choice = Console.ReadLine(); //Takes the input from the user and checks the answer, redirecting to the right process
                choice = choice.ToUpper(); 

                if (choice == "A")
                {
                    createSlip();
                }
                else if (choice == "B")
                {
                    viewSlip();
                }
                else if (choice == "C")
                {
                    deleteSlip();
                }
                else if (choice == "D")
                {
                    help();
                }
                else if (choice == "E")
                {
                    loopCounter = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR, inavlid choice, try again. Press [ENTER] to continue..."); //In case the input is invalid
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        static void math() //Method for calculating the payslip. Made in a different method, so that it can be used again without typing it again, and it makes the code more organised.
        {
            //Calculating gross
            if (hours < 40) // calculates the wages of people that worked less tahn 40 hours
            {
                gross = hours * rate;
            }
            else if (hours > 40 && hours < 50) // calculates the wages of people that worked between 40 and 50 hours
            {
                diff = hours - 40;  // calcualtes the overtime worked
                double NormalWage = 40 * rate; // calucaltion for normal wages, so the 40 hours worked
                double overtime = diff * (rate * 1.5); // calculates how much the employee earned during the overtime, which is payed 1.5 times more (only for the overtime hours)
                gross = NormalWage + overtime; // Calcualtes the gross
            }
            else if (hours > 50) // calcualtes the wage of the employees that worked more than 50 hours
            {
                diff = hours - 50; //Calcualtes the difference between the hours worked and 50, to have the hours that will be payed double as normal
                double NormalWage = 40 * rate; //Calcualtes the wage for the normal 40 hours
                double overtime1 = 10 * (1.5 * rate); // Calcualtes the overtime hours, between 40 and 50, which will be payed 1.5 times more
                double overtime2 = diff * (rate * 2); // calcualtes the rate for the hours worked after the 50th, which will be payed double
                gross = NormalWage + overtime1 + overtime2; // Calcualtes the gross
                diff = hours - 40; // total difference between hours worked, and the normal 40 hours, which indicates the total overtime worked.
            }
        }

        static void createSlip() //Method for creating slip, asks for all the information needed to create said slip
        {   // The while loop will restart this section if the item selected doesn't match the list
            Console.Clear();
            while (true) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n------------------------------------------------------CREATE SLIP----------------------------------------------------\n\n");
                Console.ForegroundColor = ConsoleColor.White;

                //Console.WriteLine("Please enter the title");
                // title = Console.ReadLine();
                Regex regexTitle = new Regex(@"^[a-zA-Z]*$"); // Validate user's input, by accepting only letters.
                bool val0 = true;
                while (val0 == true)
                {
                    Console.WriteLine("Please enter the title.");
                    title = Console.ReadLine();
                    if (regexTitle.IsMatch(title) && (title.Length > 1)) // Checks if the input fromt the user matches the validation declared above. Also it check if the user leave the field blank.
                    {
                        val0 = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nERRROR. Only letters are allowed in this field. Please try again\n"); // Error message if the user input character that are not in the range specified above.
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Regex regexName = new Regex(@"^[a-zA-Z -]*$"); // Validate user's input, by accepting only letters, spaces, and dashes.
                bool val = true;
                while (val == true)
                {
                    Console.WriteLine("\nPlease enter the first name");
                    name = Console.ReadLine();
                    Console.WriteLine("\nPlease enter the last name");
                    last_name = Console.ReadLine();
                    if (regexName.IsMatch(name) && regexName.IsMatch(last_name) && (name.Length > 1) && (last_name.Length > 1))
                    {
                        val = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nERRROR. Only letters, spaces and hyphen are allowed in the name field. Please try again\n"); // Error message if the user input character that are not in the range specified above.
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Regex regexNI = new Regex(@"^[a-zA-Z][a-zA-Z][0-9][0-9][0-9][0-9][0-9][0-9][a-zA-Z]*$"); // Restrict the user input of the NI number, to ensure the NI is correct. Allowing only certain letters or numebr in the correct position.
                bool val1 = true;
                while (val1 == true)
                {
                    Console.WriteLine("\nPlease enter the NI number (No spaces or dashes)");
                    NI_num = Console.ReadLine();
                    if (regexNI.IsMatch(NI_num))
                    {
                        val1 = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nERRROR. The NI Number should look like 'ST102030A'. Please try again.\n"); // Error message if the user input character that are not in the range specified above.
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                while (true)
                { 
                try { // This is to validate the user input, when the user insert a character that is not numeric it doesn't store it, and shows and error
                Console.WriteLine("\nPlease enter the hours worked this week");
                hours = double.Parse(Console.ReadLine());
                        break;
                    }
                catch // "catches" the error, and shows the message
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nERROR. Only numbers in this field. Please try again.\n"); // Error messagge when the input is incorrect.
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                Console.WriteLine("Please select the role (type corresponding letter):");
                Console.WriteLine("  A) Programmer \n  B) Administrators/Clerks \n  C) Project Leader/Manager \n  D) Enter manually");
                string role = Console.ReadLine();
                role = role.ToLower();

                if (role == "a") // Depending on the option it will set the rate accordingly
                {
                    rate = 12.03;
                    Console.WriteLine("\nThe houarly rate is {0:c}. Press [ENTER] to continue", rate);
                    Console.ReadLine();
                    break;
                }
                else if (role == "b")
                {
                    rate = 8.07;
                    Console.WriteLine("\nThe houarly rate is {0:c}. Press [ENTER] to continue", rate);
                    Console.ReadLine();
                    break;
                }
                else if (role == "c")
                {
                    rate = 22.54;
                    Console.WriteLine("\nThe houarly rate is {0:c}. Press [ENTER] to continue", rate);
                    Console.ReadLine();
                    break;
                }
                else if (role == "d") // Takes a manual input, in case the role is not in the given ones
                {
                    while (true) { 
                    Console.WriteLine("Please enter the role");
                    string job = Console.ReadLine();
                    try { 
                    Console.WriteLine("\nPlease enter your houarly rate (e.g. 9.85)");
                    rate = double.Parse(Console.ReadLine());
                    Console.WriteLine("\nThe houarly rate is {0:c}. Press [ENTER] to continue", rate);
                    Console.ReadLine();
                    break;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR, only numeric value (E.g. 12.03). Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    }
                    break;

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect choice, please select from list. Press [ENETR] to continue");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            math(); // Calls the math method, wich contains the calculation for generating gross, net and the taxes

            // Code for date
            DateTime date1 = new DateTime();
            date1 = DateTime.Now;
            Console.WriteLine(date1.ToString());
            bool tax_747 = false;
            if (gross > 143.65) //Applies the 747L Tax for emplyee with an income higher than £7470 per working year, so 143.65 per week.
            {

                double taxableAmount = gross - 143.65;
                ni_charge = (taxableAmount / 100) * 11;
                net = taxableAmount - ni_charge;
                income_tax = (net / 100) * 25;
                net = gross - income_tax - net;
                tax_747 = true;
            }
            else
            {
                net = gross;
            }

            // Code that prints the payslip, with realtive foramtting
            Console.WriteLine("\n Payslip created, press [ENTER] to view");
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("_____________________________________________________________________________________________________________________");
            Console.WriteLine("                                                         BANK OF ERNEST                                                 ");
            Console.WriteLine("________________________________________________________________________________________________________________________");
            Console.WriteLine("----------------------------------------------------------DATE: {0}---------------------------------------------", date1.ToShortDateString());
            Console.WriteLine("\n\n    TITLE: {0}                   NAME: {1}              LAST NAME: {2}", title.ToUpper(), name.ToUpper(), last_name.ToUpper());
            Console.WriteLine("\n    NI NUMBER: {0}", NI_num.ToUpper());
            Console.WriteLine("\n    HOURS WORKED: {0}                                     HOURLY RATE: {1:c}/h", hours, rate);
            Console.WriteLine("\n    OVERTIME: {0}", diff);
            Console.WriteLine("\n    INCOME TAX {0:c}", income_tax);
            Console.WriteLine("\n    NI CHARGE {0:c}", ni_charge);
            Console.WriteLine("\n    GROSS {0:c}", gross);
            Console.WriteLine("\n    747L TAX: {0}                                     ____________________________", tax_747);
            Console.WriteLine("                                                               NET INCOME {0:c}", net);
            Console.WriteLine("                                                        ____________________________");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Would you like to save the payslip? Y/N");
            string save = Console.ReadLine();
            save = save.ToLower();

            while (true) { 
            if (save == "y") {
            // Insert data into the SQL database        
            string query = "INSERT INTO payslip_tbl VALUES(NULL,'" + title + "','" + name + "','" + last_name + "','" + NI_num + "','" + hours + "','" + rate + "','" + diff + "','" + income_tax + "','" + ni_charge + "','" + gross + "','" + net + "','" + String.Format("{0:yyyy/MM/dd}", date1) + "')"; //Query that will be executed in MySQL
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
                    Console.WriteLine("PAYSLIP SAVED");
                    Console.WriteLine("PRESS [ENTER] TO GO BACK IN THE MAIN MENU"); // After taking the input from the user, it allows to go back in the main menu
                    break;
            }
            else if (save == "n") {
                    Console.WriteLine("PRESS [ENTER] TO GO BACK IN THE MAIN MENU");// After taking the input from the user, it allows to go back in the main menu
                    Console.ReadLine();
                    break;
            }
            else
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INVALID CHOICE, PRESS [ENTER] TO TRY AGAIN"); // Gives an error if the input is wrong
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
            }
            }
            Console.ReadLine();
        }

        static void viewSlip()
        {

            //This method allows to search for a specific payslip, by typing the ni number of the employee
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n----------------------------------------------------VIEW A PREVIOUS PAYSLIP-----------------------------------------------\n\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("SEARCH FOR A RECORD:\n");
            Regex regexNI2 = new Regex(@"^[a-zA-Z0-9]*$"); // Allows to input only letter and numebrs
            string search = "";
            while (true)
            {
            Console.WriteLine("Enter the NI number of the employee. Leave blank to display all payslips.");
            search = Console.ReadLine();

            if (regexNI2.IsMatch(search) && (search.Length <= 9))
            {
                    break;
            }
            else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Only letters and numbers in this field. Normal NI length is 9 digits.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            string query = "SELECT * FROM payslip_tbl WHERE NI_num LIKE '" + search + "%'";//Query that will be executed in MySQL
            Console.WriteLine();
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                // Shows all the payslip, for a specific NI number, it uses datareaders, to retreive information from the SQL database
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("________________________________________________________________________________________________________________________");
                Console.WriteLine("                                                         BANK OF ERNEST                                                 ");
                Console.WriteLine("________________________________________________________________________________________________________________________");
                Console.WriteLine("---------------------------------------------------DATE: {0:dd/MM/yyyy} ID: {1}--------------------------------------------", dataReader["date_"], dataReader["id"]);
                Console.WriteLine("\n\n    TITLE: {0}                   NAME: {1}              LAST NAME: {2}", dataReader["title"], dataReader["name"], dataReader["last_name"]);
                Console.WriteLine("\n    NI NUMBER: {0}", dataReader["NI_num"]);
                Console.WriteLine("\n    HOURS WORKED: {0}                                     HOURLY RATE: {1:c}/h", dataReader["hours"], dataReader["rate"]);
                Console.WriteLine("\n    OVERTIME: {0}", dataReader["overtime"]);
                Console.WriteLine("\n    INCOME TAX {0}", dataReader["income_tax"]);
                Console.WriteLine("\n    NI CHARGE {0}", dataReader["ni_charge"]);
                Console.WriteLine("\n    GROSS {0:c}", dataReader["gross"]);
                Console.WriteLine("                                                        ____________________________");
                Console.WriteLine("                                                               NET INCOME {0:c}", dataReader["net"]);
                Console.WriteLine("                                                        ____________________________");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n************************************************************************************************************************\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            dataReader.Close();
            connection.Close();
            Console.WriteLine("\nPRESS [ENTER] TO GO BACK IN THE MAIN MENU");
            Console.ReadLine();
        }

        static void deleteSlip()
        {
            // This methos allows to delete a certain payslip
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n----------------------------------------------------DELETE A PAYSLIP-----------------------------------------------\n\n");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
            try { 
            Console.WriteLine("Please enter the ID of the record to delete:");
            int rowid = int.Parse(Console.ReadLine());
            string query = "DELETE FROM payslip_tbl WHERE id='" + rowid + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine("PAYSLIP {0} HAS BEEN DELETED! \n", rowid);
            Console.WriteLine("\nPRESS [ENTER] TO GO BACK IN THE MAIN MENU");
            Console.ReadLine();
                break;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR. Incorrect input, only numbers. Can't leabe balnk");// Error message if the input is incorrect.
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
            }
        }

        static void help()
        {
            //method for opening the HTML file, explaining how to use the program
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n-------------------------------------------------------HELP--------------------------------------------------------\n\n");
            Console.ForegroundColor = ConsoleColor.White;

            string path = Directory.GetCurrentDirectory();
            System.Diagnostics.Process.Start(path + @"\help\index.html");
            Console.WriteLine("A website will open shortly, it will open with your default browser.");
            Console.WriteLine("\n\n\nPRESS [ENTER] TO GO BACK IN THE MAIN MENU");
            Console.ReadLine();
        }
    }
}
