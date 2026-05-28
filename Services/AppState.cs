namespace Warsztat.Services
{
    public class AppState
    {
        private int? _selectedToolId;
        private int? _selectedFurnitureId;

        public int? SelectedToolId
        {
            get => _selectedToolId;
            set
            {
                _selectedToolId = value;
                Notify?.Invoke();
            }
        }

        public int? SelectedFurnitureId
        {
            get => _selectedFurnitureId;
            set
            {
                _selectedFurnitureId = value;
                Notify?.Invoke();
            }
        }

        public event Action? Notify;
    }
}
