// <copyright file="PipelineConfigInfo.cs" company="Mathematics.NET">
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

using System.Runtime.CompilerServices;

namespace Mathematics.NET.Renderer;

public struct PipelineConfigInfo
{
    public PipelineInputAssemblyStateCreateInfo InputAssemblyInfo;
    public PipelineViewportStateCreateInfo ViewportInfo;
    public PipelineRasterizationStateCreateInfo RasterizationInfo;
    public PipelineMultisampleStateCreateInfo MultisampleInfo;
    public PipelineColorBlendAttachmentState ColorBlendAttachment;
    public PipelineColorBlendStateCreateInfo ColorBlendInfo;
    public PipelineDepthStencilStateCreateInfo DepthStencilInfo;

    public DynamicState[] DynamicStateEnables = null!;
    public PipelineDynamicStateCreateInfo DynamicStateInfo;

    public PipelineLayout PipelineLayout;
    public RenderPass RenderPass;
    public uint Subpass;

    public PipelineConfigInfo()
    {
        Subpass = 0;
    }

    public static unsafe void DefaultPipelineConfigInfo(ref PipelineConfigInfo configInfo)
    {
        configInfo.InputAssemblyInfo = new()
        {
            SType = StructureType.PipelineInputAssemblyStateCreateInfo,
            Topology = PrimitiveTopology.TriangleList,
            PrimitiveRestartEnable = false
        };

        configInfo.ViewportInfo = new()
        {
            SType = StructureType.PipelineViewportStateCreateInfo,
            ViewportCount = 1,
            PViewports = default,
            ScissorCount = 1,
            PScissors = default
        };

        configInfo.RasterizationInfo = new()
        {
            SType = StructureType.PipelineRasterizationStateCreateInfo,
            DepthClampEnable = false,
            RasterizerDiscardEnable = false,
            PolygonMode = PolygonMode.Fill,
            LineWidth = 1f,
            CullMode = CullModeFlags.None,
            FrontFace = FrontFace.CounterClockwise,
            DepthBiasEnable = false,
            DepthBiasConstantFactor = 0f,
            DepthBiasClamp = 0f,
            DepthBiasSlopeFactor = 0f
        };

        configInfo.MultisampleInfo = new()
        {
            SType = StructureType.PipelineMultisampleStateCreateInfo,
            SampleShadingEnable = false,
            RasterizationSamples = SampleCountFlags.Count1Bit,
            MinSampleShading = 1.0f,
            PSampleMask = null,
            AlphaToCoverageEnable = false,
            AlphaToOneEnable = false
        };

        configInfo.ColorBlendAttachment = new()
        {
            ColorWriteMask = ColorComponentFlags.RBit | ColorComponentFlags.GBit | ColorComponentFlags.BBit | ColorComponentFlags.ABit,
            BlendEnable = false,
            SrcColorBlendFactor = BlendFactor.One,
            DstColorBlendFactor = BlendFactor.Zero,
            ColorBlendOp = BlendOp.Add,
            SrcAlphaBlendFactor = BlendFactor.One,
            DstAlphaBlendFactor = BlendFactor.Zero,
            AlphaBlendOp = BlendOp.Add
        };

        configInfo.ColorBlendInfo = new()
        {
            SType = StructureType.PipelineColorBlendStateCreateInfo,
            LogicOpEnable = false,
            LogicOp = LogicOp.Copy,
            AttachmentCount = 1,
            PAttachments = (PipelineColorBlendAttachmentState*)Unsafe.AsPointer(ref configInfo.ColorBlendAttachment)
        };

        configInfo.ColorBlendInfo.BlendConstants[0] = 0;
        configInfo.ColorBlendInfo.BlendConstants[1] = 0;
        configInfo.ColorBlendInfo.BlendConstants[2] = 0;
        configInfo.ColorBlendInfo.BlendConstants[3] = 0;

        configInfo.DepthStencilInfo = new()
        {
            SType = StructureType.PipelineDepthStencilStateCreateInfo,
            DepthTestEnable = true,
            DepthWriteEnable = true,
            DepthCompareOp = CompareOp.Less,
            DepthBoundsTestEnable = false,
            MinDepthBounds = 0.0f,
            MaxDepthBounds = 1.0f,
            StencilTestEnable = false,
            Front = default,
            Back = default
        };
    }
}
