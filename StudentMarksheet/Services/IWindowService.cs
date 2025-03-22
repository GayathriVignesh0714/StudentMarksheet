namespace StudentMarksheet.Services
{
    public interface IWindowService
    {
        void OpenWindow(Type windowType, object viewModel);
        void CloseWindow(object viewModel);
    }
}
