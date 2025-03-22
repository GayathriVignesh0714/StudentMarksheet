using System.Windows;

namespace StudentMarksheet.Services
{
    public class WindowService : IWindowService
    {
        private readonly Dictionary<object, Window> _openWindows = new();

        public void OpenWindow(Type windowType, object viewModel)
        {
            if (windowType == null)
            {
                throw new ArgumentNullException(nameof(windowType), "The resolved window type cannot be null.");
            }

            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel), "The ViewModel cannot be null.");
            }

            // Attempt to create an instance of the specified window type
            if (Activator.CreateInstance(windowType) is not Window window)
            {
                throw new InvalidOperationException($"The type '{windowType.FullName}' is not a valid WPF Window.");
            }

            // Assign the DataContext
            window.DataContext = viewModel;

            // Track the opened window
            _openWindows[viewModel] = window;

            // Show the window as a dialog
            window.ShowDialog();
        }

        public void CloseWindow(object viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel), "ViewModel cannot be null.");
            }

            // Find the window associated with the ViewModel
            if (_openWindows.TryGetValue(viewModel, out var window))
            {
                // Close the window if it's still open
                if (window.IsLoaded)
                {
                    window.Close();
                }

                // Remove it from the tracked list
                _openWindows.Remove(viewModel);
            }
            else
            {
                throw new InvalidOperationException("No active window found for the provided ViewModel.");
            }
        }
    }
}
