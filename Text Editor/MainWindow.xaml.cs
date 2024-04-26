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
    public List<string> FontList = new();
    public List<int> SizeList = new();
    string CopyText = "";
    string filePath = "";
    bool weight = false;
    bool italic = false;
    bool underLine = false;
    public MainWindow()
    {
        InitializeComponent();
        foreach (var item in FontFamily.FamilyNames)
            FontList.Add(item.ToString());
        for (int i = 8; i < 50; i++)
            SizeList.Add(i);
        Sizes.ItemsSource = SizeList;
        Sizes.SelectedIndex = 0;
        Fonts.ItemsSource = FontList;
        Fonts.SelectedIndex = 0;
    }
    private void Load_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog();
        dialog.FileName = "Document";
        dialog.DefaultExt = ".txt";
        dialog.Filter = "Text documents|*.txt";

        if (dialog.ShowDialog() == true)
            try
            {
                var text = File.ReadAllText(dialog.FileName);
                Richtext.Document.Blocks.Clear();
                Richtext.AppendText(text);
                filePath = dialog.FileName;
                Search.Text = filePath;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.Message}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = Search.Text == "" ? "Document" : Search.Text,
                DefaultExt = ".txt",
                Filter = "Text documents|*.txt"
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, new TextRange(Richtext.Document.ContentStart, Richtext.Document.ContentEnd).Text);
                filePath = dialog.FileName;
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show($"{exception.Message}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
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

    private void Select_Click(object sender, RoutedEventArgs? e)
    {
        Richtext.SelectAll();
        Richtext.SelectionBrush.GetAsFrozen();
    }

    private void Bold_Click(object sender, RoutedEventArgs e)
    {
        if (weight == false)
        {
            if (Richtext.Selection.Text.Length > 0)
                Richtext.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            else
            {
                Select_Click(this, null);
                Richtext.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            weight = true;
        }
        else
        {
            if (Richtext.Selection.Text.Length > 0)
                Richtext.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            else
            {
                Select_Click(this, null);
                Richtext.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
            weight = false;
        }
    }
    private void Italic_Click(object sender, RoutedEventArgs e)
    {
        if (italic == false)
        {
            if (Richtext.Selection.Text.Length > 0)
                Richtext.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            else
            {
                Select_Click(this, null);
                Richtext.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            italic = true;
        }
        else
        {
            if (Richtext.Selection.Text.Length > 0)
                Richtext.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            else
            {
                Select_Click(this, null);
                Richtext.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
            italic = false;
        }
    }
    private void Underline_Click(object sender, RoutedEventArgs e)
    {
        if (underLine == false)
        {
            if (Richtext.Selection.Text.Length > 0)
                Richtext.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                Select_Click(this, null);
                Richtext.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            underLine = true;
        }
        else
        {
            if (Richtext.Selection.Text.Length > 0)
                Richtext.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            else
            {
                Select_Click(this, null);
                Richtext.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
            underLine = false;
        }

    }
    private void Color_Click(object sender, RoutedEventArgs e)
    {
        var btn = sender as Button;
        var brush = btn?.Background as SolidColorBrush;
        if (brush != null)
            if (Richtext.Selection.Text.Length > 0)
            {
                TextRange range = new TextRange(Richtext.Selection.Start, Richtext.Selection.End);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            }
            else
            {
                Select_Click(this, null);
                TextRange range = new TextRange(Richtext.Selection.Start, Richtext.Selection.End);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            }
        else
            MessageBox.Show("Color can not handled correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

    }
    private void Richtext_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Check.IsChecked == true)
        {
            File.WriteAllText(Search.Text, Richtext.ToString());
        }
    }

    private void Fonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        FontFamily fontFamily = new FontFamily(Fonts.SelectedItem.ToString());

        if (Richtext.Selection.Text.Length == 0)
            Select_Click(this, null);

        TextRange range = new TextRange(Richtext.Selection.Start, Richtext.Selection.End);
        range.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
    }

    private void Sizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Richtext.Selection.Text.Length == 0)
            Select_Click(null, null);
        TextRange range = new TextRange(Richtext.Selection.Start, Richtext.Selection.End);
        range.ApplyPropertyValue(TextElement.FontSizeProperty, Convert.ToDouble(Sizes.SelectedItem));
    }

    private void Check_Checked(object sender, RoutedEventArgs e)
    {
        if (Search.Text.Length == 0)
        {
            MessageBox.Show("Save or Load file first", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            Check.IsChecked = false;
        }
    }
}
