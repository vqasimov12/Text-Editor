using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Text_Editor;
public partial class MainWindow : Window
{
    List<string> FontList = new();
    List<int> SizeList = new();
    string CopyText = "";
    public MainWindow()
    {
        InitializeComponent();

    }
    private void Load_Click(object sender, RoutedEventArgs e)
    {
        if (Search.Text == "")
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; 
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                var text=File.ReadAllText(dialog.FileName);
                Richtext.AppendText(text);
            }
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = "Document",
            DefaultExt = ".txt",
            Filter = "Text documents|*.txt"
        };
        bool? result = dialog.ShowDialog();
        if (result == true)
            File.WriteAllText(dialog.FileName, new TextRange(Richtext.Document.ContentStart, Richtext.Document.ContentEnd).Text);
    }

    private void Cut_Click(object sender, RoutedEventArgs e)
    {
        TextRange range = new TextRange(Richtext.Selection.Start, Richtext.Selection.End);
        CopyText = range.Text;
        range.Text = "";
    }

    private void Copy_Click(object sender, RoutedEventArgs e)
    {
        CopyText = new TextRange(Richtext.Selection.Start, Richtext.Selection.End).Text;
    }

    private void Paste_Click(object sender, RoutedEventArgs e)
    {
       TextPointer insertionPosition = Richtext.CaretPosition;
        insertionPosition.InsertTextInRun(CopyText);
    }

    private void Select_Click(object sender, RoutedEventArgs e)
    {
        //TextRange range = new TextRange(Richtext.SelectAll, Richtext.End);
        Richtext.SelectAll(); 
        Richtext.SelectionBrush.GetAsFrozen();
    }

    private void Bold_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Italic_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Underline_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Color_Click(object sender, RoutedEventArgs e)
    {
        TextRange range = new TextRange(Richtext.Selection.Start, Richtext.Selection.End);
        range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red); // Change the color

    }
}
