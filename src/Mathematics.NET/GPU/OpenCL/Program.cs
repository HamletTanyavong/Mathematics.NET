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

// TODO: Create a source generator that finds kernels in the Kernels folder and adds them automatically.

#pragma warning disable IDE0058

using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents an OpenCL program.</summary>
public sealed class Program : IOpenCLObject
{
    private readonly CL _cl;
    private readonly ILogger<OpenCLService> _logger;

    public unsafe Program(ILogger<OpenCLService> logger, CL cl, Context context, ReadOnlySpan<Device> devices)
    {
        _cl = cl;
        _logger = logger;

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Mathematics.NET.GPU.OpenCL.Kernels.vec_mul_scalar.cl");
        if (stream is not null)
        {
            using var reader = new StreamReader(stream);
            var kernelCode = reader.ReadToEnd();

            Handle = _cl.CreateProgramWithSource(context.Handle, 1, [kernelCode], null, out var error);
#if DEBUG
            if (error != (int)ErrorCodes.Success)
            {
                _logger.LogDebug("Unable to create the program from source.");
            }
#endif

            fixed (nint* pDevices = devices.ToArray().Select(x => x.Handle).ToArray())
            {
                error = _cl.BuildProgram(Handle, (uint)devices.Length, pDevices, (byte*)null, null, null);
                if (error != (int)ErrorCodes.Success)
                {
                    _cl.GetProgramBuildInfo(Handle, *pDevices, ProgramBuildInfo.BuildLog, 0, null, out var infoSize);
                    Span<byte> infoSpan = new byte[infoSize];
                    _cl.GetProgramBuildInfo(Handle, *pDevices, ProgramBuildInfo.BuildLog, infoSize, infoSpan, []);
                    _cl.ReleaseProgram(Handle);
                    throw new Exception(Encoding.UTF8.GetString(infoSpan));
                }
            }
        }
        else
        {
            _logger.LogDebug("Unable to find the embedded resource.");
        }

        // Create kernels.
        CreateKernel("vec_mul_scalar");
    }

    public void Dispose()
    {
        foreach (var entry in Kernels)
        {
            if (entry.Value is Kernel kernel && kernel.IsValidInstance)
            {
                kernel.Dispose();
            }
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

    public void CreateKernel(string name)
    {
        var kernel = new Kernel(_logger, _cl, this, name);
        Kernels.Add(name, kernel);
    }
}
