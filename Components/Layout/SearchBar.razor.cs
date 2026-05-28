using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Warsztat.Models;
using Warsztat.Services;

namespace Warsztat.Components.Layout
{
    public partial class SearchBar
    {
        [Parameter] public EventCallback<int> ToolSelected { get; set; }

        [Parameter] public EventCallback<int> FurnitureSelected { get; set; }

        private CancellationTokenSource? debounceCts;

        private string searchText { get; set; } = "";

        private List<SearchResult> results { get; set; } = new();

        private async Task HandleSearch(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? "";

            var term = searchText?.Trim();

            debounceCts?.Cancel();
            debounceCts = new CancellationTokenSource();

            try
            {
                await Task.Delay(300, debounceCts.Token);

                await Search(term);

                StateHasChanged();
            }
            catch (TaskCanceledException) { }
        }

        private async Task Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
            {
                results.Clear();
                return;
            }

            var toolResults = await DbContext.Narzedzia
                .Where(t => t.Nazwa.Contains(term))
                .Select(t => new SearchResult
                {
                    Type = "tool",
                    Title = t.Nazwa,
                    ID = t.ID
                })
                .ToListAsync();

            var furnitureResults = await DbContext.Meble
                .Where(m => m.Nazwa.Contains(term))
                .Select(m => new SearchResult
                {
                    Type = "furniture",
                    Title = m.Nazwa,
                    ID = m.ID
                })
                .ToListAsync();

            results = toolResults
                .Concat(furnitureResults)
                .ToList();
        }

        private void Navigate(SearchResult result)
        {
            results.Clear();
            searchText = "";

            if (result.Type == "tool")
            {
                AppState.SelectedToolId = result.ID;
                Nav.NavigateTo("/narzedzia");
            }
            else if (result.Type == "furniture")
            {
                AppState.SelectedFurnitureId = result.ID;
                Nav.NavigateTo("/meble");
            }
        }
    }
}