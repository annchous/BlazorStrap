using BlazorComponentUtilities;
using BlazorStrap.Components.Base;
using Microsoft.AspNetCore.Components;

namespace BlazorStrap
{
    public partial class BSModalContent : LayoutBase
    {
        [Parameter] public BSColor ModalColor { get; set; } = BSColor.Default;
        private string? ClassBuilder => new CssBuilder("modal-body")
            .AddClass($"bg-{ModalColor.NameToLower()}", ModalColor != BSColor.Default)
            .AddClass(Class, !string.IsNullOrEmpty(Class))
            .Build().ToNullString();
    }
}