﻿using Panuon.UI.Silver.Core;
using Panuon.UI.Silver.Internal.Models;
using Panuon.UI.Silver.Internal.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Panuon.UI.Silver.Internal.Controls
{
    class TimeSelectorMinutePresenter : Control
    {
        #region Fields
        private bool _isItemsInitialized;
        #endregion

        #region Ctor
        public TimeSelectorMinutePresenter()
        {
            AddHandler(Roller.SelectionChangingEvent, new SelectionChangingRoutedEventHandler(OnRollerSelectionChanging));
            AddHandler(Roller.SelectionChangedEvent, new SelectionChangedEventHandler(OnRollerSelectionChanged));
        }

        static TimeSelectorMinutePresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSelectorMinutePresenter), new FrameworkPropertyMetadata(typeof(TimeSelectorMinutePresenter)));
        }
        #endregion

       
        #region Events
        public event SelectedDateChangedEventHandler Selected;
        #endregion

        #region Properties

        #region MaxTime
        public DateTime? MaxTime
        {
            get { return (DateTime?)GetValue(MaxTimeProperty); }
            set { SetValue(MaxTimeProperty, value); }
        }

        public static readonly DependencyProperty MaxTimeProperty =
            DependencyProperty.Register("MaxTime", typeof(DateTime?), typeof(TimeSelectorMinutePresenter));

        #endregion

        #region MinTime
        public DateTime? MinTime
        {
            get { return (DateTime?)GetValue(MinTimeProperty); }
            set { SetValue(MinTimeProperty, value); }
        }

        public static readonly DependencyProperty MinTimeProperty =
            DependencyProperty.Register("MinTime", typeof(DateTime?), typeof(TimeSelectorMinutePresenter));
        #endregion

        #region TimeSelectorItemStyle
        public Style TimeSelectorItemStyle
        {
            get { return (Style)GetValue(TimeSelectorItemStyleProperty); }
            set { SetValue(TimeSelectorItemStyleProperty, value); }
        }

        public static readonly DependencyProperty TimeSelectorItemStyleProperty =
            DependencyProperty.Register("TimeSelectorItemStyle", typeof(Style), typeof(TimeSelectorMinutePresenter));
        #endregion

        #endregion

        #region Internal Properties
        internal TimeSelectorItemModel SelectedTimeSelectorItem
        {
            get { return (TimeSelectorItemModel)GetValue(SelectedTimeSelectorItemProperty); }
            set { SetValue(SelectedTimeSelectorItemProperty, value); }
        }

        internal static readonly DependencyProperty SelectedTimeSelectorItemProperty =
            DependencyProperty.Register("SelectedTimeSelectorItem", typeof(TimeSelectorItemModel), typeof(TimeSelectorMinutePresenter));


        internal ObservableCollection<TimeSelectorItemModel> TimeSelectorItemModels
        {
            get { return (ObservableCollection<TimeSelectorItemModel>)GetValue(TimeSelectorItemModelsProperty); }
            set { SetValue(TimeSelectorItemModelsProperty, value); }
        }

        internal static readonly DependencyProperty TimeSelectorItemModelsProperty =
           DependencyProperty.Register("TimeSelectorItemModels", typeof(ObservableCollection<TimeSelectorItemModel>), typeof(TimeSelectorMinutePresenter));
        #endregion


        #region Methods
        internal void Update(int hour,
            int minute,
            int second)
        {
            if (!_isItemsInitialized)
            {
                TimeSelectorItemModels = new ObservableCollection<TimeSelectorItemModel>();
            }

            for (var i = 0; i < 60; i++)
            {
                TimeSelectorItemModel secondItem = null;
                if (!_isItemsInitialized)
                {
                    secondItem = new TimeSelectorItemModel();
                    TimeSelectorItemModels.Add(secondItem);
                }
                else
                {
                    secondItem = TimeSelectorItemModels[i];
                }
                secondItem.Content = i.ToString("00");
                secondItem.Time = new DateTime(1, 1, 1, hour, i, second);
                secondItem.IsEnabled = IsTimeAvailable(hour, i);
            }

            SelectedTimeSelectorItem = TimeSelectorItemModels[minute];

            _isItemsInitialized = true;
        }
        #endregion

        #region Event Handlers
        private void OnRollerSelectionChanging(object sender, SelectionChangingRoutedEventArgs e)
        {
            if (e.SelectedIndex == -1)
            {
                e.Cancel = true;
            }
        }

        private void OnRollerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            var timeSelectorItemModel = e.AddedItems[0] as TimeSelectorItemModel;
            if (timeSelectorItemModel == null)
            {
                return;
            }

            Selected?.Invoke(this, new SelectedDateChangedEventArgs(timeSelectorItemModel.Time));
        }
        #endregion

        #region Functions

        private bool IsTimeAvailable(int hour, int minute)
        {
            if (MinTime != null)
            {
                var minTime = (DateTime)MinTime;
                if (hour < minTime.Hour)
                {
                    return false;
                }
                if(hour == minTime.Hour && minute < minTime.Minute)
                {
                    return false;
                }
            }
            if (MaxTime != null)
            {
                var maxTime = (DateTime)MaxTime;
                if (hour > maxTime.Hour)
                {
                    return false;
                }
                if (hour == maxTime.Hour && minute > maxTime.Minute)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
