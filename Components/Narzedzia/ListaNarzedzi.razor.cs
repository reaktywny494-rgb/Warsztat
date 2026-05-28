using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Warsztat.Models;
using Warsztat.Services;

namespace Warsztat.Components.Narzedzia
{
    public partial class ListaNarzedzi
    {

        private List<Narzedzie> narzedzia { get; set; } = new();

        private List<Mebel> meble { get; set; } = new();

        private Mebel? wybranyMebel => meble.FirstOrDefault(m => m.ID == noweNarzedzie.Mebel_ID);

        private Narzedzie noweNarzedzie { get; set; } = new();

        private Narzedzie? edytowaneNarzedzie { get; set; }

        private bool showForm { get; set; } = false;

        private int? highlightToolId;

        protected override void OnInitialized()
        {
            AppState.Notify += HandleAppStateChange;
        }

        private async void HandleAppStateChange()
        {
            if (AppState.SelectedToolId is not null)
            {
                highlightToolId = AppState.SelectedToolId;

                AppState.SelectedToolId = null;

                await InvokeAsync(StateHasChanged);

                await Task.Delay(50);

                await ScrollToHighlighted();
            }
        }

        public void Dispose()
        {
            AppState.Notify -= HandleAppStateChange;
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            narzedzia = await DbContext.Narzedzia
                .Include(t => t.Mebel)
                .ToListAsync();

            meble = await DbContext.Meble.ToListAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (AppState.SelectedToolId is not null)
            {
                highlightToolId = AppState.SelectedToolId;

                AppState.SelectedToolId = null;

                await Task.Yield();

                await ScrollToHighlighted();
            }
        }

        private async Task ScrollToHighlighted()
        {
            if (highlightToolId is null)
                return;

            await JS.InvokeVoidAsync("scrollToElement", $"tool-{highlightToolId}");
        }

        private void ToggleForm()
        {
            showForm = !showForm;
        }

        private async Task SaveTool()
        {
            if (string.IsNullOrWhiteSpace(noweNarzedzie.Nazwa))
                return;

            if (noweNarzedzie.ID == 0)
            {
                DbContext.Narzedzia.Add(noweNarzedzie);
            }
            else
            {
                var existing = await DbContext.Narzedzia.FindAsync(noweNarzedzie.ID);

                if (existing != null)
                {
                    existing.Nazwa = noweNarzedzie.Nazwa;
                    existing.Opis = noweNarzedzie.Opis;
                    existing.Mebel_ID = noweNarzedzie.Mebel_ID;
                }
            }

            await DbContext.SaveChangesAsync();

            noweNarzedzie = new Narzedzie();
            edytowaneNarzedzie = null;

            await LoadData();
            showForm = false;
        }

        private async Task DeleteTool(int id)
        {
            var narzedzie = await DbContext.Narzedzia.FindAsync(id);

            if (narzedzie != null)
            {
                DbContext.Narzedzia.Remove(narzedzie);

                await DbContext.SaveChangesAsync();

                await LoadData();
            }
        }

        private void EditTool(Narzedzie narzedzie)
        {
            edytowaneNarzedzie = new Narzedzie
            {
                ID = narzedzie.ID,
                Nazwa = narzedzie.Nazwa,
                Opis = narzedzie.Opis,
                Mebel_ID = narzedzie.Mebel_ID
            };

            noweNarzedzie = edytowaneNarzedzie;

            showForm = true;
        }
    }
}