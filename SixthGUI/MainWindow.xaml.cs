using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace SixthGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IList<string> playerList;

        public MainWindow()
        {
            InitializeComponent();
            playerList = new List<string>();
        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            string sql = SearchPlayersSQLString(txtFirstName.Text, txtLastName.Text);
            DataTable searchResults = executeSQL(sql);

            playerList.Clear();
            foreach (DataRow row in searchResults.Rows)
            {
                playerList.Add(row[0].ToString() + ": " + row[1].ToString() + " " + row[2].ToString());
            }
            populateList();
        }

        private void getPlayerInfo(out string playerID, out string playerName)
        {
            //This is me adding a comment, just to test the pull request functionality
            playerID = "";
            playerName = "";
            //Get the username out of the selected list item
            if (outputListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a player from the list.");
                return;
            }
            string fullItem = outputListBox.SelectedItem.ToString();
            int separatorIndex = fullItem.IndexOf(':');
            playerID = fullItem.Substring(0, separatorIndex);
            playerName = fullItem.Substring(separatorIndex + 1);

        }


        private void triples_button_Click(object sender, RoutedEventArgs e)
        {
            string playerID = "";
            string playerName = "";
            getPlayerInfo(out playerID, out playerName);

            if (playerID != "")
            {
                //Execute query
                DataTable searchResults = executeSQL(CreateTriplesSQLString(playerID));
                string triples = searchResults.Rows[0][0].ToString();
                MessageBox.Show($"{playerName} hit {triples} triples in 1991!");
            }
            else
            {
                MessageBox.Show("Please select a player from the list.");
            }
        }

        private void homeruns_button_Click(object sender, RoutedEventArgs e)
        {
            string playerID = "";
            string playerName = "";
            getPlayerInfo(out playerID, out playerName);

            if (playerID != "")
            {
                //Execute query
                DataTable searchResults = executeSQL(CreateTriplesSQLString(playerID));
                string homeruns = searchResults.Rows[0][0].ToString();
                MessageBox.Show($"{playerName} hit {homeruns} home runs in 1991!");
            }
            else
            {
                MessageBox.Show("Please select a player from the list.");
            }
        }

        private string SearchPlayersSQLString(string firstName, string lastName)
        {
            return $"SELECT playerid, namefirst, namelast FROM Master NATURAL JOIN Batting WHERE yearid = '1991' AND namefirst = '{firstName}' AND namelast = '{lastName}' GROUP BY playerid";
        }

        private string CreateTriplesSQLString(string playerID)
        {
            return $"SELECT Batting.'3B' FROM Master NATURAL JOIN Batting WHERE yearid = '1991' AND playerID = '{playerID}' GROUP BY playerid";
        }

        private string CreateHomerunsSQLString(string playerID)
        {
            return $"SELECT Batting.'3B' FROM Master NATURAL JOIN Batting WHERE yearid = '1991' AND playerID = '{playerID}' GROUP BY playerid";
        }

        private DataTable executeSQL(string sql)
        {
            DataTable dt = new DataTable();

            //Change this if you put your program on a Flash Drive!
            string datasource = @"Data Source=..\..\lahman2016.sqlite;";

            //This is the SQL query to get the players who have hit more than 60 doubles in a single season
            //Keep if you want, but you'll evetually replace this with a new query
            using (SQLiteConnection conn = new SQLiteConnection(datasource))
            {
                conn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
                da.Fill(dt);
                conn.Close();
            }
            return dt;
        }

        private void populateList()
        {
            outputListBox.Items.Clear();
            foreach (string s in playerList)
            {
                outputListBox.Items.Add(s);
            }
        }
    }
}
