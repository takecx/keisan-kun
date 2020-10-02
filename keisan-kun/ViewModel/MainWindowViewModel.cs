using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using System.Collections.ObjectModel;
using WMPLib;
using System.Speech.Synthesis;
using keisan_kun.Model;
using Newtonsoft.Json;
using System.IO;

namespace keisan_kun.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        #region ていすう
        private readonly string K_ADDSTR = "たす";
        private readonly string K_SUBSTR = "ひく";
        private readonly Dictionary<string, string> K_OPERATORS;
        private readonly string K_BOO = @"./sounds/boo.mp3";
        private readonly string K_PIPO = @"./sounds/pipo.mp3";
        private readonly string K_PIPO2 = @"./sounds/pipopipo.mp3";
        private readonly string K_SCORES_CONF = @"./scores.json";
        #endregion

        private WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();
        private WindowsMediaPlayer _BGMPlayer = new WindowsMediaPlayer();
        private int max = 10;
        private int min = 0;
        private Random random1 = new Random(1);
        private Random random2 = new Random(2);
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
            set {
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
            }
        }

        private void UpdateUntilPositive()
        {
            if (m_CurrentCalcType ==　K_OPERATORS[K_SUBSTR] && m_FirstValue - m_SecondValue < 0)
            {
                // 0より おおきな すうじに なるようにする
                while (m_FirstValue - m_SecondValue < 0)
                {
                    UpdateProblem();
                }
                SpeechProblem();
            }
        }

        private string _LatestStatus;
        public string m_LatestStatus
        {
            get { return _LatestStatus; }
            set { this.SetProperty(ref this._LatestStatus, value); }
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

        public MainWindowViewModel()
        {
            CreateCommands();
            LoadSetting();

            m_Points = 0;
            K_OPERATORS = new Dictionary<string, string>() { { K_ADDSTR, "+" }, { K_SUBSTR, "-" } };
            m_CalcTypes = new ObservableCollection<string>(K_OPERATORS.Values);
            m_CurrentCalcType = K_OPERATORS[K_ADDSTR];

            //PlayBGM();
            UpdateProblem();
            SpeechProblem();
        }

        private void LoadSetting()
        {
            using(var sr = new StreamReader(K_SCORES_CONF))
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

        private void PlaySounds(string path)
        {
            _mediaPlayer.URL = path;
            _mediaPlayer.controls.play();
        }

        private void SetAnswer(string val)
        {
            if(int.TryParse(val,out int valInt))
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
            if(m_Answer == null)
            {
                // MessageBox.Show("");
                return;
            }

            if (CheckAnswer(m_FirstValue, m_SecondValue, m_Answer))
            {
                // せいかい
                m_Points++;
                m_LatestStatus = "◎";
                if(m_Points % 10 == 0)
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
                PlaySounds(K_BOO);
            }
            m_Answer = null;
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
            if(m_CurrentCalcType == K_OPERATORS[K_ADDSTR])
            {
                return CheckAddAnswer(first, second, answer);
            }
            else if(m_CurrentCalcType == K_OPERATORS[K_SUBSTR])
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

    }
}
