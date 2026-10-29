#define MyAppName "Hmed-S-Tetris"
#define MyAppVersion "v0.2.0-beta"
#define MyAppPublisher "Ahmed Siamar"
#define MyAppExeName "Hmed-S-Tetris"

[Setup]
AppId={{A57C02BD-13C9-4D73-AF25-89E8B2C1D746}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={localappdata}\Programs\{#MyAppName}
DefaultGroupName={#MyAppName}
PrivilegesRequired=lowest
UninstallDisplayIcon={app}\{#MyAppExeName}

OutputDir=..\artifacts\installer
OutputBaseFilename=Hmed-S-Tetris-Setup-{#MyAppVersion}

Compression=lzma2
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; \
    Description: "Create a &desktop shortcut"; \
    GroupDescription: "Additional shortcuts:"; \
    Flags: unchecked

[Files]
Source: "..\bin\Release\net10.0-windows\win-x64\*"; \
    DestDir: "{app}"; \
    Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; \
    Filename: "{app}\{#MyAppExeName}"

Name: "{autodesktop}\{#MyAppName}"; \
    Filename: "{app}\{#MyAppExeName}"; \
    Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; \
    Description: "Launch {#MyAppName}"; \
    Flags: postinstall nowait skipifsilent
