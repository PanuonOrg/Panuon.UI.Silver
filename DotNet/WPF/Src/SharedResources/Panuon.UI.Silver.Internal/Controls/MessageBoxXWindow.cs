﻿using Panuon.UI.Silver.Configurations;
using Panuon.UI.Silver.Internal.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Panuon.UI.Silver.Internal.Controls
{
    [TemplatePart()]
    class MessageBoxXWindow : WindowX
    {
        #region Fields
        private const string OKButtonTemplateName = "PART_OKButton";

        private const string CancelButtonTemplateName = "PART_CancelButton";

        private const string YesButtonTemplateName = "PART_YesButton";

        private const string NoButtonTemplateName = "PART_NoButton";

        private Button _okButton;

        private Button _cancelButton;

        private Button _yesButton;

        private Button _noButton;

        private MessageBoxButton _messageBoxButton;

        private DefaultButton _defaultButton;

        private MessageBoxXSetting _setting;

        #endregion

        #region Ctor
        public MessageBoxXWindow(string message, string caption, MessageBoxButton button, MessageBoxIcon icon, DefaultButton defaultButton, Window owner, MessageBoxXSetting setting)
        {
            _setting = setting;
            _messageBoxButton = button;
            _defaultButton = defaultButton;

            Owner = owner;
            Title = caption ?? "";
            WindowStartupLocation = owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

            MessageBoxContent = new MessageBoxContent()
            {
                Message = message,
                Caption = caption,
                Icon = icon,
            };

        Style = setting.WindowXStyle;
            Content = MessageBoxContent;
            ContentTemplate = setting.ContentTemplate;
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                _okButton = FrameworkElementUtil.FindVisualChild<Button>(this, OKButtonTemplateName);
                if (_okButton == null)
                {
                    throw new Exception($"MessageBoxXSetting : Can not find Button named {OKButtonTemplateName} in ContentTemplate property.");
                }
                _cancelButton = FrameworkElementUtil.FindVisualChild<Button>(this, CancelButtonTemplateName);
                if (_cancelButton == null)
                {
                    throw new Exception($"MessageBoxXSetting : Can not find Button named {CancelButtonTemplateName} in ContentTemplate property.");
                }
                _yesButton = FrameworkElementUtil.FindVisualChild<Button>(this, YesButtonTemplateName);
                if (_yesButton == null)
                {
                    throw new Exception($"MessageBoxXSetting : Can not find Button named {YesButtonTemplateName} in ContentTemplate property.");
                }
                _noButton = FrameworkElementUtil.FindVisualChild<Button>(this, NoButtonTemplateName);
                if (_noButton == null)
                {
                    throw new Exception($"MessageBoxXSetting : Can not find Button named {NoButtonTemplateName} in ContentTemplate property.");
                }

                if (_setting.ButtonStyle != null)
                {
                    _okButton.Style = _setting.ButtonStyle;
                    _cancelButton.Style = _setting.ButtonStyle;
                    _yesButton.Style = _setting.ButtonStyle;
                    _noButton.Style = _setting.ButtonStyle;
                }

                _okButton.Content = _setting.OKButtonContent;
                _cancelButton.Content = _setting.CancelButtonContent;
                _yesButton.Content = _setting.YesButtonContent;
                _noButton.Content = _setting.NoButtonContent;

                _okButton.Visibility = (_messageBoxButton == MessageBoxButton.OK || _messageBoxButton == MessageBoxButton.OKCancel) ? Visibility.Visible : Visibility.Collapsed;
                _cancelButton.Visibility = (_messageBoxButton == MessageBoxButton.OKCancel || _messageBoxButton == MessageBoxButton.YesNoCancel) ? Visibility.Visible : Visibility.Collapsed;
                _yesButton.Visibility = (_messageBoxButton == MessageBoxButton.YesNo || _messageBoxButton == MessageBoxButton.YesNoCancel) ? Visibility.Visible : Visibility.Collapsed;
                _noButton.Visibility = (_messageBoxButton == MessageBoxButton.YesNo || _messageBoxButton == MessageBoxButton.YesNoCancel) ? Visibility.Visible : Visibility.Collapsed;

                _okButton.IsDefault = (_defaultButton == DefaultButton.YesOK && _messageBoxButton == MessageBoxButton.OK);
                _cancelButton.IsDefault = (_defaultButton == DefaultButton.CancelNo) || (_defaultButton == DefaultButton.NoCancel && _messageBoxButton == MessageBoxButton.OKCancel);
                _yesButton.IsDefault = (_defaultButton == DefaultButton.YesOK);
                _noButton.IsDefault = (_defaultButton == DefaultButton.NoCancel) || (_defaultButton == DefaultButton.CancelNo && (_messageBoxButton == MessageBoxButton.YesNoCancel || _messageBoxButton == MessageBoxButton.OKCancel));

                _okButton.DataContext = "OK";
                _cancelButton.DataContext = "Cancel";
                _yesButton.DataContext = "Yes";
                _noButton.DataContext = "No";

            }), DispatcherPriority.DataBind);

        }

        #endregion

        #region Properties

        #region MessageBoxContent
        public MessageBoxContent MessageBoxContent
        {
            get { return (MessageBoxContent)GetValue(MessageBoxContentProperty); }
            set { SetValue(MessageBoxContentProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxContentProperty =
            DependencyProperty.Register("MessageBoxContent", typeof(MessageBoxContent), typeof(MessageBoxXWindow));
        #endregion

        #region Result
        public MessageBoxResult Result
        {
            get { return (MessageBoxResult)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(MessageBoxResult), typeof(MessageBoxXWindow));
        #endregion

        #endregion
    }

    class MessageBoxContent : DependencyObject
    {
        #region Message
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxContent));
        #endregion

        #region Caption
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(MessageBoxContent));
        #endregion

        #region Icon
        public MessageBoxIcon Icon
        {
            get { return (MessageBoxIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(MessageBoxIcon), typeof(MessageBoxXWindow));
        #endregion

    }
}