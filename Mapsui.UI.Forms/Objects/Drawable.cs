﻿using Mapsui.Providers;
using Mapsui.Styles;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
#if __MAUI__
using Mapsui.UI.Maui;
using Mapsui.UI.Maui.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
#else
using Mapsui.UI.Forms;
using Mapsui.UI.Forms.Extensions;
using Xamarin.Forms;
#endif

namespace Mapsui.UI.Objects
{
    /// <summary>
    /// Base class for all drawables like polyline, polygon and circle
    /// </summary>
    public class Drawable : BindableObject, IClickable, IFeatureProvider
    {
        public static readonly BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(Pin), default(string));
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(Circle), 1f);
        public static readonly BindableProperty MinVisibleProperty = BindableProperty.Create(nameof(MinVisible), typeof(double), typeof(Circle), 0.0);
        public static readonly BindableProperty MaxVisibleProperty = BindableProperty.Create(nameof(MaxVisible), typeof(double), typeof(Circle), double.MaxValue);
        public static readonly BindableProperty ZIndexProperty = BindableProperty.Create(nameof(ZIndex), typeof(int), typeof(Circle), 0);
        public static readonly BindableProperty IsClickableProperty = BindableProperty.Create(nameof(IsClickable), typeof(bool), typeof(Drawable), false);
#if __MAUI__
        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(nameof(StrokeColor), typeof(Microsoft.Maui.Graphics.Color), typeof(Circle), Mapsui.Styles.Color.Black.ToMaui());
#else
        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(nameof(StrokeColor), typeof(Xamarin.Forms.Color), typeof(Circle), Xamarin.Forms.Color.Black);
#endif

        /// <summary>
        /// Label of drawable
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// StrokeWidth of drawable in pixel
        /// </summary>
        public float StrokeWidth
        {
            get { return (float)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// StrokeColor for drawable
        /// </summary>
#if __MAUI__
        public Microsoft.Maui.Graphics.Color StrokeColor
        {
            get { return (Microsoft.Maui.Graphics.Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }
#else
        public Xamarin.Forms.Color StrokeColor
        {
            get { return (Xamarin.Forms.Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }
#endif

        /// <summary>
        /// MinVisible for drawable in resolution of Mapsui (smaller values are higher zoom levels)
        /// </summary>
        public double MinVisible
        {
            get { return (double)GetValue(MinVisibleProperty); }
            set { SetValue(MinVisibleProperty, value); }
        }

        /// <summary>
        /// MaxVisible for drawable in resolution of Mapsui (smaller values are higher zoom levels)
        /// </summary>
        public double MaxVisible
        {
            get { return (double)GetValue(MaxVisibleProperty); }
            set { SetValue(MaxVisibleProperty, value); }
        }

        /// <summary>
        /// ZIndex of this drawable
        /// </summary>
        public int ZIndex
        {
            get { return (int)GetValue(ZIndexProperty); }
            set { SetValue(ZIndexProperty, value); }
        }

        /// <summary>
        /// Is this drawable clickable
        /// </summary>
        public bool IsClickable
        {
            get { return (bool)GetValue(IsClickableProperty); }
            set { SetValue(IsClickableProperty, value); }
        }

        /// <summary>
        /// Object for free use
        /// </summary>
        public object Tag { get; set; }

        private Feature feature;

        /// <summary>
        /// Mapsui Feature belonging to this drawable
        /// </summary>
        public Feature Feature
        {
            get
            {
                return feature;
            }
            set
            {
                if (feature == null || !feature.Equals(value))
                    feature = value;
            }
        }

        /// <summary>
        /// Event called, if this drawable is clicked an IsClickable is true
        /// </summary>
        public event EventHandler<DrawableClickedEventArgs> Clicked;

        void IClickable.HandleClicked(DrawableClickedEventArgs e)
        {
        }

        internal void HandleClicked(DrawableClickedEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(StrokeWidth):
                    ((VectorStyle)Feature.Styles.First()).Line.Width = StrokeWidth;
                    break;
                case nameof(StrokeColor):
                    ((VectorStyle)Feature.Styles.First()).Line.Color = StrokeColor.ToMapsui();
                    break;
                case nameof(MinVisible):
                    ((VectorStyle)Feature.Styles.First()).MinVisible = MinVisible;
                    break;
                case nameof(MaxVisible):
                    ((VectorStyle)Feature.Styles.First()).MaxVisible = MaxVisible;
                    break;
            }
        }
    }
}