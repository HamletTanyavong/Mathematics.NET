// <copyright file="VulkanDevice.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023 Hamlet Tanyavong
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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;

namespace Mathematics.NET.Renderer;

public unsafe class VulkanDevice : IDisposable
{
    private readonly Vk _vk = null!;
    private readonly VulkanWindow _vulkanWindow;

    private Instance _instance;

    private PhysicalDevice _physicalDevice;
    private SampleCountFlags _sampleCountFlags = SampleCountFlags.Count1Bit;
    private Device _device;

    private KhrSurface _khrSurface = null!;
    private SurfaceKHR _surface;

    private uint _graphicsFamilyIndex;
    private Queue _graphicsQueue;
    private Queue _presentQueue;

    private CommandPool _commandPool;

    private string[] _deviceExtensions = new[]
    {
        KhrSwapchain.ExtensionName
    };

#if DEBUG
    private bool _enableValidationLayers = true;
#else
    private bool _enableValidationLayers = false;
#endif
    private string[] _validationLayers = { "VK_LAYER_KHRONOS_validation" };

    private ExtDebugUtils _extDebugUtils = null!;
    private DebugUtilsMessengerEXT _debugMessenger;

    private bool _disposed;

    public VulkanDevice(Vk vk, VulkanWindow vulkanWindow)
    {
        _vk = vk;
        _vulkanWindow = vulkanWindow;

        CreateInstance();
        SetUpDebugMessenger();
        CreateSurface();
        PickPhysicalDevice();
        CreateLogicalDevice();
        CreateCommandPool();
    }

    public CommandPool CommandPool => _commandPool;

    public Queue GraphicsQueue => _graphicsQueue;

    public Instance Instance => _instance;

    public Queue PresentQueue => _presentQueue;

    public SurfaceKHR Surface => _surface;

    public Device Device => _device;

