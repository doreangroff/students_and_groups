using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using Group = AvaloniaApplication1.Models.Group;

namespace AvaloniaApplication1;

public partial class MainWindow : Window
{
    private List<Models.Group> Groups { get; set; }
    private MySqlConnectionStringBuilder _connectionSb;
    public MainWindow()
    {
        InitializeComponent();
        Groups = new List<Group>();

        _connectionSb = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "school",
            UserID = "user_1",
            Password = "1234"
        };
        
        UpdateDataGrid();
    }

    private void UpdateDataGrid()
    {
        using (var con = new MySqlConnection(_connectionSb.ConnectionString))
        {
            con.Open();
            using (var com = con.CreateCommand())
            {
                com.CommandText = "SELECT * FROM `groups`";
                using (var reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Groups.Add(new Models.Group()
                        {
                            Id = reader.GetInt32("id"),
                            Group_name = reader.GetString("group_name")
                        });
                    }
                }
            }
            con.Close();
        }

        GroupsDataGrid.ItemsSource = Groups;

    }

    private void StudentsWindowBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Students stud = new Students();
        stud.Show();
        this.Close();
    }
}