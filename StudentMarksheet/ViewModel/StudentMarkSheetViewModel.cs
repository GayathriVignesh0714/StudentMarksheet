using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using StudentMarksheet.DataAccess;
using StudentMarksheet.Extensions;
using StudentMarksheet.Model;
using StudentMarksheet.ViewModelBase;
using System.Collections.ObjectModel;
using System.Windows;

namespace StudentMarksheet.ViewModel
{
    public class StudentMarkSheetViewModel : NotificationBase
    {
        #region Readonly Variables

        private readonly ILogger<StudentMarkSheetViewModel> _logger;
        private readonly IStudentMarkSheetDataAccess _studentMarksheetDataAccess;

        #endregion

        #region string properties

        private string _filterRollNumber = string.Empty;
        public string FilterRollNumber
        {
            get { return _filterRollNumber; }
            set
            {
                _filterRollNumber = value;
                NotifyPropertyChanged("FilterRollNumber");
            }
        }

        private string _filterStatus = string.Empty;
        public string FilterStatus
        {
            get { return _filterStatus; }
            set
            {
                _filterStatus = value;
                NotifyPropertyChanged("FilterStatus");
            }
        }

        private int _studentGridCount = 0;
        public int StudentGridCount
        {
            get { return _studentGridCount; }
            set
            {
                _studentGridCount = value;
                NotifyPropertyChanged("StudentGridCount");
            }
        }

        #endregion

        #region Boolean Properties

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        private bool _isFilterEnabled;
        public bool IsFilterEnabled
        {
            get { return _isFilterEnabled; }
            set
            {
                _isFilterEnabled = value;
                NotifyPropertyChanged("IsFilterEnabled");
            }
        }

        #endregion

        #region Collection Properties

        private List<StudentMarkModel> _studentMarksheetList = new List<StudentMarkModel>();

        private ObservableCollection<StudentMarkModel> _studentMarksheetCollection;
        public ObservableCollection<StudentMarkModel> StudentMarksheetCollection
        {
            get { return _studentMarksheetCollection; }
            set
            {
                _studentMarksheetCollection = value;
                if (value == null || value.Count() == 0)
                {
                    IsFilterEnabled = false;
                }
                else
                {
                    IsFilterEnabled = true;
                    StudentGridCount = value.Count();
                }
                NotifyPropertyChanged("StudentMarksheetCollection");
            }
        }

        public ObservableCollection<ExamResult> ExamResultCollection { get; set; }

        private ExamResult _selectedFilterResult = ExamResult.ALL;
        public ExamResult SelectedFilterResult
        {
            get { return _selectedFilterResult; }
            set
            {
                _selectedFilterResult = value;
                NotifyPropertyChanged("SelectedFilterResult");
            }
        }
        #endregion

        #region RelayCommands

        private AsyncRelayCommand<object> _fetchStudentsMarksBtnClick;
        public AsyncRelayCommand<object> FetchStudentsMarksCommand
        {
            get
            {
                return _fetchStudentsMarksBtnClick ?? (_fetchStudentsMarksBtnClick = new AsyncRelayCommand<object>(FetchStudentMarksAsync));
            }
        }

        private ViewModelBase.RelayCommand<object> _filterCommand;
        public ViewModelBase.RelayCommand<object> FilterCommand
        {
            get
            {
                return _filterCommand ?? (_filterCommand = new ViewModelBase.RelayCommand<object>(FilterStudentMarkSheet));
            }
        }

        private ViewModelBase.RelayCommand<object> _clearFilterCommand;
        public ViewModelBase.RelayCommand<object> ClearFilterCommand
        {
            get
            {
                return _clearFilterCommand ?? (_clearFilterCommand = new ViewModelBase.RelayCommand<object>(ClearFilter));
            }
        }

        private ViewModelBase.RelayCommand<object> _openFileBrowserCommand;
        public ViewModelBase.RelayCommand<object> OpenFileBrowserCommand
        {
            get
            {
                return _openFileBrowserCommand ?? (_openFileBrowserCommand = new ViewModelBase.RelayCommand<object>(UploadStudentMarks));
            }
        }

        #endregion

        #region Constructor
        public StudentMarkSheetViewModel(ILogger<StudentMarkSheetViewModel> logger, IStudentMarkSheetDataAccess studentMarksheetDataAccess)
        {
            _logger = logger;
            _studentMarksheetDataAccess = studentMarksheetDataAccess;
            // Populate enum collection
            ExamResultCollection = new ObservableCollection<ExamResult>(Enum.GetValues(typeof(ExamResult)).Cast<ExamResult>());
            SelectedFilterResult = ExamResultCollection.FirstOrDefault();
            FilterStatus = EnumHelper.GetDescription(SelectedFilterResult);
            LoadStudentMarkSheet();
        }

        #endregion

        #region Private Methods

