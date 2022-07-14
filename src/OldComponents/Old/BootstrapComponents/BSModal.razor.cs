using BlazorComponentUtilities;
using BlazorStrap.Bootstrap.Common.Enums.Public;
using BlazorStrap.Bootstrap.V5_1.Enums;
using BlazorStrap.InternalComponents;
using BlazorStrap.Service;
using BlazorStrap.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorStrap
{
    public partial class BSModal : BlazorStrapToggleBase<BSModal>, IAsyncDisposable
    {
        private Func<Task>? _callback;
        private DotNetObjectReference<BSModal>? _objectRef;
        private bool _lock;

        /// <summary>
        /// Color of modal. Defaults to <see cref="BSColor.Default"/>
        /// </summary>
        [Parameter] public BSColor ModalColor { get; set; } = BSColor.Default;

        /// <summary>
        /// Allows the page to be scrolled while the Modal is being shown.
        /// </summary>
        [Parameter] public bool AllowScroll { get; set; }

        /// <summary>
        /// CSS classes to be added to Modal activation button.
        /// </summary>
        [Parameter] public string? ButtonClass { get; set; }

        /// <summary>
        /// CSS classes to add to modal dialog
        /// </summary>
        [Parameter] public string? DialogClass { get; set; }

        /// <summary>
        /// Modal content.
        /// </summary>
        [Parameter] public RenderFragment? Content { get; set; }

        /// <summary>
        /// CSS classes to apply to the modal content.
        /// </summary>
        [Parameter] public string? ContentClass { get; set; }

        /// <summary>
        /// Modal footer.
        /// </summary>
        [Parameter] public RenderFragment<BSModal>? Footer { get; set; }

        /// <summary>
        /// CSS classes to be applied to the modal footer.
        /// </summary>
        [Parameter] public string? FooterClass { get; set; }

        /// <summary>
        /// Sets the full screen modal size. Only has effect if <see cref="IsFullScreen"/> is true.
        /// See <see href="https://getbootstrap.com/docs/5.2/components/modal/#fullscreen-modal">Bootstrap Documentation</see>
        /// </summary>
        [Parameter] public Size FullScreenSize { get; set; } = Size.None;

        /// <summary>
        /// Modal header content.
        /// </summary>
        [Parameter] public RenderFragment? Header { get; set; }

        /// <summary>
        /// CSS classes to be applied to the modal header.
        /// </summary>
        [Parameter] public string? HeaderClass { get; set; }

        /// <summary>
        /// Centers the modal.
        /// </summary>
        [Parameter] public bool IsCentered { get; set; }

        /// <summary>
        /// Enables the modal to be full screen. Set the size with <see cref="FullScreenSize"/>
        /// </summary>
        [Parameter] public bool IsFullScreen { get; set; }

        /// <summary>
        /// Whether or not the modal is scrollable.
        /// </summary>
        [Parameter] public bool IsScrollable { get; set; }

        /// <summary>
        /// Adds a close button to the modal.
        /// </summary>
        [Parameter] public bool HasCloseButton { get; set; } = true;

        /// <summary>
        /// Enables the static backdrop. 
        /// See <see href="https://getbootstrap.com/docs/5.2/components/modal/#static-backdrop">Bootstrap Documentation</see>
        /// </summary>
        [Parameter] public bool IsStaticBackdrop { get; set; }

        /// <summary>
        /// Show backdrop. Defaults to true.
        /// </summary>
        [Parameter] public bool ShowBackdrop { get; set; } = true;

        /// <summary>
        /// Sets modal size.
        /// See <see href="https://getbootstrap.com/docs/5.2/components/modal/#optional-sizes">Bootstrap Documentation</see>
        /// </summary>
        [Parameter] public Size Size { get; set; } = Size.None;

        private bool _leaveBodyAlone;

        private bool _shown;
        private Backdrop? BackdropRef { get; set; }

        protected override bool ShouldRender()
        {
            return !_lock;
        }

        private string? ClassBuilder => new CssBuilder("modal")
            .AddClass("fade")
            .AddClass("show", Shown)
            //     .AddClass($"modal-{Size.ToDescriptionString()}", Size != Size.None)
            .AddClass(LayoutClass, !string.IsNullOrEmpty(LayoutClass))
            .AddClass(Class, !string.IsNullOrEmpty(Class))
            .Build().ToNullString();

        private string? BodyClassBuilder => new CssBuilder("modal-body")
            .AddClass(ContentClass)
            .Build().ToNullString();

        private string? ContentClassBuilder => new CssBuilder("modal-content")
            .AddClass($"bg-{ModalColor.NameToLower()}", ModalColor != BSColor.Default)
            .Build().ToNullString();

        private string? DialogClassBuilder => new CssBuilder("modal-dialog")
            .AddClass("modal-fullscreen", IsFullScreen && FullScreenSize == Size.None)
            .AddClass($"modal-fullscreen-{FullScreenSize.ToDescriptionString()}-down", FullScreenSize != Size.None)
            .AddClass("modal-dialog-scrollable", IsScrollable)
            .AddClass("modal-dialog-centered", IsCentered)
            .AddClass((IsScrollable ? "modal-dialog-scrollable" : string.Empty))
            .AddClass($"modal-{Size.ToDescriptionString()}", Size != Size.None)
            .AddClass("modal-dialog-centered", IsCentered)
            .AddClass(DialogClass)
            .Build().ToNullString();

        private string? HeaderClassBuilder => new CssBuilder("modal-header")
            .AddClass(HeaderClass)
            .Build().ToNullString();

        private string? FooterClassBuilder => new CssBuilder("modal-footer")
            .AddClass(FooterClass)
            .Build().ToNullString();

        private ElementReference? MyRef { get; set; }

        /// <summary>
        /// Whether or not modal is shown.
        /// </summary>
        public bool Shown
        {
            get => _shown;
            private set => _shown = value;
        }

        private string Style { get; set; } = "display: none;";

        private async Task TryCallback(bool renderOnFail = true)
        {
            try
            {
                // Check if objectRef set if not callback will be handled after render.
                // If anything fails callback will will be handled after render.
                if (_objectRef != null)
                {
                    if (_callback != null)
                    {
                        await _callback();
                        _callback = null;
                    }
                }
                else
                {
                    throw new InvalidOperationException("No object ref");
                }
            }
            catch
            {
                if (renderOnFail)
                    await InvokeAsync(StateHasChanged);
            }
        }

        /// <inheritdoc/>
        public override Task HideAsync()
        {
            if (!Shown) return Task.CompletedTask;
            _callback = async () =>
            {
                await HideActionsAsync();
            };
            return TryCallback();
        }

        private async Task HideActionsAsync()
        {
            CanRefresh = false;
            _lock = true;
            Shown = false;
            if (OnHide.HasDelegate)
                await OnHide.InvokeAsync(this);
            await BlazorStrap.Interop.RemoveDocumentEventAsync(this, DataId, EventType.Keyup);
            await BlazorStrap.Interop.RemoveDocumentEventAsync(this, DataId, EventType.Click);

            if (!EventsSet)
            {
                await BlazorStrap.Interop.AddEventAsync(_objectRef, DataId, EventType.TransitionEnd);
                EventsSet = true;
            }

            // Used to hide popovers
            BlazorStrap.ForwardToggle("", this);

            if (!_leaveBodyAlone)
            {
                await BlazorStrap.Interop.RemoveBodyClassAsync("modal-open");
                await BlazorStrap.Interop.SetBodyStyleAsync("overflow", "");
                await BlazorStrap.Interop.SetBodyStyleAsync("paddingRight", "");
            }

            await BlazorStrap.Interop.RemoveClassAsync(MyRef, "show", 50);
            if (BackdropRef != null)
                await BackdropRef.ToggleAsync();

            if (await BlazorStrap.Interop.TransitionDidNotStartAsync(MyRef))
            {
                await TransitionEndAsync();
            }

            _leaveBodyAlone = false;
        }

        /// <inheritdoc/>
        public override Task ShowAsync()
        {
            if (Shown) return Task.CompletedTask;
            _callback = async () =>
            {
                await ShowActionsAsync();
            };
            return TryCallback();
        }

        private async Task ShowActionsAsync()
        {
            CanRefresh = false;
            _lock = true;
            Shown = true;
            if (OnShow.HasDelegate)
                await OnShow.InvokeAsync(this);
            await BlazorStrap.Interop.AddDocumentEventAsync(_objectRef, DataId, EventType.Keyup);
            await BlazorStrap.Interop.AddDocumentEventAsync(_objectRef, DataId, EventType.Click);

            if (!EventsSet)
            {
                await BlazorStrap.Interop.AddDocumentEventAsync(_objectRef, DataId, EventType.TransitionEnd);
                EventsSet = true;
            }

            // Used to hide popovers
            BlazorStrap.ForwardToggle("", this);

            await BlazorStrap.Interop.AddBodyClassAsync("modal-open");

            if (!AllowScroll)
            {
                var scrollWidth = await BlazorStrap.Interop.GetScrollBarWidth();
                var viewportHeight = await BlazorStrap.Interop.GetWindowInnerHeightAsync();
                var peakHeight = await BlazorStrap.Interop.PeakHeightAsync(MyRef);

                if (viewportHeight > peakHeight)
                {
                    await BlazorStrap.Interop.SetBodyStyleAsync("overflow", "hidden");
                    if (scrollWidth != 0)
                        await BlazorStrap.Interop.SetBodyStyleAsync("paddingRight", $"{scrollWidth}px");
                }
            }


            BlazorStrapCore.ModalChanged(this);

            if (BackdropRef != null)
                await BackdropRef.ToggleAsync();
            await BlazorStrap.Interop.SetStyleAsync(MyRef, "display", "block", 50);
            await BlazorStrap.Interop.AddClassAsync(MyRef, "show");

            if (await BlazorStrap.Interop.TransitionDidNotStartAsync(MyRef))
            {
                await TransitionEndAsync();
            }
        }

        private void Toggle()
        {
            EventUtil.AsNonRenderingEventHandler(ToggleAsync).Invoke();
        }


        private Task BackdropClicked()
        {
            if (IsStaticBackdrop)
            {
                _callback = async () =>
                {
                    await BlazorStrap.Interop.AddClassAsync(MyRef, "modal-static");
                    await BlazorStrap.Interop.RemoveClassAsync(MyRef, "modal-static", 250);
                };
                return TryCallback();
            }

            return ToggleAsync();
        }

        /// <inheritdoc/>
        public override Task ToggleAsync()
        {
            return Shown ? HideAsync() : ShowAsync();
        }

        private async Task TransitionEndAsync()
        {
            _callback = async () =>
            {
                await BlazorStrap.Interop.RemoveEventAsync(this, DataId, EventType.TransitionEnd);
            };
            await TryCallback(false);

            Style = Shown ? "display: block;" : "display: none;";
            _lock = false;

            await InvokeAsync(StateHasChanged);
            if (Shown)
            {
                if (OnShown.HasDelegate)
                    _ = Task.Run(() => { _ = OnShown.InvokeAsync(this); });
            }
            else
            {
                if (OnHidden.HasDelegate)
                    _ = Task.Run(() => { _ = OnHidden.InvokeAsync(this); });
            }
            CanRefresh = true;
        }

        private void ClickEvent()
        {
            Toggle();
        }

        public override async Task InteropEventCallback(string id, CallerName name, EventType type)
        {
            if (DataId == id && name.Equals(typeof(ClickForward)) && type == EventType.Click)
            {
                await ToggleAsync();
            }
        }

        [JSInvokable]
        public override async Task InteropEventCallback(string id, CallerName name, EventType type,
            Dictionary<string, string>? classList, JavascriptEvent? e)
        {
            if (MyRef == null)
                return;
            if (DataId == id && name.Equals(this) && type == EventType.TransitionEnd)
            {
                await TransitionEndAsync();
            }
            else if (DataId == id && name.Equals(this) && type == EventType.Keyup && e?.Key == "Escape")
            {
                if (IsStaticBackdrop)
                {
                    await BlazorStrap.Interop.AddClassAsync(MyRef, "modal-static", 250);
                    await BlazorStrap.Interop.RemoveClassAsync(MyRef, "modal-static");
                    return;
                }

                await HideAsync();
            }
            else if (DataId == id && name.Equals(this) && type == EventType.Click &&
                     e?.Target.ClassList.Any(q => q.Value == "modal") == true)
            {
                if (IsStaticBackdrop)
                {

                    await BlazorStrap.Interop.AddClassAsync(MyRef, "modal-static", 250);
                    await BlazorStrap.Interop.RemoveClassAsync(MyRef, "modal-static");
                    return;
                }

                await HideAsync();
            }
        }

        private async void OnModalChange(BSModal? model, bool fromJs)
        {
            if (fromJs)
            {
                if (_shown)
                    await HideAsync();
                return;
            }

            if (model == this || !_shown) return;
            _leaveBodyAlone = true;
            if (_shown)
                await HideAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _objectRef = DotNetObjectReference.Create<BSModal>(this);
                BlazorStrap.OnEventForward += InteropEventCallback;
                BlazorStrapCore.ModalChange += OnModalChange;
            }
            if (_callback != null)
            {
                await _callback.Invoke();
                _callback = null;
            }
        }
        public async ValueTask DisposeAsync()
        {
            try
            {
                await BlazorStrap.Interop.RemoveDocumentEventAsync(this, DataId, EventType.Keyup);
                await BlazorStrap.Interop.RemoveDocumentEventAsync(this, DataId, EventType.Click);
                if (EventsSet)
                    await BlazorStrap.Interop.RemoveEventAsync(this, DataId, EventType.TransitionEnd);
            }
            catch { }
            BlazorStrap.OnEventForward -= InteropEventCallback;
            BlazorStrapCore.ModalChange -= OnModalChange;
            _objectRef?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}