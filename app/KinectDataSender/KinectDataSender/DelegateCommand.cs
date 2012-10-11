using System;
using System.Windows.Input;

namespace KinectDataSender
{
    /// <summary>
    /// Delegate を使う ICommand の実装クラス
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private Action<object> _executeAction;
        private Func<object, bool> _canExecuteAction;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executeAction">実行するアクション</param>
        /// <param name="canExecuteAction">実行可能かの判定を行うアクション</param>
        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        /// <summary>
        /// 実行可能かの判定
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns>実行可能なら true</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecuteAction(parameter);
        }

        /// <summary>
        /// CommandManager からイベント発行してもらうようにする
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}
