@echo off
REM Build solution
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" Online_Bookstore.sln /p:Configuration=Debug
IF %ERRORLEVEL% NEQ 0 (
    echo Build failed
    exit /b 1
)

REM Note: No test project currently exists
REM When you add tests, uncomment the following lines:
REM vstest.console.exe Online_Bookstore.Tests.dll
REM IF %ERRORLEVEL% NEQ 0 (
REM     echo Tests failed
REM     exit /b 1
REM )

echo Build passed
exit /b 0
