using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace GBFDesktopTools.Model
{
    public class DownLoadMessage:INotifyPropertyChanged
    {
        #region Constructor

        public DownLoadMessage(string TempMainMessage,string TempProgressBarMessageOne)
        {
            this.MainMessage = TempMainMessage;
            this.ProgressBarMessageOne = TempProgressBarMessageOne;
            CustomMessage = new ObservableCollection<string>();
            AddCustomMessage(MainMessage);
            AddCustomMessage(ProgressBarMessageOne);
        }

        #endregion

        #region Property

        private string _MainMessage;
        private string _ProgressBarMessageOne;
        private string _ProgressBarMessageTwo;
        private ObservableCollection<string> _CustomMessage;
        private double _ProgressBarValue;

        //主要信息
        public string MainMessage
        {
            get { return _MainMessage; }
            set { _MainMessage = value; OnPropertyChanged("MainMessage"); }
        }
        //进度信息A
        public string ProgressBarMessageOne
        {
            get { return _ProgressBarMessageOne; }
            set { _ProgressBarMessageOne = value; OnPropertyChanged("ProgressBarMessageOne"); }
        }
        //进度信息B
        public string ProgressBarMessageTwo
        {
            get { return _ProgressBarMessageTwo; }
            set { _ProgressBarMessageTwo = value; OnPropertyChanged("ProgressBarMessageTwo"); }
        }
        //进度条
        public double ProgressBarValue
        {
            get { return _ProgressBarValue; }
            set { _ProgressBarValue = value; OnPropertyChanged("ProgressBarValue"); }
        }
        //自定义信息
        public ObservableCollection<string> CustomMessage
        {
            get { return _CustomMessage; }
            set { _CustomMessage = value; OnPropertyChanged("CustomMessage"); }
        }

        #endregion

        #region Method

        public void AddCustomMessage(string TargetMessage)
        {
            if (CustomMessage != null)
            {
                CustomMessage.Add("<"+TargetMessage+">");
            }
        }

        public void ClearAllMessage()
        {
            CustomMessage.Clear();
            MainMessage = null;
            ProgressBarMessageOne = null;
            ProgressBarMessageTwo = null;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
