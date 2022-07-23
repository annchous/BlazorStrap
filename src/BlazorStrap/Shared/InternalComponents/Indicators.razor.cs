﻿using BlazorStrap.Shared.Components.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorStrap.Shared.InternalComponents
{

    public partial class Indicators : ComponentBase
    {
        int _i = 0;
        [Parameter] public string Version { get; set; } = "V5";
        private int Count { get; set; }
        private int Active { get; set; }
        [CascadingParameter] BSCarouselBase? Parent { get; set; }

        private async Task ClickEvent(int index)
        {
            if (Parent == null) return;
            await Parent.GotoSlideAsync(index);
        }
        private async Task PressEvent(KeyboardEventArgs e)
        {
            if (Parent == null) return;
            if (e.Code == "37")
            {
                await Parent.BackAsync();
            }
            else if (e?.Code == "39")
            {
                await Parent.NextAsync();
            }
        }
        public void Refresh(int count, int active)
        {
            Active = active;
            Count = count;
            StateHasChanged();
        }
    }
}