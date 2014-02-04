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

namespace CenaPlus.Server.ServerMode.Contest.Problem
{
    /// <summary>
    /// Interaction logic for Code.xaml
    /// </summary>
    public partial class Code : UserControl
    {
        public Code()
        {
            InitializeComponent();
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (string s in GetBlueKeyWords())
            {
                ChangeColor("blue", txtCode, s);
            }
        }
        public void ChangeColor(string l, RichTextBox richBox, string keyword)
        {

            //设置文字指针为Document初始位置
            // richBox.Document.FlowDirection
            TextPointer position = richBox.Document.ContentStart, old = richBox.Selection.Start;
            while (position != null)
            {
                //向前搜索,需要内容为Text
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //拿出Run的Text
                    string text = position.GetTextInRun(LogicalDirection.Forward);
                    //可能包含多个keyword,做遍历查找
                    int index = 0;
                    while (index < text.Length)
                    {
                        index = text.IndexOf(keyword, index);
                        if (index == -1)
                        {
                            break;
                        }
                        else
                        {
                            if (index - 1 >= 0)
                            {
                                foreach (var c in ignore)
                                {
                                    if (c == text[index-1])
                                        goto keywordend;
                                }
                                break;
                            }
                        keywordend:
                            if (index + keyword.Length < text.Length)
                            {
                                foreach (var c in ignore)
                                {
                                    if (c == text[index  + keyword.Length])
                                        goto addrange;
                                }
                                break;
                            }
                        addrange:
                            //添加为新的Range
                            TextPointer start = position.GetPositionAtOffset(index);
                            TextPointer end = start.GetPositionAtOffset(keyword.Length);
                            TextPointer end1 = start.GetPositionAtOffset(index + keyword.Length);
                            selecta(l, richBox, keyword.Length, start, end);

                            index += keyword.Length;
                        }
                    }
                }
                //文字指针向前偏移
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            richBox.Selection.Select(old,old);
        }
        public void selecta(string l, RichTextBox richTextBox1, int selectLength, TextPointer tpStart, TextPointer tpOffset)
        {
            TextRange range = richTextBox1.Selection;
            range.Select(tpStart, tpOffset);

            if (l == "blue")
            {
                range.ApplyPropertyValue(TextElement.ForegroundProperty,
                           new SolidColorBrush(Colors.Orange));
                range.ApplyPropertyValue(TextElement.FontWeightProperty,
                            FontWeights.Bold);
            }
            TextPointer position = richTextBox1.Document.ContentStart;
            TextPointer start = position.GetPositionAtOffset(2);
            TextPointer end = richTextBox1.Document.ContentEnd;
            TextRange range1 = richTextBox1.Selection;
            range1.Select(end, end);
            range1.ClearAllProperties();
            range1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            range1.ApplyPropertyValue(TextElement.ForegroundProperty, txtCode.Foreground);
            
        }
        public string[] GetBlueKeyWords() 
        {
            string[] res = {
                           "int", "long", "float", "double", "string", "#include","#define","const","private","protected","public",
                           "using", "namespace", "return", "break", "continue", "if", "for", "while", "goto", "sizeof", "typedef",
                           "struct", "union", "virtual", "enum", "short", "unsigned", "bool", "inline", "auto", "vector", "list", "queue",
                           "stack", "map", "set","do","NULL","static"
                           };
            return res;
        }
        public char[] ignore = " <>;:{}[]()+-*/&~!=|^?".ToCharArray();
    }
}
