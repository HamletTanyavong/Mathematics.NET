// <copyright file="VulkanPipeline.cs" company="Mathematics.NET">
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

using System.Reflection;
using Silk.NET.Core.Native;

namespace Mathematics.NET.Renderer;

public class VulkanPipeline
{
    private readonly Vk _vk = null!;
    private readonly VulkanDevice _vulkanDevice = null!;

    private Pipeline _pipeline;
    private ShaderModule _vertShaderModule;
    private ShaderModule _fragShaderModule;

    public VulkanPipeline(Vk vk, VulkanDevice vulkanDevice, string vertShaderPath, string fragShaderPath, PipelineConfigInfo pipelineConfigInfo)
    {
        _vk = vk;
        _vulkanDevice = vulkanDevice;

        CreateGraphicsPipeline(vertShaderPath, fragShaderPath, pipelineConfigInfo);
    }

    //
    // Constructor methods
    //

    private unsafe void CreateGraphicsPipeline(string vertShaderPath, string fragShaderPath, PipelineConfigInfo pipelineConfigInfo)
    {
        var vertSource = GetShaderBytes(vertShaderPath);
        var fragSource = GetShaderBytes(fragShaderPath);

        _vertShaderModule = CreateShaderModule(vertSource);
        _fragShaderModule = CreateShaderModule(fragSource);

        PipelineShaderStageCreateInfo vertShaderStageInfo = new()
        {
            SType = StructureType.PipelineShaderStageCreateInfo,
            Stage = ShaderStageFlags.VertexBit,
            Module = _vertShaderModule,
            PName = (byte*)SilkMarshal.StringToPtr("main"),
            Flags = PipelineShaderStageCreateFlags.None,
            PNext = null,
            PSpecializationInfo = null
        };

        PipelineShaderStageCreateInfo fragShaderStageInfo = new()
        {
            SType = StructureType.PipelineShaderStageCreateInfo,
            Stage = ShaderStageFlags.FragmentBit,
            Module = _fragShaderModule,
            PName = (byte*)SilkMarshal.StringToPtr("main"),
            Flags = PipelineShaderStageCreateFlags.None,
            PNext = null,
            PSpecializationInfo = null,
        };

        var shaderStages = stackalloc[]
        {
            vertShaderStageInfo,
            fragShaderStageInfo
        };

        var attributeDescriptions = Vertex.GetAttributeDescriptions();
        var bindingDescriptions = Vertex.GetBindingDescriptions();

        fixed (VertexInputAttributeDescription* pAttributeDescriptions = attributeDescriptions)
        fixed (VertexInputBindingDescription* pBindingDescriptions = bindingDescriptions)
        {
            PipelineVertexInputStateCreateInfo vertexInputInfo = new()
            {
                SType = StructureType.PipelineVertexInputStateCreateInfo,
                VertexAttributeDescriptionCount = (uint)attributeDescriptions.Length,
                VertexBindingDescriptionCount = (uint)bindingDescriptions.Length,
                PVertexAttributeDescriptions = pAttributeDescriptions,
                PVertexBindingDescriptions = pBindingDescriptions
            };

            var dynamicStates = stackalloc DynamicState[] { DynamicState.Viewport, DynamicState.Scissor };
            PipelineDynamicStateCreateInfo dynamicState = new()
            {
                SType = StructureType.PipelineDynamicStateCreateInfo,
                PDynamicStates = dynamicStates,
                DynamicStateCount = 2,
                Flags = 0
            };

            GraphicsPipelineCreateInfo pipelineInfo = new()
            {
                SType = StructureType.GraphicsPipelineCreateInfo,
                StageCount = 2,
                PStages = shaderStages,
                PVertexInputState = &vertexInputInfo,
                PInputAssemblyState = &pipelineConfigInfo.InputAssemblyInfo,
                PViewportState = &pipelineConfigInfo.ViewportInfo,
                PRasterizationState = &pipelineConfigInfo.RasterizationInfo,
                PMultisampleState = &pipelineConfigInfo.MultisampleInfo,
                PColorBlendState = &pipelineConfigInfo.ColorBlendInfo,
                PDepthStencilState = &pipelineConfigInfo.DepthStencilInfo,
                PDynamicState = &dynamicState,

                Layout = pipelineConfigInfo.PipelineLayout,
                RenderPass = pipelineConfigInfo.RenderPass,
                Subpass = pipelineConfigInfo.Subpass,

                BasePipelineIndex = -1,
                BasePipelineHandle = default
            };

            if (_vk.CreateGraphicsPipelines(_vulkanDevice.Device, default, 1, pipelineInfo, null, out _pipeline) != Result.Success)
            {
                throw new Exception("Failed to create graphics pipeline");
            }
        }

        _ = SilkMarshal.Free((nint)vertShaderStageInfo.PName);
        _ = SilkMarshal.Free((nint)fragShaderStageInfo.PName);

        _vk.DestroyShaderModule(_vulkanDevice.Device, _fragShaderModule, null);
        _vk.DestroyShaderModule(_vulkanDevice.Device, _vertShaderModule, null);
    }

    //
    // Other methods
    //

    public void Bind(CommandBuffer commandBuffer) => _vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, _pipeline);

    private unsafe ShaderModule CreateShaderModule(Span<byte> code)
    {
        ShaderModuleCreateInfo shaderModuleCreateInfo = new()
        {
            SType = StructureType.ShaderModuleCreateInfo,
            CodeSize = (nuint)code.Length
        };

        ShaderModule shaderModule;

        fixed (byte* pCode = code)
        {
            shaderModuleCreateInfo.PCode = (uint*)pCode;
            if (_vk.CreateShaderModule(_vulkanDevice.Device, shaderModuleCreateInfo, null, out shaderModule) != Result.Success)
            {
                throw new Exception("Unable to create shader module");
            }
        }

        return shaderModule;
    }

    private static Span<byte> GetShaderBytes(string fileName)
    {
        var assembly = Assembly.GetEntryAssembly();

        var resourceName = assembly!
            .GetManifestResourceNames()
            .FirstOrDefault(s => s.EndsWith(fileName))
            ?? throw new ArgumentNullException($$"""
                No shader found with name {{fileName}}
                Don't forget to set glsl file to "Embedded Resource"
                """);

        using var stream = assembly!
            .GetManifestResourceStream(resourceName)
            ?? throw new ApplicationException($$"""
                No shader found with name {{fileName}}
                Don't forget to set glsl file to "Embedded Resource"
                """);

        using var memoryStream = new MemoryStream();
        if (stream is null)
        {
            return Array.Empty<byte>();
        }
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
