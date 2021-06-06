using Microsoft.Win32;
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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.IO;
using CenterOfCreativity.BaseModel;

namespace CenterOfCreativity.Interface
{
    /// <summary>
    /// Логика взаимодействия для PrintPage.xaml
    /// </summary>
    public partial class PrintPage : Page
    {
        public PrintPage()
        {
            InitializeComponent();
            dataGridSchedule.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Schedule.ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XPS Files (*.xps)|*.xps";
            if (saveFileDialog.ShowDialog() == true)
            {
                XpsDocument doc = new XpsDocument(saveFileDialog.FileName, FileAccess.Write);
                XpsDocumentWriter writerDoc = XpsDocument.CreateXpsDocumentWriter(doc);
                writerDoc.Write(docViewerSchedule.Document as FixedDocument);
                doc.Close();
            }
        }
    }
}
