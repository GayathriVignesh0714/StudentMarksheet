using StudentMarksheet.ViewModel;
using System.Windows;

namespace StudentMarksheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StudentMarksheetView : Window
    {
        public StudentMarksheetView(StudentMarkSheetViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}