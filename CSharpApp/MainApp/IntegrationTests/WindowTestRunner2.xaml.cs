using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MainApp.IntegrationTests
{
    /// <summary>
    /// Interaction logic for WindowTestRunner2.xaml
    /// </summary>
    public partial class WindowTestRunner : Window
    {

        private ObservableTestSuiteResults _ObservableResults;

        public virtual ObservableTestSuiteResults ObservableResults
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ObservableResults;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ObservableResults != null)
                {
                    _ObservableResults.MessageAdded -= TestMessages_OnChanged;
                    _ObservableResults.ResultAdded -= TestResults_OnChanged;
                }

                _ObservableResults = value;
                if (_ObservableResults != null)
                {
                    _ObservableResults.MessageAdded += TestMessages_OnChanged;
                    _ObservableResults.ResultAdded += TestResults_OnChanged;
                }
            }
        }


        private BrushConverter converter = new BrushConverter();
        public readonly Brush Green;
        public readonly Brush Red;






        private void TestMessages_OnChanged(object sender, EventArgs e)
        {
            ObservableTestSuiteResults suiteResults = (ObservableTestSuiteResults)sender;
            var messages = suiteResults.Messages;
            string lastItem = messages[messages.Count - 1];
            this.ConsoleRichTextBox.AppendText(lastItem);
            System.Windows.Forms.Application.DoEvents();
        }


        private void TestResults_OnChanged(object sender, EventArgs e)
        {
            ObservableTestSuiteResults suiteResults = (ObservableTestSuiteResults)sender;
            var testResults = suiteResults.TestResults;
            var lastItem = testResults[testResults.Count - 1];
            var tvi = new TreeViewItem();

            tvi.Header = lastItem.TestName;

            if (lastItem.Passed)
            {
                tvi.Items.Add("Passed: " + lastItem.TestName);
                tvi.Background = Green;
                ObservableResults.PassedTests += 1;
            }
            else
            {
                tvi.Items.Add("Failed: " + lastItem.TestName);
                tvi.Background = Red;
                tvi.Items.Add("Exception: " + lastItem.ExceptionMessage);
                string stackTraceString = cadwiki.NetUtils.Lists.StringListToString(lastItem.StackTrace, Environment.NewLine);
                tvi.Items.Add("Stack trace: " + stackTraceString);
                ObservableResults.FailedTests += 1;
            }
            ObservableResults.TotalTests += 1;
            this.ResultsTree.Items.Add(tvi);
            this.ResultsTree.Items.Refresh();
            System.Windows.Forms.Application.DoEvents();
        }

        public void AddResult()
        {
            var tvi = CreateResultItem();
            this.ResultsTree.Items.Add(tvi);
        }
        public void UpdateResult()
        {
            var tvi = CreateResultItem();
            this.ResultsTree.Items[0] = tvi;
        }

        private TreeViewItem CreateResultItem()
        {
            var tvi = new TreeViewItem();
            tvi.Header = "Test Run Results: " + ObservableResults.TimeElapsed;
            tvi.Items.Add("Total: " + ObservableResults.TotalTests.ToString());
            tvi.Items.Add("Passed: " + ObservableResults.PassedTests.ToString());
            tvi.Items.Add("Failed: " + ObservableResults.FailedTests.ToString());
            tvi.Items.Add("Time Elasped: " + ObservableResults.TimeElapsed);
            tvi.IsExpanded = true;
            return tvi;
        }

        public WindowTestRunner()
        {
            ObservableResults = new ObservableTestSuiteResults();
            Green = (Brush)converter.ConvertFromString("#00FF00");
            Red = (Brush)converter.ConvertFromString("#FF0000");
            this.InitializeComponent();
            Init();
        }

        public void Init()
        {

            this.ConsoleRichTextBox.AppendText(Environment.NewLine + "NunitTestRunner started");
        }
        public WindowTestRunner(ObservableTestSuiteResults suiteResults)
        {
            ObservableResults = new ObservableTestSuiteResults();
            Green = (Brush)converter.ConvertFromString("#00FF00");
            Red = (Brush)converter.ConvertFromString("#FF0000");
            this.InitializeComponent();
            Init();

        }

        private void ButtonOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
