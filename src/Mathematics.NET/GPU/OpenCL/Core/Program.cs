// <copyright file="Program.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023-present Hamlet Tanyavong
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>

// TODO: Consider making a method to retrieve file names.
// Mathematics.NET.GPU.OpenCL.Kernels.{name}.cl
//                                    ^    ^
//                                   35   ^3

#pragma warning disable IDE0058, IDE0305

using System.Reflection;
using System.Text;
using Mathematics.NET.Exceptions;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL.Core;

/// <summary>Represents an OpenCL program.</summary>
public sealed partial class Program : IOpenCLObject
{
    private readonly ILogger<OpenCLService> _logger;
    private readonly CL _cl;

    public unsafe Program(ILogger<OpenCLService> logger, CL cl, Context context, ReadOnlySpan<Device> devices, params ReadOnlySpan<string> options)
    {
        _logger = logger;
        _cl = cl;

        // This only works if the contents of the header file are copied to every kernel that would have used an #include directive.
        // Make sure that all contents of the Kernel folder are marked as embedded resources in the project file.

        var assembly = Assembly.GetExecutingAssembly();

        var files = assembly.GetManifestResourceNames()
            .Where(x => x.StartsWith("Mathematics.NET.GPU.OpenCL.Kernels") && (x.EndsWith(".c") || x.EndsWith(".cl")))
            .ToArray();

        var code = new string[files.Length];
        var codeLengths = new nuint[files.Length];
        List<string> kernels = [];

        var headers = ParseHeaders(assembly);

        StringBuilder builder = new();
        for (int i = 0; i < files.Length; i++)
        {
            using var stream = assembly.GetManifestResourceStream(files[i]) ?? throw new Exception("Unable to get manifest resource stream.");
            using var reader = new StreamReader(stream);

            while (reader.ReadLine() is string line)
            {
                if (line.StartsWith("#include"))
                {
                    var quoteStart = line.IndexOf('"') + 1;
                    if (headers.TryGetValue(line[quoteStart..^1], out var contents))
                        builder.AppendLine(contents);
                    else
                        throw new ComputeServiceException($"Unable to locate header file: {line[quoteStart..^1]}.");
                }
                else
                {
                    builder.AppendLine(line);
                }
            }

            var text = builder.ToString();
            code[i] = text;
            codeLengths[i] = (nuint)text.Length;

            if (files[i].EndsWith(".cl"))
                kernels.Add(files[i][35..^3]);

            builder.Clear();
        }

        fixed (nuint* pCodeLengths = codeLengths)
        {
            Handle = _cl.CreateProgramWithSource(context.Handle, (uint)code.Length, code, pCodeLengths, out var error);
            ComputeServiceException.ThrowIfNotSuccess(error, "Unable to create the program from source.");
        }

        fixed (nint* pDevices = devices.ToArray().Select(x => x.Handle).ToArray())
        {
            var optionsString = Encoding.UTF8.GetBytes(string.Join(' ', options.ToArray().Where(x => !string.IsNullOrEmpty(x))));
            fixed (byte* pOptionsString = optionsString)
            {
                var error = _cl.BuildProgram(Handle, (uint)devices.Length, pDevices, pOptionsString, null, null);
                if (error != (int)ErrorCodes.Success)
                {
                    _cl.GetProgramBuildInfo(Handle, *pDevices, ProgramBuildInfo.BuildLog, 0, null, out var infoSize);
                    Span<byte> infoSpan = new byte[infoSize];
                    _cl.GetProgramBuildInfo(Handle, *pDevices, ProgramBuildInfo.BuildLog, infoSize, infoSpan, []);
                    throw new ComputeServiceException(Encoding.UTF8.GetString(infoSpan));
                }
            }
        }

        foreach (var kernel in kernels)
        {
            CreateKernel(kernel);
        }
    }

    public void Dispose()
    {
        foreach (var entry in Kernels)
        {
            if (entry.Value is Kernel kernel && kernel.IsValidInstance)
                kernel.Dispose();
        }

        if (Handle != 0)
        {
            _cl.ReleaseProgram(Handle);
            Handle = 0;
        }
    }

    public nint Handle { get; private set; }

    public bool IsValidInstance => Handle != 0;

    public Dictionary<string, Kernel> Kernels { get; } = [];

    private void CreateKernel(string name)
    {
        var kernel = new Kernel(_cl, this, name);
        Kernels.Add(name, kernel);
    }

    private static Dictionary<string, string> ParseHeaders(Assembly assembly)
    {
        var headers = assembly.GetManifestResourceNames()
            .Where(x => x.StartsWith("Mathematics.NET.GPU.OpenCL.Kernels") && x.EndsWith(".h"))
            .ToArray();

        Dictionary<string, string> contents = [];
        foreach (var header in headers)
        {
            var name = Path.GetFileName(header)[35..];
            using var stream = assembly.GetManifestResourceStream(header) ?? throw new Exception("Unable to get manifest resource stream.");
            using var reader = new StreamReader(stream!);
            char[] text = new char[stream.Length - 1];
            reader.Read(text, 0, (int)stream.Length - 1);
            contents.Add(name, new(text));
        }

        return contents;
    }
}
