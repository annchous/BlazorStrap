﻿using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorStrap
{
    public class BSCol : BlazorStrapBase
    {
        [Parameter] public Align Align { get; set; }
        [Parameter] public string? Column { get; set; }
        [Parameter] public string? ColumnXL { get; set; }
        [Parameter] public string? ColumnXXL { get; set; }
        [Parameter] public string? ColumnLarge { get; set; }
        [Parameter] public string? ColumnMedium { get; set; }
        [Parameter] public string? ColumnSmall { get; set; }
        [Parameter] public string? Offset { get; set; } 
        [Parameter] public string? OffsetXXL { get; set; } 
        [Parameter] public string? OffsetXL { get; set; } 
        [Parameter] public string? OffsetLarge { get; set; } 
        [Parameter] public string? OffsetMedium { get; set; } 
        [Parameter] public string? OffsetSmall { get; set; } 
        [Parameter] public string? Order { get; set; }
        [Parameter] public bool OrderFirst { get; set; }
        [Parameter] public string? OrderLarge { get; set; }
        [Parameter] public string? OrderXL { get; set; }
        [Parameter] public string? OrderXXL { get; set; }
        [Parameter] public bool OrderLast { get; set; }
        [Parameter] public string? OrderMedium { get; set; }
        [Parameter] public string? OrderSmall { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (BlazorStrap.BootstrapVersion == BootstrapVersion.Bootstrap4)
                Version4RenderBuilder(builder);
            else
                Version5RenderBuilder(builder);
        }

        protected override void OnParametersSet()
        {
            if (BlazorStrap.BootstrapVersion == BootstrapVersion.Bootstrap4 && BlazorStrap.ShowDebugMessages)
            {
                if (OrderXXL != null || OffsetXXL != null || ColumnXXL != null)
                    Console.WriteLine("Warning: BSCol, Bootstrap 4 does not support ExtraExtraLarge. Changed to nearest supported ExrtaLarge");
            }

            base.OnParametersSet();
        }

        #region Bootstrap render support methods
        protected override string? ClassBuilder()
        {
            return BlazorStrap.BootstrapVersion == BootstrapVersion.Bootstrap4 ? Version4ClassBuilder() : Version5ClassBuilder();
        }

        #region Bootstrap 4
        [CascadingParameter] public BSRow? BSRow { get; set; }
        protected override string? Version4ClassBuilder()
        {
            return new CssBuilder()
                .AddClass("col", Column == null && ColumnSmall == null && ColumnMedium == null && ColumnLarge == null && ColumnXL == null && ColumnXXL == null)
                .AddClass($"px-{BSRow?.Gutters.ToIndex()} py-{BSRow?.Gutters.ToIndex()}", BSRow?.Gutters != Gutters.Default)
                .AddClass($"px-{BSRow?.HorizontalGutters.ToIndex()}", BSRow?.HorizontalGutters != Gutters.Default)
                .AddClass($"py-{BSRow?.VerticalGutters.ToIndex()}", BSRow?.VerticalGutters != Gutters.Default)
                .AddClass($"col-{Column}", Column.VaildGridSize())
                .AddClass($"col-sm-{ColumnSmall}", ColumnSmall.VaildGridSize())
                .AddClass($"col-md-{ColumnMedium}", ColumnMedium.VaildGridSize())
                .AddClass($"col-lg-{ColumnLarge}", ColumnLarge.VaildGridSize())
                .AddClass($"col-xl-{ColumnXL}", ColumnXL.VaildGridSize())
                .AddClass($"col-xl-{ColumnXXL}", ColumnXXL.VaildGridSize())
                .AddClass($"order-{Order}", Order.VaildGridSize())
                .AddClass($"order-sm-{OrderSmall}", OrderSmall.VaildGridSize())
                .AddClass($"order-md-{OrderMedium}", OrderMedium.VaildGridSize())
                .AddClass($"order-lg-{OrderLarge}", OrderLarge.VaildGridSize())
                .AddClass($"order-xl-{OrderXL}", OrderXL.VaildGridSize())
                .AddClass($"order-xl-{OrderXXL}", OrderXXL.VaildGridSize())
                .AddClass("order-first", OrderFirst)
                .AddClass("order-last", OrderLast)
                .AddClass($"offset-{Offset}", Offset.VaildGridSize())
                .AddClass($"offset-sm-{OffsetSmall}", OffsetSmall.VaildGridSize())
                .AddClass($"offset-md-{OffsetMedium}", OffsetMedium.VaildGridSize())
                .AddClass($"offset-lg-{OffsetLarge}", OffsetLarge.VaildGridSize())
                .AddClass($"offset-xl-{OffsetXL}", OffsetXL.VaildGridSize())
                .AddClass($"offset-xl-{OffsetXXL}", OffsetXXL.VaildGridSize())
                .AddClass($"align-self-{Align.NameToLower()}", Align != Align.Default)
                .AddClass(LayoutClass, !string.IsNullOrEmpty(LayoutClass))
                .AddClass(Class, !string.IsNullOrEmpty(Class))
                .Build().ToNullString();
        }

        protected override void Version4RenderBuilder(RenderTreeBuilder builder)
        {
            Version5RenderBuilder(builder);
        }
        #endregion

        #region Bootstrap 5
        protected override string? Version5ClassBuilder()
        {
            return new CssBuilder()
                .AddClass("col", Column == null && ColumnSmall == null && ColumnMedium == null && ColumnLarge == null && ColumnXL == null && ColumnXXL == null)
                .AddClass($"col-{Column}", Column.VaildGridSize())
                .AddClass($"col-sm-{ColumnSmall}", ColumnSmall.VaildGridSize())
                .AddClass($"col-md-{ColumnMedium}", ColumnMedium.VaildGridSize())
                .AddClass($"col-lg-{ColumnLarge}", ColumnLarge.VaildGridSize())
                .AddClass($"col-xl-{ColumnXL}", ColumnXL.VaildGridSize())
                .AddClass($"col-xxl-{ColumnXXL}", ColumnXXL.VaildGridSize())
                .AddClass($"order-{Order}", Order.VaildGridSize())
                .AddClass($"order-sm-{OrderSmall}", OrderSmall.VaildGridSize())
                .AddClass($"order-md-{OrderMedium}", OrderMedium.VaildGridSize())
                .AddClass($"order-lg-{OrderLarge}", OrderLarge.VaildGridSize())
                .AddClass($"order-xl-{OrderXL}", OrderXL.VaildGridSize())
                .AddClass($"order-xxl-{OrderXXL}", OrderXXL.VaildGridSize())
                .AddClass("order-first", OrderFirst)
                .AddClass("order-last", OrderLast)
                .AddClass($"offset-{Offset}", Offset.VaildGridSize())
                .AddClass($"offset-sm-{OffsetSmall}", OffsetSmall.VaildGridSize())
                .AddClass($"offset-md-{OffsetMedium}", OffsetMedium.VaildGridSize())
                .AddClass($"offset-lg-{OffsetLarge}", OffsetLarge.VaildGridSize())
                .AddClass($"offset-xl-{OffsetXL}", OffsetXL.VaildGridSize())
                .AddClass($"offset-xxl-{OffsetXXL}", OffsetXXL.VaildGridSize())
                .AddClass($"align-self-{Align.NameToLower()}", Align != Align.Default)
                .AddClass(LayoutClass, !string.IsNullOrEmpty(LayoutClass))
                .AddClass(Class, !string.IsNullOrEmpty(Class))
                .Build().ToNullString();
        }

        protected override void Version5RenderBuilder(RenderTreeBuilder builder)
        {
            var s = 0;
            builder.OpenElement(s, "div");
            builder.AddAttribute(s++, "class", ClassBuilder());
            builder.AddMultipleAttributes(s++, Attributes);
            builder.AddContent(s++, ChildContent);
            builder.CloseElement();
        }
        #endregion
        #endregion
    }
}
