

Namespace IntegrationTests
    Public Class UiLogic
        Public Shared Sub RunTestsFromType(suiteResult As ObservableTestSuiteResults, stopwatch As Stopwatch, integrationTestTypes As Type())
            Dim tuples As List(Of Tuple(Of Type, Reflection.MethodInfo)) = IntegrationTests.Utils.GetTestMethodDictionarySafely(integrationTestTypes)

            Dim setupTuple As Tuple(Of Type, Reflection.MethodInfo) = IntegrationTests.Utils.GetSetupMethod(integrationTestTypes)
            Dim setupObject As Object = Nothing
            Dim setupMethodInfo As Reflection.MethodInfo = Nothing
            If setupTuple IsNot Nothing Then
                Dim setupType As Type = setupTuple.Item1
                setupObject = Activator.CreateInstance(setupType)
                setupMethodInfo = setupTuple.Item2
            End If

            Dim tearDownTuple As Tuple(Of Type, Reflection.MethodInfo) = IntegrationTests.Utils.GetTearDownMethod(integrationTestTypes)
            Dim tearDownObject As Object = Nothing
            Dim tearDownMethodInfo As Reflection.MethodInfo = Nothing
            If tearDownTuple IsNot Nothing Then
                Dim tearDownType As Type = tearDownTuple.Item1
                tearDownObject = Activator.CreateInstance(tearDownType)
                tearDownMethodInfo = tearDownTuple.Item2
            End If


            For Each item As Tuple(Of Type, Reflection.MethodInfo) In tuples
                Dim testResult As New TestResult
                Dim type As Type = item.Item1
                Dim mi As Reflection.MethodInfo = item.Item2
                Try
                    suiteResult.AddMessage(vbLf & "Running test method: " + mi.Name)
                    Dim o As Object
                    o = Activator.CreateInstance(type)
                    If setupMethodInfo IsNot Nothing And setupObject IsNot Nothing Then
                        setupMethodInfo.Invoke(setupObject, Nothing)
                    End If
                    mi.Invoke(o, Nothing)
                    testResult.TestName = mi.Name
                    testResult.Passed = True
                    If tearDownMethodInfo IsNot Nothing And tearDownObject IsNot Nothing Then
                        tearDownMethodInfo.Invoke(tearDownObject, Nothing)
                    End If
                Catch ex As System.Reflection.TargetInvocationException
                    If (TypeOf ex.InnerException Is NUnit.Framework.AssertionException) Then
                        Dim ae As NUnit.Framework.AssertionException = CType(ex.InnerException, NUnit.Framework.AssertionException)
                        Dim result As NUnit.Framework.Interfaces.ResultState = ae.ResultState
                        testResult.TestName = mi.Name
                        testResult.Passed = False
                        testResult.ExceptionMessage = ae.Message
                        testResult.StackTrace = cadwiki.NetUtils.Exceptions.GetStackTraceLines(ae)
                    Else
                        testResult.TestName = mi.Name
                        testResult.Passed = False
                        testResult.ExceptionMessage = ex.InnerException.Message
                        testResult.StackTrace = cadwiki.NetUtils.Exceptions.GetStackTraceLines(ex.InnerException)
                    End If

                Catch ex As Exception
                    testResult.TestName = mi.Name
                    testResult.Passed = False
                    testResult.ExceptionMessage = ex.Message
                    testResult.StackTrace = cadwiki.NetUtils.Exceptions.GetStackTraceLines(ex)
                End Try
                suiteResult.AddResult(testResult)


            Next
            stopwatch.Stop()

            Dim ts As TimeSpan = stopwatch.Elapsed
            Dim elapsedTime As String = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10)
            If ts.TotalMinutes > 5 Then
                suiteResult.TimeElapsed = "Consider removing tests to reduce elapsed time to below 5 minutes " + elapsedTime
            Else
                suiteResult.TimeElapsed = elapsedTime
            End If
        End Sub
    End Class
End Namespace

