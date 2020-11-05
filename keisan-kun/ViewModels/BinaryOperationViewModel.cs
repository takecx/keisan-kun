using keisan_kun.Model;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace keisan_kun.ViewModels
{
    public enum BinaryOperationType
    {
        /// <summary>
        /// +
        /// </summary>
        plus,
        /// <summary>
        /// -
        /// </summary>
        minus,
        /// <summary>
        /// *
        /// </summary>
        multiply,
        /// <summary>
        /// /
        /// </summary>
        division,
    }

    class BinaryOperationViewModel : BindableBase, INavigationAware
    {
        #region 定数
        private readonly int MAIN_INTERVAL_MILLI = 15;
        #endregion

        private RegionManager _RegionManager;
        private DispatcherTimer _MainTimer = new DispatcherTimer();
        private DispatcherTimer _CountDownTimer = new DispatcherTimer();
        private MathQuestionGenerator questionGenerator;

        #region NotifiablePropery

        private int _Point;
        public int m_Point
        {
            get { return _Point; }
            set { this.SetProperty(ref this._Point, value); }
        }

        private TimeSpan _RemainingTime;
        public TimeSpan m_RemainingTime
        {
            get { return _RemainingTime; }
            set { this.SetProperty(ref this._RemainingTime, value); }
        }

        private int _CountDown = 3;
        public int m_CountDown
        {
            get { return _CountDown; }
            set { this.SetProperty(ref this._CountDown, value); }
        }

        private bool _IsCountDowning = true;
        public bool m_IsCountDowning
        {
            get { return _IsCountDowning; }
            set { this.SetProperty(ref this._IsCountDowning, value); }
        }

        private BinaryOperationType _BinaryOperationType;
        public BinaryOperationType m_BinaryOperationType
        {
            get { return _BinaryOperationType; }
            set { this.SetProperty(ref this._BinaryOperationType, value); }
        }
        private int _FirstValue;
        public int m_FirstValue
        {
            get { return _FirstValue; }
            set { this.SetProperty(ref this._FirstValue, value); }
        }

        private int _SecondValue;
        public int m_SecondValue
        {
            get { return _SecondValue; }
            set { this.SetProperty(ref this._SecondValue, value); }
        }

        private int? _Answer;
        public int? m_Answer
        {
            get { return _Answer; }
            set
            {
                this.SetProperty(ref this._Answer, value);
            }
        }

        #endregion

        public BinaryOperationViewModel(RegionManager regionManager)
        {
            _RegionManager = regionManager;
            StartCountDown();
        }

        private void StartCountDown()
        {
            _CountDownTimer.Interval = new TimeSpan(0, 0, 1);
            _CountDownTimer.Tick += new EventHandler(CountDown);
            _CountDownTimer.Start();
        }

        public void CountDown(object sender, EventArgs e)
        {
            if(m_CountDown > 1)
            {
                m_CountDown--;
            }
            else
            {
                _CountDownTimer.Stop();
                m_IsCountDowning = false;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var opeType = navigationContext.Parameters["OperatorType"];
            if(opeType != null) 
            { 
                m_BinaryOperationType =  (BinaryOperationType)opeType;
            }

            var limitTime = navigationContext.Parameters["LimitationTime"];
            if(limitTime != null)
            {
                if((int)limitTime == -1)
                {
                    // 無制限モード
                }
                else
                {
                    // 制限時間付きモード
                    m_RemainingTime = new TimeSpan(0, 0, 0, (int)limitTime);
                    StartMainTimer();
                }
            }

            var questionType = navigationContext.Parameters["QuestionType"];
            if(questionType != null)
            {
                var questionTypeVal = (QuestionType)questionType;
                questionGenerator = new MathQuestionGenerator(questionTypeVal);
                (m_FirstValue, m_SecondValue) = questionGenerator.UpdateQuestion();
            }
        }

        private void StartMainTimer()
        {
            _MainTimer.Interval = new TimeSpan(0, 0, 0, 0, MAIN_INTERVAL_MILLI);
            _MainTimer.Tick += new EventHandler(UpdateRemainigTime);
            _MainTimer.Start();
        }

        private void UpdateRemainigTime(object sender, EventArgs e)
        {
            if(m_IsCountDowning == false)
            {
                if(m_RemainingTime > new TimeSpan(0))
                {
                    m_RemainingTime -= new TimeSpan(0, 0, 0, 0, MAIN_INTERVAL_MILLI);
                }
                else
                {
                    // 終了
                    _MainTimer.Stop();
                }
            }
        }
    }
}
