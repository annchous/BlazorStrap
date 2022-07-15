using BlazorComponentUtilities;
using BlazorStrap.Extensions;
using BlazorStrap.Shared.Components.Datatable;

namespace BlazorStrap.V5_1
{
    public partial class BSDataTableRow : BSDataTableRowBase
    {
        protected override string? LayoutClass => LayoutClassBuilder.Build(this);

        protected override string? ClassBuilder => new CssBuilder()
                .AddClass("table-active", IsActive)
                .AddClass($"table-{Color.NameToLower()}", Color != BSColor.Default)
                .AddClass($"align-{AlignRow.NameToLower()}", AlignRow != AlignRow.Default)
                .AddClass(LayoutClass, !string.IsNullOrEmpty(LayoutClass))
                .AddClass(Class, !string.IsNullOrEmpty(Class))
                .Build().ToNullString();
    }
}