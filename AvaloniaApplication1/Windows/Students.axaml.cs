using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.Models;
using MySql.Data.MySqlClient;

namespace AvaloniaApplication1;

public partial class Students : Window
{
    private List<Models.Student> Students_list { get; set; }
    private MySqlConnectionStringBuilder _connectionSb;
    public Students()
    {
        InitializeComponent();
        Students_list = new List<Student>();

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
                com.CommandText = "SELECT surname, name, group_name FROM `students` "+
                                  " join `groups` g on g.id = students.group_id";
                using (var reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Students_list.Add(new Models.Student()
                        {
                            Surname = reader.GetString("surname"),
                            Name = reader.GetString("name"),
                            Group_name = reader.GetString("group_name")
                        });
                    }
                }
            }
            con.Close();
        }

        StudentsDataGrid.ItemsSource = Students_list;
    }

    private void GroupsWindowBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow gr = new MainWindow();
        gr.Show();
        this.Close();
    }
}