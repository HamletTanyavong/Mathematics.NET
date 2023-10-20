// <copyright file="Application.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Renderer;

public sealed class Application : IDisposable
{
    private Vk _vk = null!;

    private VulkanWindow _vulkanWindow;
    private VulkanDevice _vulkanDevice;
    private VulkanSwapChain _vulkanSwapChain = null!;

    private PipelineLayout _pipelineLayout;
    private VulkanPipeline _vulkanPipeline = null!;
    private CommandBuffer[] _commandBuffers = null!;

    private VulkanModel _vulkanModel = null!;

    public Application(string? title = null, int width = 1920, int height = 1080)
    {
        _vk = Vk.GetApi();

        _vulkanWindow = new(title ?? "Mathematics.NET Renderer", width, height);
        _vulkanDevice = new VulkanDevice(_vk, _vulkanWindow);

        LoadModels();
        CreatePipelineLayout();
        RecreateSwapChain();
        CreateCommandBuffers();
    }

    //
    // IDisposable interface
    //

    public void Dispose()
    {
        _vulkanModel.Dispose();
        _vulkanSwapChain.Dispose();
        _vulkanDevice.Dispose();
        _vulkanWindow.Dispose();
        _vk.Dispose();
    }

    //
    // Constructor methods
    //

    private unsafe void CreateCommandBuffers()
    {
        _commandBuffers = new CommandBuffer[_vulkanSwapChain.ImageCount];

        CommandBufferAllocateInfo allocInfo = new()
        {
            SType = StructureType.CommandBufferAllocateInfo,
            Level = CommandBufferLevel.Primary,
            CommandPool = _vulkanDevice.CommandPool,
            CommandBufferCount = (uint)_commandBuffers.Length
        };

        fixed (CommandBuffer* pCommandBuffers = _commandBuffers)
        {
            if (_vk!.AllocateCommandBuffers(_vulkanDevice.Device, allocInfo, pCommandBuffers) != Result.Success)
            {
                throw new Exception("Failed to allocate command buffers");
            }
        }
    }

    private unsafe void CreatePipelineLayout()
    {
        PipelineLayoutCreateInfo createInfo = new()
        {
            SType = StructureType.PipelineLayoutCreateInfo,
            SetLayoutCount = 0,
            PSetLayouts = null,
            PushConstantRangeCount = 0,
            PPushConstantRanges = null,
        };

        if (_vk.CreatePipelineLayout(_vulkanDevice.Device, createInfo, null, out _pipelineLayout) != Result.Success)
        {
            throw new Exception("Failed to create pipeline layout");
        }
    }

    private void LoadModels()
    {
        var vertices = new Vertex[]
        {
            new Vertex(0.0f, -0.5f, 1.0f, 0.0f, 0.0f),
            new Vertex(0.5f, 0.5f, 0.0f, 1.0f, 0.0f),
            new Vertex(-0.5f, 0.5f, 0.0f, 0.0f, 1.0f)
        };

        _vulkanModel = new VulkanModel(_vk, _vulkanDevice, vertices);
    }

    private unsafe void RecreateSwapChain()
    {
        var extent = _vulkanWindow.Extent;
        while (extent.Width == 0 || extent.Height == 0)
        {
            extent = _vulkanWindow.Extent;
            _vulkanWindow.Window.DoEvents();
        }

        _ = _vk.DeviceWaitIdle(_vulkanDevice.Device);

        if (_vulkanSwapChain is null)
        {
            _vulkanSwapChain = new VulkanSwapChain(_vk, _vulkanDevice, extent);
        }
        else
        {
            // We have to make sure the old vulkan swap chain is disposed properly.
            // This happens in the VulkanSwapChain constructor with 4 parameters
            // where Dispose() is called after the new swap chain is created.
            _vulkanSwapChain = new VulkanSwapChain(_vk, _vulkanDevice, extent, _vulkanSwapChain);
            if (_vulkanSwapChain.ImageCount != _commandBuffers.Length)
            {
                FreeCommandBuffers();
                CreateCommandBuffers();
            }
        }

        CreatePipeline();
    }

    //
    // Other methods
    //