        private void FilterStudentMarkSheet(object obj)
        {
            var allStatus = EnumHelper.GetDescription(ExamResult.ALL);

            try
            {
                var filteredList = _studentMarksheetList
     .Where(x => (string.IsNullOrWhiteSpace(FilterRollNumber) || x.RollNumber.Equals(FilterRollNumber)) &&
                 ((FilterStatus == allStatus) || (x.Status == FilterStatus))).ToList();

                // Initialize or update StudentMarkSheetCollection
                StudentMarksheetCollection.Clear();
                StudentMarksheetCollection.AddRange(filteredList);
                StudentGridCount = StudentMarksheetCollection.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Filtering Student Marksheet Details");
            }
        }

        private void ClearFilter(object obj)
        {
            try
            {
                FilterRollNumber = string.Empty;
                FilterStatus = string.Empty;
                SelectedFilterResult = ExamResult.ALL;

                List<StudentMarkModel> filteredList = _studentMarksheetList;

                if (StudentMarksheetCollection == null)
                {
                    StudentMarksheetCollection = new ObservableCollection<StudentMarkModel>(filteredList);
                }
                else
                {
                    StudentMarksheetCollection.Clear();
                    StudentMarksheetCollection.AddRange(filteredList);
                }
                StudentGridCount = StudentMarksheetCollection.Count();
                NotifyPropertyChanged(nameof(StudentMarksheetCollection));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occurred while Clearing Filter");
            }
        }

        private string OpenCsvFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a CSV File",
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = ".csv",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false // Ensure only one file is selected
            };

            bool? result = openFileDialog.ShowDialog();
            return result == true ? openFileDialog.FileName : string.Empty;
        }

        #endregion

        #region Async Mthods

        private async Task LoadStudentMarkSheet()
        {
            try
            {
                IsLoading = true;
                _logger.LogInformation("Fetching student marks...");

                // Fetch data asynchronously from the API
                var students = await _studentMarksheetDataAccess.FetchStudentMarkSheetAsync();

                //clear before filling the List
                _studentMarksheetList.Clear();

                // Using LINQ for mapping
                _studentMarksheetList = students.Select(student => new StudentMarkModel
                {
                    RollNumber = student.RollNumber,
                    Name = student.Name,
                    Status = student.Status.ToUpper() ?? "PASS", // Fixing the syntax issue
                    IsPassMark = student.Status.ToUpper() == "PASS", // Assigning IsPassMark
                    TotalMarks = student.TotalMarks,
                    Gender = student.Gender,
                    Id = student.Id,
                    MarkList = new List<Marks>  // Assuming MarkList should contain one item with marks
    {
        new Marks
        {
            Maths = student.Marks.Maths,
            Physics = student.Marks.Physics,
            Chemistry = student.Marks.Chemistry,
            SocialStudies = student.Marks.SocialStudies,
            SecondLanguage = student.Marks.SecondLanguage
        }
    }
                }).ToList();


                _logger.LogInformation($"Loaded {students.Count} student marks.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student marks.");
            }
            finally
            {
                IsLoading = false; //Hide Progress Bar
            }
        }

        private async Task FetchStudentMarksAsync(object? arg)
        {
            IsLoading = true; // Show progress bar

            try
            {
                // Ensure the progress bar is rendered before starting heavy work
                await Task.Delay(100);

                _logger.LogInformation("Loading the Student Marksheet to the Grid..");

                // Clear StudentMarksheetCollection before assigning
                if (StudentMarksheetCollection != null && StudentMarksheetCollection.Count > 0)
                {
                    StudentMarksheetCollection.Clear();
                    StudentMarksheetCollection.AddRange(_studentMarksheetList);
                }
                else
                {
                    StudentMarksheetCollection = new ObservableCollection<StudentMarkModel>(_studentMarksheetList);
                }

                StudentGridCount = StudentMarksheetCollection.Count();
                NotifyPropertyChanged(nameof(StudentMarksheetCollection));
            }
            catch (Exception ex)
            {
                // Handle exceptions
                _logger.LogError(ex, $"Error fetching Student marksheet");
            }
            finally
            {
                IsLoading = false; // Hide progress bar
            }
        }
        
        private async Task RefreshEmployeeListAsync()
        {
            _logger.LogInformation("Refresh Student Marksheet called");

            await LoadStudentMarkSheet();
            await FetchStudentMarksAsync(new object());
        }

        private async void UploadStudentMarks(object obj)
        {
            string selectedFilePath = OpenCsvFileDialog();

            try
            {
                // Convert CSV to List
                var csvConverter = new CsvToListConverter();
                List<StudentMarksEntity> newStudents = csvConverter.ConvertCsvToList(selectedFilePath);

                if (newStudents == null || newStudents.Count == 0)
                {
                    MessageBox.Show("No valid student data found in the CSV.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                StudentMarkSheet updatedStudents = new StudentMarkSheet()
                {
                    Students = new List<StudentMarksEntity>(newStudents)
                };

                // Send new students to Data Access Layer for Processing
                bool success = await _studentMarksheetDataAccess.UploadStudentMarksAsync(updatedStudents);

                MessageBox.Show(success ? "Data uploaded successfully!" : "Failed to upload data.",
                    "Upload Status", MessageBoxButton.OK, success ? MessageBoxImage.Information : MessageBoxImage.Error);

                await RefreshEmployeeListAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
