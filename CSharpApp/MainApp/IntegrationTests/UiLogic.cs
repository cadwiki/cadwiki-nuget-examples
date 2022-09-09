using System;
using System.Diagnostics;
using Microsoft.VisualBasic;


namespace MainApp.IntegrationTests
{
    public class UiLogic
    {
        public static void RunTestsFromType(ObservableTestSuiteResults suiteResult, Stopwatch stopwatch, Type[] integrationTestTypes)
        {
            var tuples = Utils.GetTestMethodDictionarySafely(integrationTestTypes);

            var setupTuple = Utils.GetSetupMethod(integrationTestTypes);
            object setupObject = null;
            System.Reflection.MethodInfo setupMethodInfo = null;
            if (setupTuple != null)
            {
                var setupType = setupTuple.Item1;
                setupObject = Activator.CreateInstance(setupType);
                setupMethodInfo = setupTuple.Item2;
            }

            var tearDownTuple = Utils.GetTearDownMethod(integrationTestTypes);
            object tearDownObject = null;
            System.Reflection.MethodInfo tearDownMethodInfo = null;
            if (tearDownTuple != null)
            {
                var tearDownType = tearDownTuple.Item1;
                tearDownObject = Activator.CreateInstance(tearDownType);
                tearDownMethodInfo = tearDownTuple.Item2;
            }


            foreach (Tuple<Type, System.Reflection.MethodInfo> item in tuples)
            {
                var testResult = new TestResult();
                var type = item.Item1;
                var mi = item.Item2;
                try
                {
                    suiteResult.AddMessage(Environment.NewLine + "Running test method: " + mi.Name);
                    object o;
                    o = Activator.CreateInstance(type);
                    if (setupMethodInfo != null & setupObject != null)
                    {
                        setupMethodInfo.Invoke(setupObject, null);
                    }
                    mi.Invoke(o, null);
                    testResult.TestName = mi.Name;
                    testResult.Passed = true;
                    if (tearDownMethodInfo != null & tearDownObject != null)
                    {
                        tearDownMethodInfo.Invoke(tearDownObject, null);
                    }
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    if (ex.InnerException is NUnit.Framework.AssertionException)
                    {
                        NUnit.Framework.AssertionException ae = (NUnit.Framework.AssertionException)ex.InnerException;
                        var result = ae.ResultState;
                        testResult.TestName = mi.Name;
                        testResult.Passed = false;
                        testResult.ExceptionMessage = ae.Message;
                        testResult.StackTrace = cadwiki.NetUtils.Exceptions.GetStackTraceLines(ae);
                    }
                    else
                    {
                        testResult.TestName = mi.Name;
                        testResult.Passed = false;
                        testResult.ExceptionMessage = ex.InnerException.Message;
                        testResult.StackTrace = cadwiki.NetUtils.Exceptions.GetStackTraceLines(ex.InnerException);
                    }
                }

                catch (Exception ex)
                {
                    testResult.TestName = mi.Name;
                    testResult.Passed = false;
                    testResult.ExceptionMessage = ex.Message;
                    testResult.StackTrace = cadwiki.NetUtils.Exceptions.GetStackTraceLines(ex);
                }
                suiteResult.AddResult(testResult);


            }
            stopwatch.Stop();

            var ts = stopwatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10d);
            if (ts.TotalMinutes > 5d)
            {
                suiteResult.TimeElapsed = "Consider removing tests to reduce elapsed time to below 5 minutes " + elapsedTime;
            }
            else
            {
                suiteResult.TimeElapsed = elapsedTime;
            }
        }
    }
}