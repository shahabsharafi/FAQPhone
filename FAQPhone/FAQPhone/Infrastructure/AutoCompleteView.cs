// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="AutoCompleteView.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Define the AutoCompleteView control.
    /// </summary>
    public class AutoCompleteView : ContentView
    {
        /// <summary>
        /// The execute on suggestion click property.
        /// </summary>
        public static readonly BindableProperty ExecuteOnSuggestionClickProperty = BindableProperty.Create<AutoCompleteView, bool>(p => p.ExecuteOnSuggestionClick, false);

        public static readonly BindableProperty DefaultTextProperty = BindableProperty.Create<AutoCompleteView, string>(p => p.DefaultText, string.Empty, BindingMode.TwoWay, null, DefaultTextChanged);
        /// <summary>
        /// The placeholder property.
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create<AutoCompleteView, string>(p => p.Placeholder, string.Empty, BindingMode.TwoWay, null, PlaceHolderChanged);

        /// <summary>
        /// The selected command property.
        /// </summary>
        public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create<AutoCompleteView, ICommand>(p => p.SelectedCommand, null);

        /// <summary>
        /// The selected item property.
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<AutoCompleteView, object>(p => p.SelectedItem, null, BindingMode.TwoWay);

        /// <summary>
        /// The suggestion background color property.
        /// </summary>
        public static readonly BindableProperty SuggestionBackgroundColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.SuggestionBackgroundColor, Color.Red, BindingMode.TwoWay, null, SuggestionBackgroundColorChanged);

        /// <summary>
        /// The suggestion item data template property.
        /// </summary>
        public static readonly BindableProperty SuggestionItemDataTemplateProperty = BindableProperty.Create<AutoCompleteView, DataTemplate>(p => p.SuggestionItemDataTemplate, null, BindingMode.TwoWay, null, SuggestionItemDataTemplateChanged);

        /// <summary>
        /// The suggestion height request property.
        /// </summary>
        public static readonly BindableProperty SuggestionsHeightRequestProperty = BindableProperty.Create<AutoCompleteView, double>(p => p.SuggestionsHeightRequest, 250, BindingMode.TwoWay, null, SuggestionHeightRequestChanged);

        /// <summary>
        /// The suggestions property.
        /// </summary>
        public static readonly BindableProperty SuggestionsProperty = BindableProperty.Create<AutoCompleteView, IEnumerable>(p => p.Suggestions, null);

        /// <summary>
        /// The text background color property.
        /// </summary>
        public static readonly BindableProperty TextBackgroundColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.TextBackgroundColor, Color.Transparent, BindingMode.TwoWay, null, TextBackgroundColorChanged);

        /// <summary>
        /// The text color property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create<AutoCompleteView, Color>(p => p.TextBackgroundColor, Color.Black, BindingMode.TwoWay, null, TextColorChanged);

        /// <summary>
        /// The text horizontal options property
        /// </summary>
        public static readonly BindableProperty TextHorizontalOptionsProperty = BindableProperty.Create<AutoCompleteView, LayoutOptions>(p => p.TextHorizontalOptions, LayoutOptions.FillAndExpand, BindingMode.TwoWay, null, TextHorizontalOptionsChanged);

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create<AutoCompleteView, string>(p => p.Text, string.Empty, BindingMode.TwoWay, null, TextValueChanged);

        /// <summary>
        /// The text vertical options property.
        /// </summary>
        public static readonly BindableProperty TextVerticalOptionsProperty = BindableProperty.Create<AutoCompleteView, LayoutOptions>(p => p.TextVerticalOptions, LayoutOptions.Start, BindingMode.TwoWay, null, TestVerticalOptionsChanged);
        private readonly ObservableCollection<object> _availableSuggestions;
        private readonly Entry _entText;
        private readonly ListView _lstSuggestions;
        private readonly StackLayout _stkBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteView"/> class.
        /// </summary>
        public AutoCompleteView()
        {
            _availableSuggestions = new ObservableCollection<object>();
            _stkBase = new StackLayout();
            var innerLayout = new StackLayout();
            _entText = new Entry
            {
                HorizontalOptions = TextHorizontalOptions,
                VerticalOptions = TextVerticalOptions,
                TextColor = TextColor,
                BackgroundColor = TextBackgroundColor
            };
            _lstSuggestions = new ListView
            {
                HeightRequest = SuggestionsHeightRequest,
                HasUnevenRows = true
            };

            innerLayout.Children.Add(_entText);
            _stkBase.Children.Add(innerLayout);
            _stkBase.Children.Add(_lstSuggestions);

            Content = _stkBase;


            _entText.TextChanged += (s, e) =>
            {
                Text = e.NewTextValue;
                OnTextChanged(e);
            };
            _lstSuggestions.ItemSelected += (s, e) =>
            {
                _entText.Text = e.SelectedItem.ToString();

                _availableSuggestions.Clear();
                ShowHideListbox(false);
                OnSelectedItemChanged(e.SelectedItem);                
            };
            ShowHideListbox(false);
            _lstSuggestions.ItemsSource = _availableSuggestions;
        }

        /// <summary>
        /// Occurs when [selected item changed].
        /// </summary>
        public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

        /// <summary>
        /// Occurs when [text changed].
        /// </summary>
        public event EventHandler<TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Gets the available Suggestions.
        /// </summary>
        /// <value>The available Suggestions.</value>
        public IEnumerable AvailableSuggestions
        {
            get { return _availableSuggestions; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [execute on sugestion click].
        /// </summary>
        /// <value><c>true</c> if [execute on sugestion click]; otherwise, <c>false</c>.</value>
        public bool ExecuteOnSuggestionClick
        {
            get { return (bool)GetValue(ExecuteOnSuggestionClickProperty); }
            set { SetValue(ExecuteOnSuggestionClickProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        /// <value>The placeholder.</value>
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected command.
        /// </summary>
        /// <value>The selected command.</value>
        public ICommand SelectedCommand
        {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the sugestion background.
        /// </summary>
        /// <value>The color of the sugestion background.</value>
        public Color SuggestionBackgroundColor
        {
            get { return (Color)GetValue(SuggestionBackgroundColorProperty); }
            set { SetValue(SuggestionBackgroundColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the suggestion item data template.
        /// </summary>
        /// <value>The sugestion item data template.</value>
        public DataTemplate SuggestionItemDataTemplate
        {
            get { return (DataTemplate)GetValue(SuggestionItemDataTemplateProperty); }
            set { SetValue(SuggestionItemDataTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Suggestions.
        /// </summary>
        /// <value>The Suggestions.</value>
        public IEnumerable Suggestions
        {
            get { return (IEnumerable)GetValue(SuggestionsProperty); }
            set { SetValue(SuggestionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of the suggestion.
        /// </summary>
        /// <value>The height of the suggestion.</value>
        public double SuggestionsHeightRequest
        {
            get { return (double)GetValue(SuggestionsHeightRequestProperty); }
            set { SetValue(SuggestionsHeightRequestProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text background.
        /// </summary>
        /// <value>The color of the text background.</value>
        public Color TextBackgroundColor
        {
            get { return (Color)GetValue(TextBackgroundColorProperty); }
            set { SetValue(TextBackgroundColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text horizontal options.
        /// </summary>
        /// <value>The text horizontal options.</value>
        public LayoutOptions TextHorizontalOptions
        {
            get { return (LayoutOptions)GetValue(TextHorizontalOptionsProperty); }
            set { SetValue(TextHorizontalOptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text vertical options.
        /// </summary>
        /// <value>The text vertical options.</value>
        public LayoutOptions TextVerticalOptions
        {
            get { return (LayoutOptions)GetValue(TextVerticalOptionsProperty); }
            set { SetValue(TextVerticalOptionsProperty, value); }
        }

        private static void DefaultTextChanged(BindableObject obj, string oldDefaultTextValue, string newDefaultTextValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.Text = newDefaultTextValue;
            }
        }
        /// <summary>
        /// Places the holder changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldPlaceHolderValue">The old place holder value.</param>
        /// <param name="newPlaceHolderValue">The new place holder value.</param>
        private static void PlaceHolderChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.Placeholder = newPlaceHolderValue;
            }
        }

        /// <summary>
        /// Suggestions the background color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SuggestionBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._lstSuggestions.BackgroundColor = newValue;
            }
        }

        /// <summary>
        /// Suggestions the item data template changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldShowSearchValue">The old show search value.</param>
        /// <param name="newShowSearchValue">The new show search value.</param>
        private static void SuggestionItemDataTemplateChanged(BindableObject obj, DataTemplate oldShowSearchValue, DataTemplate newShowSearchValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._lstSuggestions.ItemTemplate = newShowSearchValue;
            }
        }

        /// <summary>
        /// Suggestions the height changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SuggestionHeightRequestChanged(BindableObject obj, double oldValue, double newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._lstSuggestions.HeightRequest = newValue;
            }
        }
        

        /// <summary>
        /// Tests the vertical options changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TestVerticalOptionsChanged(BindableObject obj, LayoutOptions oldValue, LayoutOptions newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.VerticalOptions = newValue;
            }
        }

        /// <summary>
        /// Texts the background color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TextBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.BackgroundColor = newValue;
            }
        }

        /// <summary>
        /// Texts the color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TextColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.TextColor = newValue;
            }
        }

        /// <summary>
        /// Texts the horizontal options changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TextHorizontalOptionsChanged(BindableObject obj, LayoutOptions oldValue, LayoutOptions newValue)
        {
            var autoCompleteView = obj as AutoCompleteView;
            if (autoCompleteView != null)
            {
                autoCompleteView._entText.VerticalOptions = newValue;
            }
        }
        /// <summary>
        /// Texts the changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldPlaceHolderValue">The old place holder value.</param>
        /// <param name="newPlaceHolderValue">The new place holder value.</param>
        private static void TextValueChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
        {
            var control = obj as AutoCompleteView;

            if (control != null)
            {
                var cleanedNewPlaceHolderValue = Regex.Replace((newPlaceHolderValue ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);

                if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && control.Suggestions != null)
                {
                    var filteredSuggestions = control.Suggestions.Cast<object>()
                        .Where(x => Regex.Replace(x.ToString().ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue))
                        .OrderByDescending(x => Regex.Replace(x.ToString()
                        .ToLowerInvariant(), @"\s+", string.Empty)
                        .StartsWith(cleanedNewPlaceHolderValue)).ToList();

                    control._availableSuggestions.Clear();
                    if (filteredSuggestions.Count > 0)
                    {
                        foreach (var suggestion in filteredSuggestions)
                        {
                            control._availableSuggestions.Add(suggestion);
                        }
                        control.ShowHideListbox(true);
                    }
                    else
                    {
                        control.ShowHideListbox(false);
                    }
                }
                else
                {
                    if (control._availableSuggestions.Count > 0)
                    {
                        control._availableSuggestions.Clear();
                        control.ShowHideListbox(false);
                    }
                }
            }
        }

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        private void OnSelectedItemChanged(object selectedItem)
        {
            SelectedItem = selectedItem;

            if (SelectedCommand != null)
                SelectedCommand.Execute(selectedItem);

            var handler = SelectedItemChanged;
            if (handler != null)
            {
                handler(this, new SelectedItemChangedEventArgs(selectedItem));
            }
        }

        /// <summary>
        /// Handles the <see cref="E:TextChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void OnTextChanged(TextChangedEventArgs e)
        {
            var handler = TextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Shows the hide listbox.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        private void ShowHideListbox(bool show)
        {
            _lstSuggestions.IsVisible = show;
        }
    }
}