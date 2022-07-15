using BlazorComponentUtilities;
using BlazorStrap.Extensions;
using BlazorStrap.Interfaces;
using BlazorStrap.Shared.Components.Content;

namespace BlazorStrap.V5_1
{
    public partial class BSTHead : BSTHeadBase, IBSTHead
    {
        protected override string? LayoutClass => LayoutClassBuilder.Build(this);

        protected override string? ClassBuilder => new CssBuilder()
                .AddClass(LayoutClass, !string.IsNullOrEmpty(LayoutClass))
                .AddClass(Class, !string.IsNullOrEmpty(Class))
                .Build().ToNullString();
    }
}