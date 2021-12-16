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
        public Dictionary<int, int> scoring { get; set; }
        public TopPlayers()
        {
            InitializeComponent();
            TextBox textBox = new TextBox();
            int i = 1;
            foreach (var item in scoring)
            {
                textBox.Name = "tb" + i + 0;
                textBox.Text = item.Key.ToString();
                textBox.Name = "tb" + i + 1;
                textBox.Text = item.Value.ToString();
            }
        }
    }
}
