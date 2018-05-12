using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32.TaskScheduler;

namespace TaskScheduler
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> tasks = new ObservableCollection<string>();
        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        public void RegisterTasks()
        {
            
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Spouští podivný exe soubor";
                td.RegistrationInfo.Author = "Opelkuh";
                td.RegistrationInfo.Version = new Version("7.1.3.5");
                td.Triggers.Add(new DailyTrigger(1));
                td.Actions.Add(new ExecAction(System.IO.Path.GetFullPath(@"./ToLaunch.exe")));
                TaskFolder tf = ts.GetFolder(@"\Opelka");
                if (tf == null) ts.RootFolder.CreateFolder("Opelka");
                tf.RegisterTaskDefinition(@"MaxiPig", td);
            }
            LoadTasks();
        }

        public void LoadTasks()
        {
            tasks = new ObservableCollection<string>();
            using (TaskService ts = new TaskService())
            {
                //Daily
                TaskFolder tf = ts.GetFolder(@"\Opelka");
                if(tf != null)
                {
                    foreach(Task task in tf.Tasks)
                    {
                        tasks.Add(task.Name + " - Next run: " + task.NextRunTime.ToString());
                    }
                }
            }
            Display.ItemsSource = tasks;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegisterTasks();
        }
    }
}
