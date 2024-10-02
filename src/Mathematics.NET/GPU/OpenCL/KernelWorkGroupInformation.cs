// <copyright file="KernelWorkGroupInformation.cs" company="Mathematics.NET">
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

#pragma warning disable IDE0058

using Microsoft.Extensions.Logging;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents a kernel work group info object.</summary>
public sealed class KernelWorkGroupInformation
{
    // Api.
    private CL _cl;

    // Kernel work group information.
    public Memory<nuint> KernelCompileWorkGroupSize;
    public Memory<nuint> KernelGlobalWorkSize;
    public ulong KernelLocalMemSize;
    public nuint KernelPreferredWorkGroupSizeMultiple;
    public ulong KernelPrivateMemSize;
    public nuint KernelWorkGroupSize;

    public unsafe KernelWorkGroupInformation(ILogger<OpenCLService> logger, CL cl, Device device, Kernel kernel)
    {
        _cl = cl;

        // Kernel compile work group size.
        Span<nuint> kernelCompileWorkGroupSize = stackalloc nuint[3];
        _cl.GetKernelWorkGroupInfo(kernel.Handle, device.Handle, KernelWorkGroupInfo.CompileWorkGroupSize, (nuint)(3 * sizeof(nuint)), kernelCompileWorkGroupSize, []);
        KernelCompileWorkGroupSize = kernelCompileWorkGroupSize.ToArray();

        // Kernel global work size.
        Span<nuint> kernelGlobalWorkSize = stackalloc nuint[3];
        var error = _cl.GetKernelWorkGroupInfo(kernel.Handle, device.Handle, KernelWorkGroupInfo.GlobalWorkSize, (nuint)(3 * sizeof(nuint)), kernelGlobalWorkSize, []);
        if (error != (int)ErrorCodes.Success)
        {
            if (error == (int)ErrorCodes.InvalidValue)
                logger.LogDebug("This device does not support the parameter CL_KERNEL_GLOBAL_WORK_SIZE for clGetKernelWorkGroupInfo.");
            KernelGlobalWorkSize = Array.Empty<nuint>();
        }
        else
        {
            KernelGlobalWorkSize = kernelGlobalWorkSize.ToArray();
        }

        // Kernel local mem size.
        ulong kernelLocalMemSize;
        _cl.GetKernelWorkGroupInfo(kernel.Handle, device.Handle, KernelWorkGroupInfo.LocalMemSize, sizeof(ulong), &kernelLocalMemSize, null);
        KernelLocalMemSize = kernelLocalMemSize;

        // Kernel preferred work group size multiple.
        nuint kernelPreferredWorkGroupSizeMultiple;
        _cl.GetKernelWorkGroupInfo(kernel.Handle, device.Handle, KernelWorkGroupInfo.PreferredWorkGroupSizeMultiple, (nuint)sizeof(nuint), &kernelPreferredWorkGroupSizeMultiple, null);
        KernelPreferredWorkGroupSizeMultiple = kernelPreferredWorkGroupSizeMultiple;

        // Kernel private mem size.
        ulong kernelPrivateMemSize;
        _cl.GetKernelWorkGroupInfo(kernel.Handle, device.Handle, KernelWorkGroupInfo.PrivateMemSize, sizeof(ulong), &kernelPrivateMemSize, null);
        KernelPrivateMemSize = kernelPrivateMemSize;

        // Kernel work group size.
        nuint kernelWorkGroupSize;
        _cl.GetKernelWorkGroupInfo(kernel.Handle, device.Handle, KernelWorkGroupInfo.WorkGroupSize, (nuint)sizeof(nuint), &kernelWorkGroupSize, null);
        KernelWorkGroupSize = kernelWorkGroupSize;
    }
}
