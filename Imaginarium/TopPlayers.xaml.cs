using Imaginarium.ServiceReference1;
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
using System.Windows.Shapes;

namespace Imaginarium
{
    /// <summary>
    /// Логика взаимодействия для TopPlayers.xaml
    /// </summary>
    public partial class TopPlayers : Window
    {
        Dictionary<string, int> _scoring;
        public static bool signal { get; set; }
        public static int ID { get; set; }
        public ServiceGameClient client { get; set; }
        public TopPlayers(Dictionary<string, int> scoring)
        {
            InitializeComponent();
            if (signal)
            {
                tbName.Text = "Игра окончена";
            }
            _scoring = scoring;
            var textBox1Name = (TextBox)this.FindName($"tb00");
            var textBox2Name = (TextBox)this.FindName($"tb01");
            textBox1Name.Text = "Имена игроков";
            textBox2Name.Text = "Количество очков";
            int i = 5;

            foreach (var item in _scoring)
            {
                var textBox1 = (TextBox)this.FindName($"tb{i}0");
                var textBox2 = (TextBox)this.FindName($"tb{i}1");

                textBox1.Text = item.Key.ToString();
                textBox2.Text = item.Value.ToString();
                
                i--;
            }            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (signal)
            {
                client.Disconnect(ID);
            }
        }
    }
}
