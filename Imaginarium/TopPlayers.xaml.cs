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
        public TopPlayers(Dictionary<string, int> scoring)
        {
            InitializeComponent();
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
                //((TextBox)textBox.FindName("tb" + i.ToString() + 0.ToString())).Text = item.Key.ToString();
                //((TextBox)textBox.FindName("tb" + i.ToString() + 1.ToString())).Text = item.Value.ToString();

                textBox1.Text = item.Key.ToString();
                textBox2.Text = item.Value.ToString();
                
                i--;
            }            
        }
    }
}
