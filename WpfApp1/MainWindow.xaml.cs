using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;




namespace WpfApp1
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
  
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            ListBox lb = new ListBox();
            Label lbl = new Label();
            lbl.Content = "11";
            lb.Items.Add(lbl);
            
            Program program = new Program();
            program.InitializingNotifications();
            
            //Notification nf = program.CreateNotification();
            
            program.CloseNotifications();
          
        }
        

        public Notification InitNotification(string description, DateTime dataEvent, Color prioritet)
        {
            Border borderNf = new Border();
            borderNf.Width = 200;
            borderNf.Height = 500;
            Label lb = new Label();
            lb.Content = "Desription";
            
            return new Notification();
        }

     
    }

    class FrameProgram : Window
    {
        Border border = new Border();
        Program program;
        List<FrameNotification> ListFrameNotification = new List<FrameNotification>();
        public FrameProgram(Program program)
        {
            this.program = program;
            InitFrameProgram();
        }
        public void InitFrameProgram() // добавляет все уведомления
        {
           foreach(var item in program.listNotification.EnumerableNotifications)
           {
                ListFrameNotification.Add(new FrameNotification(item));
                
           }

        }

        
    }
    class FrameNotification : Window
    {
        Border border = new Border();
        Notification notification;
        public struct Elements
        {
            public Label description;
            public Border prioritet;
            public DatePicker dateEvent;
            public ListBox dateList;
            public Button remove;
        }
        Elements elements;
        public FrameNotification(Notification notification)
        {
            this.notification = notification;
            CreateFrameNotification();
        }

        public void CreateFrameNotification()
        {
            elements.description = new Label();
            elements.description.Content = this.notification.Description;

            elements.prioritet = new Border();
            elements.prioritet.Background = new SolidColorBrush(this.notification.Prioritet);

            elements.dateEvent = new DatePicker();
            elements.dateEvent.DisplayDate = this.notification.DateEvent;

            elements.dateList = new ListBox();
            
            foreach (var date in this.notification.DatesNotification)
            {
                elements.dateList.Items.Add(new DateFrame(date));
               
            }

            elements.remove = new Button();
            elements.remove.Content = "x";
            elements.remove.Click += RemoveFrameNotification;


        }
        
        public void RemoveFrameNotification(object sender, RoutedEventArgs e)
        {

        }
        public void HideFrameNotification()
        {

        }
    }
    class DateFrame : Window //добавляется в скписок напоминаний
    {
        public DateFrame(DateTime date)
        {

        }

    }
  /// <summary>
  /// Класс всей программы содержит список уведомлений
  /// сохраняет данные локально
  /// </summary>
    class Program
    {
        public ListNotifications listNotification = new ListNotifications();
        private int count = 0;
        public Program()
        {

        }
        public int Count {get;}
        public void InitializingNotifications()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("list.dat", FileMode.OpenOrCreate))
            {
                try
                {
                    listNotification = (ListNotifications)formatter.Deserialize(fs);
                }
                catch { MessageBox.Show("Reading failed..."); }
            }

        }
        public Notification CreateNotification()
        {
            Notification nf = new Notification();
            listNotification.EnumerableNotifications.Add(nf);
            count++;
            return nf;
        }
        
        public int RemoveNotification(Notification nf)
        {
            try
            {
                listNotification.EnumerableNotifications.Remove(nf);
            }
            catch (Exception ex)
            { return -1; }
            return 0;
        }
        public void CloseNotifications()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("list.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, listNotification);
            }
        }

    }
    [Serializable]


// служит для нормальной сериализации
    class ListNotifications
    {
        public List<Notification> EnumerableNotifications = new List<Notification>();
    }
    [Serializable]
    public class Notification
    {
        private static int count = 0;
        private string description;
        private DateTime dateEvent;
        private List<DateTime> datesNotification = new List<DateTime>();
        [NonSerialized]
        private Color prioritet;

        [NonSerialized]
        readonly List<Color> PRIORITES = new List<Color>()
        {
        Color.FromRgb(243,128,83),                         //High
        Color.FromRgb(133,230,244),              //Middle
        Color.FromRgb(97,248,104)                          //Low
        };
        //readonly string[] PRIORITES = { "#FF4F20", "#67F2F2", "#7DF271" };
        public Notification()
        {

            this.description = ".";
            this.dateEvent = DateTime.Now;
            this.prioritet = Color.FromRgb(133,230,244); 
        }
        
      
        public List<DateTime> DatesNotification { get; }
        public string Description
        {
            get { return description; }
            set { description = value.ToString(); }
        }
        public DateTime DateEvent
        {
            get { return dateEvent; }
            set { dateEvent = value;  }
        }

        public void AddDateNotification(DateTime dateTime)
        {
            datesNotification.Add(dateTime);
        }
        public void RemoveDateNotification(DateTime dateTime)
        {
            datesNotification.Remove(dateTime);
        }
        
        public Color Prioritet
        {
            get { return prioritet; }
            set { prioritet = value; }
        }

    }
}
