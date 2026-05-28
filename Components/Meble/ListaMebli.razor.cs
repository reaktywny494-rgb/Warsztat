using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Warsztat.Models;
using Warsztat.Services;

namespace Warsztat.Components.Meble
{
    public partial class ListaMebli
    {

        private List<Mebel> meble { get; set; } = new();

        private Mebel nowyMebel { get; set; } = new();

        private Mebel? edytowanyMebel { get; set; }

        private bool showForm { get; set; } = false;

        private int? expandedFurnitureID { get; set; } = null;

        private int? highlightFurnitureId;

        protected override void OnInitialized()
        {
            AppState.Notify += HandleAppStateChange;
        }

        private async void HandleAppStateChange()
        {
            if (AppState.SelectedFurnitureId is not null)
            {
                highlightFurnitureId = AppState.SelectedFurnitureId;

                AppState.SelectedFurnitureId = null;

                await InvokeAsync(StateHasChanged);

                await Task.Delay(50);

                await ScrollToHighlighted();
            }
        }

        public void Dispose()
        {
            AppState.Notify -= HandleAppStateChange;
        }

        private async Task ScrollToHighlighted()
        {
            if (highlightFurnitureId is null)
                return;

            await JS.InvokeVoidAsync("scrollToElement", $"furniture-{highlightFurnitureId}");
        }

        protected override async Task OnParametersSetAsync()
        {
            if (AppState.SelectedFurnitureId is not null)
            {
                highlightFurnitureId = AppState.SelectedFurnitureId;

                AppState.SelectedFurnitureId = null;

                await Task.Yield();

                await ScrollToHighlighted();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            meble = await DbContext.Meble
                .Include(m => m.Narzedzia)
                .ToListAsync();
        }

        private void ToggleForm()
        {
            showForm = !showForm;
        }

        private async Task SaveFurniture()
        {
            if (string.IsNullOrWhiteSpace(nowyMebel.Nazwa))
                return;

            if (nowyMebel.ID == 0)
            {
                DbContext.Meble.Add(nowyMebel);
            }
            else
            {
                var existing = await DbContext.Meble.FindAsync(nowyMebel.ID);

                if (existing != null)
                {
                    existing.Nazwa = nowyMebel.Nazwa;
                    existing.Opis = nowyMebel.Opis;
                }
            }

            await DbContext.SaveChangesAsync();

            nowyMebel = new Mebel();
            edytowanyMebel = null;

            await LoadData();
            showForm = false;
        }

        private async Task DeleteFurniture(int id)
        {
            var mebel = await DbContext.Meble.FindAsync(id);

            if (mebel != null)
            {
                DbContext.Meble.Remove(mebel);

                await DbContext.SaveChangesAsync();

                await LoadData();
            }

            expandedFurnitureID = null;
        }

        private void ToggleFurniture(int id)
        {
            if (expandedFurnitureID == id)
            {
                expandedFurnitureID = null;
            }
            else
            {
                expandedFurnitureID = id;
            }
        }

        private void EditFurniture(Mebel mebel)
        {
            edytowanyMebel = new Mebel
            {
                ID = mebel.ID,
                Nazwa = mebel.Nazwa,
                Opis = mebel.Opis
            };

            nowyMebel = edytowanyMebel;

            showForm = true;
        }
    }
}