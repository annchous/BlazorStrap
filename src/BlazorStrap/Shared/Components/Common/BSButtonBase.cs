﻿using Microsoft.AspNetCore.Components;

namespace BlazorStrap.Shared.Components.Common
{
    public abstract class BSButtonBase<TSize> : BlazorStrapActionBase<TSize> where TSize : Enum
    {
        /// <summary>
        /// Whether or not the button type is Reset.
        /// </summary>
        [Parameter]
        public bool IsReset
        {
            get => IsResetType;
            set => IsResetType = value;
        }

        /// <summary>
        /// Whether or not the button type is Submit.
        /// </summary>
        [Parameter]
        public bool IsSubmit
        {
            get => IsSubmitType;
            set => IsSubmitType = value;
        }

        /// <summary>
        /// Whether or not the button is a link
        /// </summary>
        [Parameter]
        public bool IsLink
        {
            get => HasLinkClass;
            set => HasLinkClass = value;
        }

        public BSButtonBase()
        {
            HasButtonClass = true;
        }
    }
}
