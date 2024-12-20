// <copyright file="Device.cs" company="Mathematics.NET">
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

using System.Text;
using Silk.NET.OpenCL;

namespace Mathematics.NET.GPU.OpenCL;

/// <summary>Represents an OpenCL device.</summary>
public sealed class Device : IOpenCLObject
{
    private readonly CL _cl;

    // Device information, listed mostly alphabetically.
    public readonly DeviceType DeviceType;
    public readonly uint MaxComputeUnits;
    public readonly nuint MaxWorkGroupSize;
    public readonly uint MaxWorkItemDimensions;
    public readonly Memory<nuint> MaxWorkItemSizes;
    public readonly string Name;
    public readonly string Vendor;

    public unsafe Device(CL cl, nint handle)
    {
        _cl = cl;
        Handle = handle;

        // Device type.
        ulong deviceType;
        _cl.GetDeviceInfo(Handle, DeviceInfo.Type, sizeof(ulong), &deviceType, null);
        DeviceType = (DeviceType)deviceType;

        // Device name.
        _cl.GetDeviceInfo(Handle, DeviceInfo.Name, 0, null, out var nameSize);
        Span<byte> nameSpan = new byte[nameSize];
        _cl.GetDeviceInfo(Handle, DeviceInfo.Name, nameSize, nameSpan, []);
        Name = Encoding.UTF8.GetString(nameSpan).TrimEnd('\0');

        // Device max compute units.
        uint maxComputeUnits;
        _cl.GetDeviceInfo(Handle, DeviceInfo.MaxComputeUnits, sizeof(uint), &maxComputeUnits, null);
        MaxComputeUnits = maxComputeUnits;

        // Device max work group size.
        nuint maxWorkGroupSize;
        _cl.GetDeviceInfo(Handle, DeviceInfo.MaxWorkGroupSize, (nuint)sizeof(nuint), &maxWorkGroupSize, null);
        MaxWorkGroupSize = maxWorkGroupSize;

        // Device max work item dimensions.
        uint maxWorkItemDimensions;
        _cl.GetDeviceInfo(Handle, DeviceInfo.MaxWorkItemDimensions, sizeof(uint), &maxWorkItemDimensions, null);
        MaxWorkItemDimensions = maxWorkItemDimensions;

        // Device max work item sizes; must come after finding max work item dimensions.
        Span<nuint> maxWorkItemSizes = new nuint[(int)MaxWorkItemDimensions];
        _cl.GetDeviceInfo(Handle, DeviceInfo.MaxWorkItemSizes, (nuint)(MaxWorkItemDimensions * sizeof(nuint)), maxWorkItemSizes, []);
        MaxWorkItemSizes = maxWorkItemSizes.ToArray();

        // Device vendor.
        _cl.GetDeviceInfo(Handle, DeviceInfo.Vendor, 0, null, out var vendorSize);
        Span<byte> vendorSpan = new byte[vendorSize];
        _cl.GetDeviceInfo(Handle, DeviceInfo.Vendor, vendorSize, vendorSpan, []);
        Vendor = Encoding.UTF8.GetString(vendorSpan).TrimEnd('\0');
    }

    public void Dispose()
    {
        if (Handle != 0)
        {
            _ = _cl.ReleaseDevice(Handle);
            Handle = 0;
        }
    }

    public nint Handle { get; private set; }

    public bool IsValidInstance => Handle != 0;
}
