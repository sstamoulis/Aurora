﻿using Aurora.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aurora.Settings.Layers
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Control_RazerLayer : UserControl
    {
        private bool settingsset = false;
        protected RazerLayerHandler Context => DataContext as RazerLayerHandler;

        public Control_RazerLayer()
        {
            InitializeComponent();
        }

        public Control_RazerLayer(RazerLayerHandler datacontext)
        {
            InitializeComponent();

            DataContext = datacontext;
        }
        public void SetSettings()
        {
            if (Context != null && !settingsset)
            {
                ColorPostProcessCheckBox.IsChecked = Context.Properties.ColorPostProcessEnabled;
                BrightnessBoostSlider.Value = Context.Properties.BrightnessBoost;
                CollectionViewSource.GetDefaultView(KeyCloneListBox.ItemsSource).Refresh();
                settingsset = true;
            }
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            SetSettings();
            this.Loaded -= OnUserControlLoaded;
        }

        private void OnAddKeyCloneButtonClick(object sender, RoutedEventArgs e)
        {
            if (KeyCloneSourceButtonComboBox.SelectedItem == null || KeyCloneDestinationButtonComboBox.SelectedItem == null)
                return;

            var sourceKey = (DeviceKeys)KeyCloneSourceButtonComboBox.SelectedItem;
            var destKey = (DeviceKeys)KeyCloneDestinationButtonComboBox.SelectedItem;

            var cloneMap = Context.Properties.KeyCloneMap;
            if (cloneMap.ContainsKey(destKey) && cloneMap[destKey] == sourceKey)
                return;

            cloneMap.Add(destKey, sourceKey);
            CollectionViewSource.GetDefaultView(KeyCloneListBox.ItemsSource).Refresh();
        }

        private void OnDeleteKeyCloneButtonClick(object sender, RoutedEventArgs e)
        {
            if (KeyCloneListBox.SelectedItem == null)
                return;

            var cloneMap = Context.Properties.KeyCloneMap;
            var item = (KeyValuePair<DeviceKeys, DeviceKeys>)KeyCloneListBox.SelectedItem;
            if (!cloneMap.ContainsKey(item.Key) || cloneMap[item.Key] != item.Value)
                return;

            cloneMap.Remove(item.Key);
            CollectionViewSource.GetDefaultView(KeyCloneListBox.ItemsSource).Refresh();
        }
    }
}