    private void CreatePipeline()
    {
        Debug.Assert(_vulkanSwapChain is not null, "Cannot create pipeline before swap chain");

        PipelineConfigInfo pipelineConfig = new();
        PipelineConfigInfo.DefaultPipelineConfigInfo(ref pipelineConfig);

        pipelineConfig.RenderPass = _vulkanSwapChain.RenderPass;
        pipelineConfig.PipelineLayout = _pipelineLayout;
        _vulkanPipeline = new(
            _vk,
            _vulkanDevice,
            "shader.vert.spv",
            "shader.frag.spv",
            pipelineConfig);
    }

    private void DrawFrame(double delta)
    {
        uint imageIndex = 0;
        var result = _vulkanSwapChain.AcquireNextImage(imageIndex);

        if (result == Result.ErrorOutOfDateKhr)
        {
            RecreateSwapChain();
            return;
        }

        if (result != Result.Success && result != Result.SuboptimalKhr)
        {
            throw new Exception("failed to acquire next swap chain image");
        }

        RecordCommandBuffer(imageIndex);
        result = _vulkanSwapChain.SubmitCommandBuffers(_commandBuffers[imageIndex], imageIndex);
        if (result == Result.ErrorOutOfDateKhr || result == Result.SuboptimalKhr || _vulkanWindow.WasWindowResized)
        {
            _vulkanWindow.ResetWindowResizedFlag();
            RecreateSwapChain();
            return;
        }
        else if (result != Result.Success)
        {
            throw new Exception("Failed to submit command buffers");
        }
    }

    private unsafe void FreeCommandBuffers()
    {
        fixed (CommandBuffer* pCommandBuffers = _commandBuffers)
        {
            _vk.FreeCommandBuffers(_vulkanDevice.Device, _vulkanDevice.CommandPool, (uint)_commandBuffers.Length, pCommandBuffers);
        }
        Array.Clear(_commandBuffers);
    }

    private unsafe void RecordCommandBuffer(uint imageIndex)
    {
        CommandBufferBeginInfo beginInfo = new() { SType = StructureType.CommandBufferBeginInfo };
        if (_vk.BeginCommandBuffer(_commandBuffers[imageIndex], beginInfo) != Result.Success)
        {
            throw new Exception("Failed to begin recording command buffer");
        }

        RenderPassBeginInfo renderPassBeginInfo = new()
        {
            SType = StructureType.RenderPassBeginInfo,
            RenderPass = _vulkanSwapChain.RenderPass,
            Framebuffer = _vulkanSwapChain.GetFramebufferAt(imageIndex),
            RenderArea =
            {
                Offset = { X = 0, Y = 0 },
                Extent = _vulkanSwapChain.Extent
            }
        };

        var clearValues = new ClearValue[]
        {
            new()
            {
                Color = new(float32_0: 0.0086f, float32_1: 0.0086f, float32_2: 0.0086f, float32_3: 1.0f),
            },
            new()
            {
                DepthStencil = new () { Depth = 1, Stencil = 0 }
            }
        };

        fixed (ClearValue* clearValuesPtr = clearValues)
        {
            renderPassBeginInfo.ClearValueCount = (uint)clearValues.Length;
            renderPassBeginInfo.PClearValues = clearValuesPtr;

            _vk.CmdBeginRenderPass(_commandBuffers[imageIndex], &renderPassBeginInfo, SubpassContents.Inline);
        }

        Viewport viewport = new()
        {
            X = 0.0f,
            Y = 0.0f,
            Width = _vulkanSwapChain.Extent.Width,
            Height = _vulkanSwapChain.Extent.Height,
            MinDepth = 0.0f,
            MaxDepth = 1.0f,
        };
        Rect2D scissor = new(new(), _vulkanSwapChain.Extent);
        _vk.CmdSetViewport(_commandBuffers[imageIndex], 0, 1, &viewport);
        _vk.CmdSetScissor(_commandBuffers[imageIndex], 0, 1, &scissor);

        _vulkanPipeline.Bind(_commandBuffers[imageIndex]);
        _vulkanModel.Bind(_commandBuffers[imageIndex]);
        _vulkanModel.Draw(_commandBuffers[imageIndex]);

        _vk.CmdEndRenderPass(_commandBuffers[imageIndex]);

        if (_vk.EndCommandBuffer(_commandBuffers[imageIndex]) != Result.Success)
        {
            throw new Exception("Failed to record command buffer");
        }
    }

    public void Run()
    {
        _vulkanWindow.Window.Render += DrawFrame;
        _vulkanWindow.Run();

        _ = _vk.DeviceWaitIdle(_vulkanDevice.Device);
    }
}
