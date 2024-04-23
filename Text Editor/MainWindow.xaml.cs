using System.IO;
using System.Windows;

namespace Text_Editor;
public partial class MainWindow : Window
{
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

    }

    private void Cut_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Copy_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Paste_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Select_Click(object sender, RoutedEventArgs e)
    {

    }
}
