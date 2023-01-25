open System
open System.IO
open System.Diagnostics

let GhostScriptExe = @"C:\Program Files\gs\gs10.00.0\bin\gswin64c.exe"

let getFilePaths () =
    let opts = EnumerationOptions(RecurseSubdirectories = true)
    Directory.GetFiles(".", "*.eps", opts) |> Seq.map Path.GetFullPath

let convertEpsToPng (path: String) =
    let outputFile = Path.ChangeExtension(path, "png")

    let arguments =
        [ "-dSAFER"
          "-dBATCH"
          "-dNOPAUSE"
          "-dEPSCrop"
          "-r96"
          "-q"
          "-sDEVICE=pngalpha"
          $"-sOutputFile=\"{outputFile}\""
          $"\"{path}\"" ]
        |> List.toArray
        |> fun a -> String.Join(" ", a)

    //printfn $"{GhostScriptExe} {arguments}"
    printfn $"Converting {Path.GetFileName(path)}"
    let pi = ProcessStartInfo(FileName = GhostScriptExe, Arguments = arguments)
    let p = Process.Start(pi)
    p.WaitForExit()

getFilePaths () |> Seq.iter convertEpsToPng
