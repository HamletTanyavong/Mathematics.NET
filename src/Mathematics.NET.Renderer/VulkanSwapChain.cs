// <copyright file="VulkanSwapChain.cs" company="Mathematics.NET">
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
using Silk.NET.Vulkan.Extensions.KHR;

namespace Mathematics.NET.Renderer;

public class VulkanSwapChain : IDisposable
{
    private const int s_maxFramesInFlight = 2;

    private readonly Vk _vk = null!;
    private readonly VulkanDevice _vulkanDevice = null!;
    private readonly Device _device;

    private KhrSwapchain _khrSwapChain = null!;
    private SwapchainKHR _swapChain;
    private Image[] _swapChainImages = null!;
    private Format _swapChainImageFormat;
    private Extent2D _extent;
    private ImageView[] _swapChainImageViews = null!;

    private Framebuffer[] _swapChainFramebuffers = null!;

    private RenderPass _renderPass;

    private Image[] _depthImages = null!;
    private DeviceMemory[] _depthImageMemoryArray = null!;
    private ImageView[] _depthImageViews = null!;

    private Extent2D _windowExtent;

    private Semaphore[] _imageAvailableSemaphores = null!;
    private Semaphore[] _renderFinishedSemaphores = null!;
    private Fence[] _inFlightFences = null!;
    private Fence[] _imagesInFlight = null!;
    private int _currentFrame = 0;

    private VulkanSwapChain? _previousVulkanSwapChain;

    private bool _disposed;

    public VulkanSwapChain(Vk vk, VulkanDevice vulkanDevice, Extent2D extent2D)
    {
        _vk = vk;
        _vulkanDevice = vulkanDevice;
        _device = _vulkanDevice.Device;
        _windowExtent = extent2D;

        Initialize();
    }

    public VulkanSwapChain(Vk vk, VulkanDevice vulkanDevice, Extent2D extent, VulkanSwapChain previousVulkanSwapChain)
    {
        _vk = vk;
        _vulkanDevice = vulkanDevice;
        _device = _vulkanDevice.Device;
        _windowExtent = extent;

        _previousVulkanSwapChain = previousVulkanSwapChain;
        Initialize();
        previousVulkanSwapChain.Dispose();

        Debug.Assert(previousVulkanSwapChain._disposed, "Old swap chain was not disposed");
    }

    ~VulkanSwapChain()
    {
        Dispose(false);
    }

    public Extent2D Extent => _extent;

    public RenderPass RenderPass => _renderPass;

    public uint ImageCount => (uint)_swapChainImageViews.Length;

