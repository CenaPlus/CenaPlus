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

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for AnswerPush.xaml
    /// </summary>
    public partial class AnswerPush : UserControl
    {
        public AnswerPush(Entity.Question Question)
        {
            InitializeComponent();
            lbQuestion.Text = Question.Description;
            lbAnswer.Text = Question.Answer;
        }
    }
}
