{ pkgs ? import <nixpkgs> {} }:
let
  dotnet = with pkgs.dotnetCorePackages; combinePackages [
    sdk_8_0-bin
    runtime_8_0-bin
    aspnetcore_8_0-bin
    sdk_9_0-bin
  ];

  # Create a VS Code with the needed extensions bundled for stability
  vscode-fhs = (pkgs.vscode-with-extensions.override {
    vscode = pkgs.vscode.fhs; # Use the FHS-compatible version!
    vscodeExtensions = with pkgs.vscode-extensions; [
      ms-dotnettools.vscode-dotnet-runtime
      ms-dotnettools.csharp
      ms-dotnettools.csdevkit
    ];
  });

in
pkgs.mkShell {
  allowUnfree = true;

  packages = [
    dotnet
    vscode-fhs
    pkgs.omnisharp-roslyn
    pkgs.netcoredbg
  ];

  DOTNET_ROOT = "${dotnet}";

  shellHook = ''
    echo "Launching FHS VS Code with C# extensions..."
    code .
  '';
}
