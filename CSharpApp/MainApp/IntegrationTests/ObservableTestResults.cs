using System;
using System.Collections.Generic;

namespace MainApp.IntegrationTests
{

    public class TestResult
    {
        public string TestName;
        public bool Passed;
        public string ExceptionMessage;
        public List<string> StackTrace;
    }

    public class ObservableTestSuiteResults
    {
        public string TimeElapsed;
        public int TotalTests;
        public int PassedTests;
        public int FailedTests;
        public List<string> Messages = new List<string>();
        public List<TestResult> TestResults = new List<TestResult>();

        public event MessageAddedEventHandler MessageAdded;

        public delegate void MessageAddedEventHandler(object sender, EventArgs e);
        public void AddMessage(string newItem)
        {
            Messages.Add(newItem);
            MessageAdded?.Invoke(this, new EventArgs());
        }


        public event ResultAddedEventHandler ResultAdded;

        public delegate void ResultAddedEventHandler(object sender, EventArgs e);
        public void AddResult(TestResult newItem)
        {
            TestResults.Add(newItem);
            ResultAdded?.Invoke(this, new EventArgs());
        }

    }


}