    //
    // IDisposable interface
    //

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // TODO: Verify proper disposal of resources
    protected virtual unsafe void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _khrSwapChain.Dispose();
            }

            // Order is important

            foreach (var imageView in _swapChainImageViews)
            {
                _vk.DestroyImageView(_device, imageView, null);
            }
            Array.Clear(_swapChainImageViews);

            _khrSwapChain.DestroySwapchain(_device, _swapChain, null);
            _swapChain = default;

            for (int i = 0; i < _depthImages.Length; i++)
            {
                _vk.DestroyImageView(_device, _depthImageViews[i], null);
                _vk.DestroyImage(_device, _depthImages[i], null);
                _vk.FreeMemory(_device, _depthImageMemoryArray[i], null);
            }
            Array.Clear(_depthImageViews);
            Array.Clear(_depthImages);

            foreach (var framebuffer in _swapChainFramebuffers)
            {
                _vk.DestroyFramebuffer(_device, framebuffer, null);
            }
            Array.Clear(_swapChainFramebuffers);

            _vk.DestroyRenderPass(_device, _renderPass, null);

            for (int i = 0; i < s_maxFramesInFlight; i++)
            {
                _vk.DestroySemaphore(_device, _renderFinishedSemaphores[i], null);
                _vk.DestroySemaphore(_device, _imageAvailableSemaphores[i], null);
                _vk.DestroyFence(_device, _inFlightFences[i], null);
            }

            _previousVulkanSwapChain = null;

            _disposed = true;
        }
    }

    //
    // Constructor methods
    //

    private void CreateImageViews()
    {
        _swapChainImageViews = new ImageView[_swapChainImages.Length];
        for (int i = 0; i < _swapChainImages.Length; i++)
        {
            _swapChainImageViews[i] = CreateImageView(_swapChainImages[i], _swapChainImageFormat, ImageAspectFlags.ColorBit, 1);
        }
    }

    private unsafe void CreateDepthResources()
    {
        Format depthFormat = _vulkanDevice.FindDepthFormat();

        var imageCount = ImageCount;
        _depthImages = new Image[imageCount];
        _depthImageMemoryArray = new DeviceMemory[imageCount];
        _depthImageViews = new ImageView[imageCount];

        for (int i = 0; i < imageCount; i++)
        {
            ImageCreateInfo imageInfo = new()
            {
                SType = StructureType.ImageCreateInfo,
                ImageType = ImageType.Type2D,
                Extent =
                {
                    Width = _extent.Width,
                    Height = _extent.Height,
                    Depth = 1,
                },
                MipLevels = 1,
                ArrayLayers = 1,
                Format = depthFormat,
                Tiling = ImageTiling.Optimal,
                InitialLayout = ImageLayout.Undefined,
                Usage = ImageUsageFlags.DepthStencilAttachmentBit,
                Samples = SampleCountFlags.Count1Bit,
                SharingMode = SharingMode.Exclusive,
                Flags = 0
            };

            fixed (Image* pImage = &_depthImages[i])
            {
                if (_vk.CreateImage(_device, imageInfo, null, pImage) != Result.Success)
                {
                    throw new Exception("Failed to create depth image");
                }
            }

            _vk.GetImageMemoryRequirements(_device, _depthImages[i], out var memRequirements);

            MemoryAllocateInfo allocInfo = new()
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = memRequirements.Size,
                MemoryTypeIndex = _vulkanDevice.FindMemoryType(memRequirements.MemoryTypeBits, MemoryPropertyFlags.DeviceLocalBit),
            };

            fixed (DeviceMemory* pImageMemory = &_depthImageMemoryArray[i])
            {
                if (_vk.AllocateMemory(_device, allocInfo, null, pImageMemory) != Result.Success)
                {
                    throw new Exception("failed to allocate depth image memory!");
                }
            }

            _ = _vk.BindImageMemory(_device, _depthImages[i], _depthImageMemoryArray[i], 0);

            ImageViewCreateInfo createInfo = new()
            {
                SType = StructureType.ImageViewCreateInfo,
                Image = _depthImages[i],
                ViewType = ImageViewType.Type2D,
                Format = depthFormat,
                SubresourceRange =
                {
                    AspectMask = ImageAspectFlags.DepthBit,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                }
            };

            if (_vk.CreateImageView(_device, createInfo, null, out _depthImageViews[i]) != Result.Success)
            {
                throw new Exception("Failed to create depth image views");
            }
        }
    }

    private unsafe void CreateFrameBuffers()
    {
        _swapChainFramebuffers = new Framebuffer[_swapChainImageViews.Length];

        for (int i = 0; i < _swapChainImageViews.Length; i++)
        {
            var attachments = new[] { _swapChainImageViews[i], _depthImageViews[i] };

            fixed (ImageView* pAttachments = attachments)
            {
                FramebufferCreateInfo framebufferInfo = new()
                {
                    SType = StructureType.FramebufferCreateInfo,
                    RenderPass = _renderPass,
                    AttachmentCount = (uint)attachments.Length,
                    PAttachments = pAttachments,
                    Width = _extent.Width,
                    Height = _extent.Height,
                    Layers = 1,
                };

                if (_vk!.CreateFramebuffer(_vulkanDevice.Device, framebufferInfo, null, out _swapChainFramebuffers[i]) != Result.Success)
                {
                    throw new Exception("Failed to create framebuffer");
                }
            }
        }
    }

    private unsafe void CreateRenderPass()
    {
        AttachmentDescription depthAttachment = new()
        {
            Format = _vulkanDevice.FindDepthFormat(),
            Samples = SampleCountFlags.Count1Bit,
            LoadOp = AttachmentLoadOp.Clear,
            StoreOp = AttachmentStoreOp.DontCare,
            StencilLoadOp = AttachmentLoadOp.DontCare,
            StencilStoreOp = AttachmentStoreOp.DontCare,
            InitialLayout = ImageLayout.Undefined,
            FinalLayout = ImageLayout.DepthStencilAttachmentOptimal,
        };

        AttachmentReference depthAttachmentRef = new()
        {
            Attachment = 1,
            Layout = ImageLayout.DepthStencilAttachmentOptimal,
        };

        AttachmentDescription colorAttachment = new()
        {
            Format = _swapChainImageFormat,
            Samples = SampleCountFlags.Count1Bit,
            LoadOp = AttachmentLoadOp.Clear,
            StoreOp = AttachmentStoreOp.Store,
            StencilLoadOp = AttachmentLoadOp.DontCare,
            StencilStoreOp = AttachmentStoreOp.DontCare,
            InitialLayout = ImageLayout.Undefined,
            FinalLayout = ImageLayout.PresentSrcKhr,
        };

        AttachmentReference colorAttachmentRef = new()
        {
            Attachment = 0,
            Layout = ImageLayout.ColorAttachmentOptimal,
        };

        SubpassDescription subpass = new()
        {
            PipelineBindPoint = PipelineBindPoint.Graphics,
            ColorAttachmentCount = 1,
            PColorAttachments = &colorAttachmentRef,
            PDepthStencilAttachment = &depthAttachmentRef,
        };

        SubpassDependency dependency = new()
        {
            SrcSubpass = Vk.SubpassExternal,
            DstSubpass = 0,
            SrcStageMask = PipelineStageFlags.ColorAttachmentOutputBit | PipelineStageFlags.EarlyFragmentTestsBit,
            SrcAccessMask = 0,
            DstStageMask = PipelineStageFlags.ColorAttachmentOutputBit | PipelineStageFlags.EarlyFragmentTestsBit,
            DstAccessMask = AccessFlags.ColorAttachmentWriteBit | AccessFlags.DepthStencilAttachmentWriteBit
        };

        var attachments = new[] { colorAttachment, depthAttachment };

        fixed (AttachmentDescription* pAttachments = attachments)
        {
            RenderPassCreateInfo createInfo = new()
            {
                SType = StructureType.RenderPassCreateInfo,
                AttachmentCount = (uint)attachments.Length,
                PAttachments = pAttachments,
                SubpassCount = 1,
                PSubpasses = &subpass,
                DependencyCount = 1,
                PDependencies = &dependency,
            };

            if (_vk.CreateRenderPass(_device, createInfo, null, out _renderPass) != Result.Success)
            {
                throw new Exception("Failed to create render pass");
            }
        }
    }

    private unsafe void CreateSwapChain()
    {
        var swapChainSupport = _vulkanDevice.QuerySwapChainSupport();

        var surfaceFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);
        var presentMode = ChoosePresentMode(swapChainSupport.PresentModes);
        var extent = ChooseSwapExtent(swapChainSupport.Capabilities);

        var imageCount = swapChainSupport.Capabilities.MaxImageCount;

        SwapchainCreateInfoKHR createInfo = new()
        {
            SType = StructureType.SwapchainCreateInfoKhr,
            Surface = _vulkanDevice.Surface,

            MinImageCount = imageCount,
            ImageFormat = surfaceFormat.Format,
            ImageColorSpace = surfaceFormat.ColorSpace,
            ImageExtent = extent,
            ImageArrayLayers = 1,
            ImageUsage = ImageUsageFlags.ColorAttachmentBit
        };

        var indices = _vulkanDevice.FindQueueFamilies();
        var queueFamilyIndices = stackalloc[] { indices.GraphicsFamily!.Value, indices.PresentFamily!.Value };

        if (indices.GraphicsFamily != indices.PresentFamily)
        {
            createInfo = createInfo with
            {
                ImageSharingMode = SharingMode.Concurrent,
                QueueFamilyIndexCount = 2,
                PQueueFamilyIndices = queueFamilyIndices
            };
        }
        else
        {
            createInfo.ImageSharingMode = SharingMode.Exclusive;
        }

        createInfo = createInfo with
        {
            PreTransform = swapChainSupport.Capabilities.CurrentTransform,
            CompositeAlpha = CompositeAlphaFlagsKHR.OpaqueBitKhr,
            PresentMode = presentMode,
            Clipped = true
        };

        if (_khrSwapChain is null)
        {
            if (!_vk.TryGetDeviceExtension(_vulkanDevice.Instance, _device, out _khrSwapChain))
            {
                throw new NotSupportedException("VK_KHR_swapchain extension not found");
            }
        }

        createInfo.OldSwapchain = _previousVulkanSwapChain is null ? default : _previousVulkanSwapChain._swapChain;

        if (_khrSwapChain.CreateSwapchain(_device, createInfo, null, out _swapChain) != Result.Success)
        {
            throw new Exception("Failed to create swap chain");
        }

        _ = _khrSwapChain.GetSwapchainImages(_device, _swapChain, ref imageCount, null);
        _swapChainImages = new Image[imageCount];
        fixed (Image* pSwapChainImages = _swapChainImages)
        {
            _ = _khrSwapChain.GetSwapchainImages(_device, _swapChain, ref imageCount, pSwapChainImages);
        }

        _swapChainImageFormat = surfaceFormat.Format;
        _extent = extent;
    }

    private unsafe void CreateSyncObjects()
    {
        _imageAvailableSemaphores = new Semaphore[s_maxFramesInFlight];
        _renderFinishedSemaphores = new Semaphore[s_maxFramesInFlight];
        _inFlightFences = new Fence[s_maxFramesInFlight];
        _imagesInFlight = new Fence[_swapChainImages!.Length];

        SemaphoreCreateInfo semaphoreInfo = new()
        {
            SType = StructureType.SemaphoreCreateInfo,
        };

        FenceCreateInfo fenceInfo = new()
        {
            SType = StructureType.FenceCreateInfo,
            Flags = FenceCreateFlags.SignaledBit,
        };

        for (var i = 0; i < s_maxFramesInFlight; i++)
        {
            if (_vk.CreateSemaphore(_device, semaphoreInfo, null, out _imageAvailableSemaphores[i]) != Result.Success ||
                _vk.CreateSemaphore(_device, semaphoreInfo, null, out _renderFinishedSemaphores[i]) != Result.Success ||
                _vk.CreateFence(_device, fenceInfo, null, out _inFlightFences[i]) != Result.Success)
            {
                throw new Exception("failed to create synchronization objects for a frame!");
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize()
    {
        CreateSwapChain();
        CreateImageViews();
        CreateRenderPass();
        CreateDepthResources();
        CreateFrameBuffers();
        CreateSyncObjects();
    }

    //
    // Other methods
    //

    public unsafe Result AcquireNextImage(uint imageIndex)
    {
        var fence = _inFlightFences[_currentFrame];
        _ = _vk.WaitForFences(_vulkanDevice.Device, 1, in fence, Vk.True, ulong.MaxValue);

        Result result = _khrSwapChain.AcquireNextImage(_vulkanDevice.Device, _swapChain, ulong.MaxValue, _imageAvailableSemaphores[_currentFrame], default, &imageIndex);

        return result;
    }

    private static PresentModeKHR ChoosePresentMode(IReadOnlyList<PresentModeKHR> availablePresentModes)
    {
        foreach (var availablePresentMode in availablePresentModes)
        {
            if (availablePresentMode == PresentModeKHR.MailboxKhr)
            {
                Debug.WriteLine($"Using present mode: {availablePresentMode}");
                return availablePresentMode;
            }
        }
        Debug.WriteLine($"Using fallback present mode: {PresentModeKHR.FifoKhr}");
        return PresentModeKHR.FifoKhr;
    }

    private Extent2D ChooseSwapExtent(SurfaceCapabilitiesKHR capabilities)
    {
        if (capabilities.CurrentExtent.Width != uint.MaxValue)
        {
            return capabilities.CurrentExtent;
        }
        else
        {
            var framebufferSize = _windowExtent;

            Extent2D actualExtent = new()
            {
                Width = framebufferSize.Width,
                Height = framebufferSize.Height
            };

            actualExtent.Width = Math.Clamp(actualExtent.Width, capabilities.MinImageExtent.Width, capabilities.MaxImageExtent.Width);
            actualExtent.Height = Math.Clamp(actualExtent.Height, capabilities.MinImageExtent.Height, capabilities.MaxImageExtent.Height);

            return actualExtent;
        }
    }

    private static SurfaceFormatKHR ChooseSwapSurfaceFormat(IReadOnlyList<SurfaceFormatKHR> availableFormats)
    {
        foreach (var availableFormat in availableFormats)
        {
            if (availableFormat.Format == Format.B8G8R8A8Srgb && availableFormat.ColorSpace == ColorSpaceKHR.SpaceSrgbNonlinearKhr)
            {
                return availableFormat;
            }
        }
        return availableFormats[0];
    }

    private unsafe ImageView CreateImageView(Image image, Format format, ImageAspectFlags aspectFlags, uint mipLevels)
    {
        ImageViewCreateInfo createInfo = new()
        {
            SType = StructureType.ImageViewCreateInfo,
            Image = image,
            ViewType = ImageViewType.Type2D,
            Format = format,
            SubresourceRange =
            {
                AspectMask = aspectFlags,
                BaseMipLevel = 0,
                LevelCount = mipLevels,
                BaseArrayLayer = 0,
                LayerCount = 1
            }
        };

        if (_vk.CreateImageView(_device, createInfo, null, out var imageView) != Result.Success)
        {
            throw new Exception("Failed to create image views");
        }

        return imageView;
    }

    public Framebuffer GetFramebufferAt(uint i) => _swapChainFramebuffers[i];

    public unsafe Result SubmitCommandBuffers(CommandBuffer commandBuffer, uint imageIndex)
    {
        if (_imagesInFlight[imageIndex].Handle != 0)
        {
            _ = _vk.WaitForFences(_vulkanDevice.Device, 1, in _imagesInFlight[imageIndex], Vk.True, ulong.MaxValue);
        }

        _imagesInFlight[imageIndex] = _inFlightFences[_currentFrame];

        Result result = Result.NotReady;

        var submitInfo = new SubmitInfo { SType = StructureType.SubmitInfo };
        Semaphore[] waitSemaphores = { _imageAvailableSemaphores[_currentFrame] };
        PipelineStageFlags[] waitStages = { PipelineStageFlags.ColorAttachmentOutputBit };
        submitInfo.WaitSemaphoreCount = 1;
        var signalSemaphore = _renderFinishedSemaphores[_currentFrame];
        fixed (Semaphore* pWaitSemaphores = waitSemaphores)
        fixed (PipelineStageFlags* pWaitStages = waitStages)
        {
            submitInfo.PWaitSemaphores = pWaitSemaphores;
            submitInfo.PWaitDstStageMask = pWaitStages;

            submitInfo.CommandBufferCount = 1;
            var buffer = commandBuffer;
            submitInfo.PCommandBuffers = &buffer;

            submitInfo.SignalSemaphoreCount = 1;
            submitInfo.PSignalSemaphores = &signalSemaphore;

            _ = _vk.ResetFences(_vulkanDevice.Device, 1, _inFlightFences[_currentFrame]);

            if (_vk.QueueSubmit(_vulkanDevice.GraphicsQueue, 1, &submitInfo, _inFlightFences[_currentFrame]) != Result.Success)
            {
                throw new Exception("failed to submit draw command buffer!");
            }
        }

        fixed (SwapchainKHR* pSwapChain = &_swapChain)
        {
            PresentInfoKHR presentInfo = new()
            {
                SType = StructureType.PresentInfoKhr,
                WaitSemaphoreCount = 1,
                PWaitSemaphores = &signalSemaphore,
                SwapchainCount = 1,
                PSwapchains = pSwapChain,
                PImageIndices = &imageIndex
            };

            result = _khrSwapChain.QueuePresent(_vulkanDevice.PresentQueue, &presentInfo);
        }

        _currentFrame = (_currentFrame + 1) % s_maxFramesInFlight;
        return result;
    }
}
