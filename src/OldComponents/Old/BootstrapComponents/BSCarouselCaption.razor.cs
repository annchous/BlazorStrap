﻿using BlazorComponentUtilities;
using BlazorStrap.Components.Base;

namespace BlazorStrap
{
    /// <summary>
    /// Adds a caption for a carousel item.
    /// </summary>
    public partial class BSCarouselCaption : LayoutBase
    {
        private string? ClassBuilder => new CssBuilder("carousel-caption")

            .AddClass(LayoutClass, !string.IsNullOrEmpty(LayoutClass))
            .AddClass(Class, !string.IsNullOrEmpty(Class))
            .Build().ToNullString();
    }
}