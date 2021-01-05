﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI.Silver
{
    public static class RadioButtonHelper
    {
        #region Properties

        #region GlyphBrush
        public static Brush GetGlyphBrush(RadioButton radioButton)
        {
            return (Brush)radioButton.GetValue(GlyphBrushProperty);
        }

        public static void SetGlyphBrush(RadioButton radioButton, Brush value)
        {
            radioButton.SetValue(GlyphBrushProperty, value);
        }

        public static readonly DependencyProperty GlyphBrushProperty =
            DependencyProperty.RegisterAttached("GlyphBrush", typeof(Brush), typeof(RadioButtonHelper));
        #endregion

        #region NullableGlyphBrush
        public static Brush GetNullableGlyphBrush(RadioButton radioButton)
        {
            return (Brush)radioButton.GetValue(NullableGlyphBrushProperty);
        }

        public static void SetNullableGlyphBrush(RadioButton radioButton, Brush value)
        {
            radioButton.SetValue(NullableGlyphBrushProperty, value);
        }

        public static readonly DependencyProperty NullableGlyphBrushProperty =
            DependencyProperty.RegisterAttached("NullableGlyphBrush", typeof(Brush), typeof(RadioButtonHelper));
        #endregion

        #region CheckedGlyphBrush
        public static Brush GetCheckedGlyphBrush(RadioButton radioButton)
        {
            return (Brush)radioButton.GetValue(CheckedGlyphBrushProperty);
        }

        public static void SetCheckedGlyphBrush(RadioButton radioButton, Brush value)
        {
            radioButton.SetValue(CheckedGlyphBrushProperty, value);
        }

        public static readonly DependencyProperty CheckedGlyphBrushProperty =
            DependencyProperty.RegisterAttached("CheckedGlyphBrush", typeof(Brush), typeof(RadioButtonHelper));
        #endregion

        #region CheckedForeground
        public static Brush GetCheckedForeground(RadioButton radioButton)
        {
            return (Brush)radioButton.GetValue(CheckedForegroundProperty);
        }

        public static void SetCheckedForeground(RadioButton radioButton, Brush value)
        {
            radioButton.SetValue(CheckedForegroundProperty, value);
        }

        public static readonly DependencyProperty CheckedForegroundProperty =
            DependencyProperty.RegisterAttached("CheckedForeground", typeof(Brush), typeof(RadioButtonHelper));
        #endregion

        #region CheckedBackground
        public static Brush GetCheckedBackground(RadioButton radioButton)
        {
            return (Brush)radioButton.GetValue(CheckedBackgroundProperty);
        }

        public static void SetCheckedBackground(RadioButton radioButton, Brush value)
        {
            radioButton.SetValue(CheckedBackgroundProperty, value);
        }

        public static readonly DependencyProperty CheckedBackgroundProperty =
            DependencyProperty.RegisterAttached("CheckedBackground", typeof(Brush), typeof(RadioButtonHelper));
        #endregion

        #region CheckedBorderBrush
        public static Brush GetCheckedBorderBrush(RadioButton radioButton)
        {
            return (Brush)radioButton.GetValue(CheckedBorderBrushProperty);
        }

        public static void SetCheckedBorderBrush(RadioButton radioButton, Brush value)
        {
            radioButton.SetValue(CheckedBorderBrushProperty, value);
        }

        public static readonly DependencyProperty CheckedBorderBrushProperty =
            DependencyProperty.RegisterAttached("CheckedBorderBrush", typeof(Brush), typeof(RadioButtonHelper));
        #endregion

        #region BoxHeight
        public static double GetBoxHeight(RadioButton radioButton)
        {
            return (double)radioButton.GetValue(BoxHeightProperty);
        }

        public static void SetBoxHeight(RadioButton radioButton, double value)
        {
            radioButton.SetValue(BoxHeightProperty, value);
        }

        public static readonly DependencyProperty BoxHeightProperty =
            DependencyProperty.RegisterAttached("BoxHeight", typeof(double), typeof(RadioButtonHelper));
        #endregion

        #region BoxWidth
        public static double GetBoxWidth(RadioButton radioButton)
        {
            return (double)radioButton.GetValue(BoxWidthProperty);
        }

        public static void SetBoxWidth(RadioButton radioButton, double value)
        {
            radioButton.SetValue(BoxWidthProperty, value);
        }

        public static readonly DependencyProperty BoxWidthProperty =
            DependencyProperty.RegisterAttached("BoxWidth", typeof(double), typeof(RadioButtonHelper));
        #endregion

        #region CornerRadius
        public static CornerRadius GetCornerRadius(RadioButton radioButton)
        {
            return (CornerRadius)radioButton.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(RadioButton radioButton, CornerRadius value)
        {
            radioButton.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(RadioButtonHelper));
        #endregion

        #region CheckedContent
        public static object GetCheckedContent(RadioButton radioButton)
        {
            return (object)radioButton.GetValue(CheckedContentProperty);
        }

        public static void SetCheckedContent(RadioButton radioButton, object value)
        {
            radioButton.SetValue(CheckedContentProperty, value);
        }

        public static readonly DependencyProperty CheckedContentProperty =
            DependencyProperty.RegisterAttached("CheckedContent", typeof(object), typeof(RadioButtonHelper));
        #endregion

        #endregion
    }
}
