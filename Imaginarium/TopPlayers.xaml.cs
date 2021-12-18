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
        Dictionary<int, int> _scoring;
        public TopPlayers(Dictionary<int, int> scoring)
        {
            InitializeComponent();
            _scoring = scoring;

            TextBox textBox = new TextBox();
            int i = 1;

            foreach (var item in _scoring)
            {               
                //((TextBox)textBox.FindName("tb" + i.ToString() + 0.ToString())).Text = item.Key.ToString();
                //((TextBox)textBox.FindName("tb" + i.ToString() + 1.ToString())).Text = item.Value.ToString();

                tb10.Text = item.Key.ToString();
                tb11.Text = item.Value.ToString();
                
                i++;
            }            
        }
    }
}