    //
    // IDisposable interface
    //

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _khrSurface.Dispose();
                _extDebugUtils?.Dispose();
            }

            _disposed = true;
        }
    }

    //
    // Constructor methods
    //

    private void CreateCommandPool()
    {
        var queueFamilyIndicies = FindQueueFamilies(_physicalDevice);

        CommandPoolCreateInfo poolInfo = new()
        {
            SType = StructureType.CommandPoolCreateInfo,
            QueueFamilyIndex = queueFamilyIndicies.GraphicsFamily!.Value,
            Flags = CommandPoolCreateFlags.ResetCommandBufferBit | CommandPoolCreateFlags.TransientBit
        };

        if (_vk.CreateCommandPool(_device, poolInfo, null, out _commandPool) != Result.Success)
        {
            throw new Exception("Failed to create command pool");
        }
    }

    private void CreateInstance()
    {
        if (_enableValidationLayers && !ValidationLayersSupported())
        {
            throw new Exception("Validation layers were requested but are not available");
        }

        ApplicationInfo appInfo = new()
        {
            SType = StructureType.ApplicationInfo,
            PApplicationName = (byte*)Marshal.StringToHGlobalAnsi("Renderer"),
            ApplicationVersion = new Version32(0, 1, 0),
            PEngineName = (byte*)Marshal.StringToHGlobalAnsi("No Engine"),
            EngineVersion = new Version32(0, 1, 0),
            ApiVersion = Vk.Version13
        };

        InstanceCreateInfo createInfo = new()
        {
            SType = StructureType.InstanceCreateInfo,
            PApplicationInfo = &appInfo
        };

        var extensions = GetRequiredExtensions();
        createInfo.EnabledExtensionCount = (uint)extensions.Length;
        createInfo.PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(extensions);

        if (_enableValidationLayers)
        {
            createInfo.EnabledLayerCount = (uint)_validationLayers.Length;
            createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(_validationLayers);

            DebugUtilsMessengerCreateInfoEXT debugUtilsCreateInfo = new();
            PopulateDebugMessengerCreateInfo(ref debugUtilsCreateInfo);
            createInfo.PNext = &debugUtilsCreateInfo;
        }
        else
        {
            createInfo.EnabledLayerCount = 0;
            createInfo.PNext = null;
        }

        if (_vk.CreateInstance(createInfo, null, out _instance) != Result.Success)
        {
            throw new Exception("Failed to create instance");
        }

        Marshal.FreeHGlobal((IntPtr)appInfo.PApplicationName);
        Marshal.FreeHGlobal((IntPtr)appInfo.PEngineName);
        _ = SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);

        if (_enableValidationLayers)
        {
            _ = SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
        }
    }

    private void CreateLogicalDevice()
    {
        var indices = FindQueueFamilies(_physicalDevice);

        var uniqueQueueFamilies = new[] { indices.GraphicsFamily!.Value, indices.PresentFamily!.Value };
        uniqueQueueFamilies = uniqueQueueFamilies.Distinct().ToArray();

        _graphicsFamilyIndex = indices.GraphicsFamily.Value;

        using var memory = GlobalMemory.Allocate(uniqueQueueFamilies.Length * sizeof(DeviceQueueCreateInfo));
        var queueCreateInfo = (DeviceQueueCreateInfo*)Unsafe.AsPointer(ref memory.GetPinnableReference());

        float queuePriority = 1.0f;
        for (int i = 0; i < uniqueQueueFamilies.Length; i++)
        {
            queueCreateInfo[i] = new()
            {
                SType = StructureType.DeviceQueueCreateInfo,
                QueueFamilyIndex = uniqueQueueFamilies[i],
                QueueCount = 1
            };
            queueCreateInfo[i].PQueuePriorities = &queuePriority;
        }

        PhysicalDeviceFeatures deviceFeatures = new()
        {
            SamplerAnisotropy = true
        };

        DeviceCreateInfo createInfo = new()
        {
            SType = StructureType.DeviceCreateInfo,
            QueueCreateInfoCount = (uint)uniqueQueueFamilies.Length,
            PQueueCreateInfos = queueCreateInfo,

            PEnabledFeatures = &deviceFeatures,

            EnabledExtensionCount = (uint)_deviceExtensions.Length,
            PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(_deviceExtensions)
        };

        if (_enableValidationLayers)
        {
            createInfo.EnabledLayerCount = (uint)_validationLayers.Length;
            createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(_validationLayers);
        }
        else
        {
            createInfo.EnabledLayerCount = 0;
        }

        if (_vk.CreateDevice(_physicalDevice, in createInfo, null, out _device) != Result.Success)
        {
            throw new Exception("Failed to create logical device");
        }

        _vk.GetDeviceQueue(_device, indices.GraphicsFamily!.Value, 0, out _graphicsQueue);
        _vk.GetDeviceQueue(_device, indices.PresentFamily!.Value, 0, out _presentQueue);

        if (_enableValidationLayers)
        {
            _ = SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
        }

        _ = SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);
    }

    private void CreateSurface()
    {
        if (!_vk.TryGetInstanceExtension(_instance, out _khrSurface))
        {
            throw new NotSupportedException("KHR_surface extension not found");
        }

        _surface = _vulkanWindow.VkSurface.Create<AllocationCallbacks>(_instance.ToHandle(), null).ToSurface();
    }

    private void PickPhysicalDevice()
    {
        uint deviceCount = 0;
        _ = _vk.EnumeratePhysicalDevices(_instance, ref deviceCount, null);
        if (deviceCount == 0)
        {
            throw new Exception("Failed to find Vulkan-supported GPUs");
        }

        var devices = new PhysicalDevice[deviceCount];
        fixed (PhysicalDevice* pDevice = devices)
        {
            _ = _vk.EnumeratePhysicalDevices(_instance, ref deviceCount, pDevice);
        }

        foreach (var device in devices)
        {
            if (IsDeviceSuitable(device))
            {
                _physicalDevice = device;
                _sampleCountFlags = GetMaxSampleCount();
                break;
            }
        }

        if (_physicalDevice.Handle == 0)
        {
            throw new Exception("Failed to find a suitable GPU");
        }
    }

    private void SetUpDebugMessenger()
    {
        if (!_enableValidationLayers)
        {
            return;
        }

        if (!_vk.TryGetInstanceExtension(_instance, out _extDebugUtils))
        {
            return;
        }

        DebugUtilsMessengerCreateInfoEXT createInfo = new();
        PopulateDebugMessengerCreateInfo(ref createInfo);

        if (_extDebugUtils.CreateDebugUtilsMessenger(_instance, in createInfo, null, out _debugMessenger) != Result.Success)
        {
            throw new Exception("Failed to set up debug messenger");
        }
    }

    //
    // Other methods
    //

    private bool CheckDeviceExtensionsSupport(PhysicalDevice physicalDevice)
    {
        uint extensionsCount = 0;
        _ = _vk.EnumerateDeviceExtensionProperties(physicalDevice, (byte*)null, ref extensionsCount, null);

        var availableExtensions = new ExtensionProperties[extensionsCount];
        fixed (ExtensionProperties* pAvailableExtensions = availableExtensions)
        {
            _ = _vk.EnumerateDeviceExtensionProperties(physicalDevice, (byte*)null, ref extensionsCount, pAvailableExtensions);
        }

        var availableExtensionNames = availableExtensions.Select(ext => Marshal.PtrToStringAnsi((IntPtr)ext.ExtensionName)).ToHashSet();

        return _deviceExtensions.All(availableExtensionNames.Contains);
    }

    public Format FindDepthFormat()
    {
        return FindSupportedFormat(new[]
        {
            Format.D32Sfloat,
            Format.D32SfloatS8Uint,
            Format.D24UnormS8Uint
        }, ImageTiling.Optimal, FormatFeatureFlags.DepthStencilAttachmentBit);
    }

    public uint FindMemoryType(uint typeFilter, MemoryPropertyFlags properties)
    {
        _vk.GetPhysicalDeviceMemoryProperties(_physicalDevice, out var memProperties);

        for (int i = 0; i < memProperties.MemoryTypeCount; i++)
        {
            if ((typeFilter & (1 << i)) != 0 && (memProperties.MemoryTypes[i].PropertyFlags & properties) == properties)
            {
                return (uint)i;
            }
        }

        throw new Exception("Failed to find suitable memory type");
    }

    public QueueFamilyIndices FindQueueFamilies() => FindQueueFamilies(_physicalDevice);

    private QueueFamilyIndices FindQueueFamilies(PhysicalDevice physicalDevice)
    {
        var indices = new QueueFamilyIndices();

        uint queueFamilyCount = 0;
        _vk.GetPhysicalDeviceQueueFamilyProperties(physicalDevice, ref queueFamilyCount, null);

        var queueFamilies = new QueueFamilyProperties[queueFamilyCount];
        fixed (QueueFamilyProperties* pQueueFamilies = queueFamilies)
        {
            _vk.GetPhysicalDeviceQueueFamilyProperties(physicalDevice, ref queueFamilyCount, pQueueFamilies);
        }

        uint i = 0;
        foreach (var queueFamily in queueFamilies)
        {
            if (queueFamily.QueueFlags.HasFlag(QueueFlags.GraphicsBit))
            {
                indices.GraphicsFamily = i;
            }

            _ = _khrSurface.GetPhysicalDeviceSurfaceSupport(physicalDevice, i, _surface, out var presentSupport);

            if (presentSupport)
            {
                indices.PresentFamily = i;
            }

            if (indices.IsComplete)
            {
                break;
            }

            i++;
        }

        return indices;
    }

    private Format FindSupportedFormat(IEnumerable<Format> candidates, ImageTiling tiling, FormatFeatureFlags features)
    {
        foreach (var format in candidates)
        {
            _vk.GetPhysicalDeviceFormatProperties(_physicalDevice, format, out var properties);

            if (tiling == ImageTiling.Linear && (properties.LinearTilingFeatures & features) == features)
            {
                return format;
            }
            else if (tiling == ImageTiling.Optimal && (properties.OptimalTilingFeatures & features) == features)
            {
                return format;
            }
        }

        throw new Exception("Failed to find supported format");
    }

    private DebugUtilsMessengerCallbackFunctionEXT GetCallbackFunction()
    {
        return (messageSeverity, messageTypes, pCallbackData, pUserData) =>
        {
            if (messageSeverity == DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt)
            {
                return Vk.False;
            }

            var msg = Marshal.PtrToStringAnsi((nint)pCallbackData->PMessage);
            Debug.WriteLine($"{messageSeverity} | validation layer: {msg}");

            return Vk.False;
        };
    }

    private SampleCountFlags GetMaxSampleCount()
    {
        _vk.GetPhysicalDeviceProperties(_physicalDevice, out var physicalDeviceProperties);
        var counts = physicalDeviceProperties.Limits.FramebufferColorSampleCounts & physicalDeviceProperties.Limits.FramebufferDepthSampleCounts;

        return counts switch
        {
            var c when (c & SampleCountFlags.Count64Bit) != 0 => SampleCountFlags.Count64Bit,
            var c when (c & SampleCountFlags.Count32Bit) != 0 => SampleCountFlags.Count32Bit,
            var c when (c & SampleCountFlags.Count16Bit) != 0 => SampleCountFlags.Count16Bit,
            var c when (c & SampleCountFlags.Count8Bit) != 0 => SampleCountFlags.Count8Bit,
            var c when (c & SampleCountFlags.Count4Bit) != 0 => SampleCountFlags.Count4Bit,
            var c when (c & SampleCountFlags.Count2Bit) != 0 => SampleCountFlags.Count2Bit,
            _ => SampleCountFlags.Count1Bit
        };
    }

    private string[] GetRequiredExtensions()
    {
        var glfwExtensions = _vulkanWindow.VkSurface.GetRequiredExtensions(out var glfwExtensionCount);
        var extensions = SilkMarshal.PtrToStringArray((nint)glfwExtensions, (int)glfwExtensionCount);

        if (_enableValidationLayers)
        {
            return extensions.Append(ExtDebugUtils.ExtensionName).ToArray();
        }

        return extensions;
    }

    private static string GetString(byte* pString)
    {
        int characters = 0;
        while (pString[characters] != 0)
        {
            characters++;
        }
        return Encoding.UTF8.GetString(pString, characters);
    }

    private bool IsDeviceSuitable(PhysicalDevice physicalDevice)
    {
        var indices = FindQueueFamilies(physicalDevice);

        var extensionsSupported = CheckDeviceExtensionsSupport(physicalDevice);

        var swapChainAdequate = false;
        if (extensionsSupported)
        {
            var swapChainSupport = QuerySwapChainSupport(physicalDevice);
            swapChainAdequate = swapChainSupport.Formats.Any() && swapChainSupport.PresentModes.Any();
        }

        _vk.GetPhysicalDeviceFeatures(physicalDevice, out var supportedFeatures);

        return indices.IsComplete && extensionsSupported && swapChainAdequate && supportedFeatures.SamplerAnisotropy;
    }

    private void PopulateDebugMessengerCreateInfo(ref DebugUtilsMessengerCreateInfoEXT createInfo)
    {
        createInfo.SType = StructureType.DebugUtilsMessengerCreateInfoExt;
        createInfo.MessageSeverity =
            DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt |
            DebugUtilsMessageSeverityFlagsEXT.WarningBitExt |
            DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt;
        createInfo.MessageType =
            DebugUtilsMessageTypeFlagsEXT.GeneralBitExt |
            DebugUtilsMessageTypeFlagsEXT.PerformanceBitExt |
            DebugUtilsMessageTypeFlagsEXT.ValidationBitExt;
        createInfo.PfnUserCallback = GetCallbackFunction();
    }

    public SwapChainSupportDetails QuerySwapChainSupport() => QuerySwapChainSupport(_physicalDevice);

    private SwapChainSupportDetails QuerySwapChainSupport(PhysicalDevice physicalDevice)
    {
        SwapChainSupportDetails details = new();

        _ = _khrSurface.GetPhysicalDeviceSurfaceCapabilities(physicalDevice, _surface, out details.Capabilities);

        uint formalCount = 0;
        _ = _khrSurface.GetPhysicalDeviceSurfaceFormats(physicalDevice, _surface, ref formalCount, null);
        if (formalCount != 0)
        {
            details.Formats = new SurfaceFormatKHR[formalCount];
            fixed (SurfaceFormatKHR* pFormats = details.Formats)
            {
                _ = _khrSurface.GetPhysicalDeviceSurfaceFormats(physicalDevice, _surface, ref formalCount, pFormats);
            }
        }
        else
        {
            details.Formats = Array.Empty<SurfaceFormatKHR>();
        }

        uint presentModeCount = 0;
        _ = _khrSurface.GetPhysicalDeviceSurfacePresentModes(physicalDevice, _surface, ref presentModeCount, null);
        if (presentModeCount != 0)
        {
            details.PresentModes = new PresentModeKHR[presentModeCount];
            fixed (PresentModeKHR* pPresentModes = details.PresentModes)
            {
                _ = _khrSurface.GetPhysicalDeviceSurfacePresentModes(physicalDevice, _surface, ref presentModeCount, pPresentModes);
            }
        }
        else
        {
            details.PresentModes = Array.Empty<PresentModeKHR>();
        }

        return details;
    }

    private bool ValidationLayersSupported()
    {
        uint propCount = 0;
        var result = _vk.EnumerateInstanceLayerProperties(ref propCount, null);
        if (propCount == 0)
        {
            return false;
        }

        var hasLayer = false;
        using var memory = GlobalMemory.Allocate((int)propCount * sizeof(LayerProperties));
        var properties = (LayerProperties*)Unsafe.AsPointer(ref memory.GetPinnableReference());
        _ = _vk.EnumerateInstanceLayerProperties(ref propCount, properties);

        for (int i = 0; i < propCount; i++)
        {
            var layerName = GetString(properties[i].LayerName);
            if (layerName == _validationLayers[0])
            {
                hasLayer = true;
            }
        }
        return hasLayer;
    }
}
