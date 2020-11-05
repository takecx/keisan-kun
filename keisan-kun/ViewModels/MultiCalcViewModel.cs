using keisan_kun.Model;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace keisan_kun.ViewModels
{
    class MultiCalcViewModel : BindableBase, INavigationAware
    {
        #region ていすう
        private readonly string K_ADDSTR = "たす";
        private readonly string K_SUBSTR = "ひく";
        private readonly Dictionary<string, string> K_OPERATORS;
        private readonly string K_BOO = @"./sounds/boo.mp3";
        private readonly string K_PIPO = @"./sounds/pipo.mp3";
        private readonly string K_PIPO2 = @"./sounds/pipopipo.mp3";
        private readonly string K_SCORES_CONF = @"./data/scores.json";
        #endregion

        private readonly IRegionManager _regionManager;

        private WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();
        private WindowsMediaPlayer _BGMPlayer = new WindowsMediaPlayer();
        private int max = 10;
        private int min = 0;
        private Random random1 = new Random(DateTime.Now.GetHashCode());
        private Random random2 = new Random(DateTime.Now.GetHashCode() + 1);
        private UserScores _UserScores;

        #region Notifiable Property
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
                //if()
                this.SetProperty(ref this._Answer, value);
            }
        }

        private int _Points;
        public int m_Points
        {
            get { return _Points; }
            set { this.SetProperty(ref this._Points, value); }
        }

        private ObservableCollection<string> _CalcTypes;
        public ObservableCollection<string> m_CalcTypes
        {
            get { return _CalcTypes; }
            set { this.SetProperty(ref this._CalcTypes, value); }
        }

        private string _CurrentCalcType;
        public string m_CurrentCalcType
        {
            get { return _CurrentCalcType; }
            set
            {
                this.SetProperty(ref this._CurrentCalcType, value);
                UpdateUntilPositive();
                if (m_CurrentSelectedUserName != null) UpdateScoresForDisplay();
            }
        }

        private string _LatestStatus;
        public string m_LatestStatus
        {
            get { return _LatestStatus; }
            set { this.SetProperty(ref this._LatestStatus, value); }
        }

        private ObservableCollection<string> _UserNames = new ObservableCollection<string>();
        public ObservableCollection<string> m_UserNames
        {
            get { return _UserNames; }
            set { this.SetProperty(ref this._UserNames, value); }
        }

        private string _CurrentSelectedUserName;
        public string m_CurrentSelectedUserName
        {
            get { return _CurrentSelectedUserName; }
            set
            {
                this.SetProperty(ref this._CurrentSelectedUserName, value);
                UpdateScoresForDisplay();
                m_Points = 0;
            }
        }

        private int _CurOperatorTotalChallenge;
        public int m_CurOperatorTotalChallenge
        {
            get { return _CurOperatorTotalChallenge; }
            set { this.SetProperty(ref this._CurOperatorTotalChallenge, value); }
        }

        private int _CurOperatorCorrect;
        public int m_CurOperatorCorrect
        {
            get { return _CurOperatorCorrect; }
            set { this.SetProperty(ref this._CurOperatorCorrect, value); }
        }

        private int _CurOperatorIncorrect;
        public int m_CurOperatorIncorrect
        {
            get { return _CurOperatorIncorrect; }
            set { this.SetProperty(ref this._CurOperatorIncorrect, value); }
        }

        private bool _IsTargetedCarryUp;
        public bool m_IsTargetedCarryUp
        {
            get { return _IsTargetedCarryUp; }
            set { this.SetProperty(ref this._IsTargetedCarryUp, value); }
        }

        #endregion

        #region Commands
        public DelegateCommand ExcecuteCalcCommand { get; private set; }
        private bool CanExcecuteCalc()
        {
            return true;
        }
        public DelegateCommand<string> SetAnswerCommand { get; private set; }
        private bool CanSetAnswerCommand(string val)
        {
            return true;
        }
        #endregion

        public MultiCalcViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            CreateCommands();
            LoadSetting();

            m_Points = 0;
            K_OPERATORS = new Dictionary<string, string>() { { K_ADDSTR, "+" }, { K_SUBSTR, "-" } };
            m_CalcTypes = new ObservableCollection<string>(K_OPERATORS.Values);
            m_CurrentCalcType = K_OPERATORS[K_ADDSTR];
            m_UserNames = new ObservableCollection<string>(_UserScores.m_Users.Select(x => x.m_Username));

            //PlayBGM();
            UpdateProblem();
            SpeechProblem();
        }

        ~MultiCalcViewModel()
        {
            var scoresResult = JsonConvert.SerializeObject(_UserScores);
            using (var sw = new StreamWriter(K_SCORES_CONF))
            {
                sw.Write(scoresResult);
            }
        }

        private void LoadSetting()
        {
            using (var sr = new StreamReader(K_SCORES_CONF))
            {
                var scoresJson = sr.ReadToEnd();
                _UserScores = JsonConvert.DeserializeObject<UserScores>(scoresJson);
            }
        }

        private void PlayBGM()
        {
            _BGMPlayer.URL = @"./sounds/timeattack.mp3";
            _BGMPlayer.settings.setMode("loop", true);
            _BGMPlayer.settings.volume = 10;
            _BGMPlayer.controls.play();

        }

        private void CreateCommands()
        {
            ExcecuteCalcCommand = new DelegateCommand(ExcecuteCalc, CanExcecuteCalc);
            SetAnswerCommand = new DelegateCommand<string>(SetAnswer, CanSetAnswerCommand);
        }

        private void UpdateUntilPositive()
        {
            // 暫定的にここで処理する
            // ちゃんと考えて別の場所に移動させること
            if(m_CurrentCalcType == K_OPERATORS[K_ADDSTR] && m_IsTargetedCarryUp == true)
            {
                while(m_FirstValue + m_SecondValue < 10)
                {
                    UpdateProblem();

                }
                SpeechProblem();
            }
            else if (m_CurrentCalcType == K_OPERATORS[K_SUBSTR] && m_FirstValue - m_SecondValue < 0)
            {
                // 0より おおきな すうじに なるようにする
                while (m_FirstValue - m_SecondValue < 0)
                {
                    UpdateProblem();
                }
                SpeechProblem();
            }
        }

        private void PlaySounds(string path)
        {
            _mediaPlayer.URL = path;
            _mediaPlayer.controls.play();
        }

        private void SetAnswer(string val)
        {
            if (int.TryParse(val, out int valInt))
            {
                m_Answer = int.Parse(val);
            }
            else
            {
                throw new Exception("すうじを にゅうりょくしてね");
            }
        }

        private void ExcecuteCalc()
        {
            if (m_Answer == null)
            {
                // MessageBox.Show("");
                return;
            }

            if (CheckAnswer(m_FirstValue, m_SecondValue, m_Answer))
            {
                // せいかい
                m_Points++;
                m_LatestStatus = "◎";
                PlusCorrectAnswerCount();
                UpdateScoresForDisplay();
                if (m_Points % 10 == 0)
                {
                    PlaySounds(K_PIPO2);
                }
                else
                {
                    PlaySounds(K_PIPO);
                }
                UpdateProblem();
                UpdateUntilPositive();
                SpeechProblem();
            }
            else
            {
                // まちがい
                m_Points--;
                m_LatestStatus = "X";
                PlusIncorrectAnswerCount();
                UpdateScoresForDisplay();
                PlaySounds(K_BOO);
            }
            m_Answer = null;
        }

        /// <summary>
        /// 得点表示を更新する
        /// </summary>
        private void UpdateScoresForDisplay()
        {
            var targetScore = SearchTargetScore();
            m_CurOperatorTotalChallenge = targetScore.m_TotalChallenge;
            m_CurOperatorCorrect = targetScore.m_Correct;
            m_CurOperatorIncorrect = targetScore.m_Incorrect;
        }

        /// <summary>
        /// 不正解数を増やす
        /// </summary>
        private void PlusIncorrectAnswerCount()
        {
            var targetScore = SearchTargetScore();
            targetScore.m_TotalChallenge++;
            targetScore.m_Incorrect++;
        }

        /// <summary>
        /// 正解数を増やす
        /// </summary>
        private void PlusCorrectAnswerCount()
        {
            var targetScore = SearchTargetScore();
            targetScore.m_TotalChallenge++;
            targetScore.m_Correct++;
        }

        /// <summary>
        /// 対象のScoreオブジェクトを取得する
        /// </summary>
        /// <returns></returns>
        private Score SearchTargetScore()
        {
            var targetUser = _UserScores.m_Users.Where(x => x.m_Username == m_CurrentSelectedUserName).Select(x => x).First();
            return targetUser.m_Scores.Where(x => x.m_Operator == m_CurrentCalcType).Select(x => x).First();
        }

        private void SpeechProblem()
        {
            SpeechSynthesizer sz = new SpeechSynthesizer();
            sz.Volume = 100;
            sz.Rate = -3;

            var message = m_FirstValue.ToString() + K_OPERATORS.Where(x => x.Value == m_CurrentCalcType).First().Key + m_SecondValue.ToString();
            sz.SpeakAsync(message);
        }

        private void UpdateProblem()
        {
            m_FirstValue = random1.Next(min, max);
            m_SecondValue = random2.Next(min, max);
        }

        private bool CheckAnswer(int first, int second, int? answer)
        {
            if (m_CurrentCalcType == K_OPERATORS[K_ADDSTR])
            {
                return CheckAddAnswer(first, second, answer);
            }
            else if (m_CurrentCalcType == K_OPERATORS[K_SUBSTR])
            {
                return CheckSubAnswer(first, second, answer);
            }
            else
            {
                throw new Exception();
            }
        }

        private bool CheckAddAnswer(int first, int second, int? answer)
        {
            if (first + second == answer) return true;
            else return false;
        }
        private bool CheckSubAnswer(int first, int second, int? answer)
        {
            if (first - second == answer) return true;
            else return false;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
