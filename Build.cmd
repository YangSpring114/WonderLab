@echo off
ver|findstr /r /i " [版本 10.0.*]" > NUL && goto Windows10+
goto UnknownVersion

:Windows10+
winget install Microsoft.Dotnet.SDK.6
goto Build

:UnknownVersion
echo 请确认你已经安装了.net6 sdk
pause
goto Build

:Build
dotnet build --configuration Release

echo 编译完成 请到WonderLab\WonderLab\bin查看结果
pause
