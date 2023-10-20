// <copyright file="VulkanModel.cs" company="Mathematics.NET">
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

namespace Mathematics.NET.Renderer;

public class VulkanModel : IDisposable
{
    private readonly Vk _vk = null!;
    private readonly VulkanDevice _vulkanDevice = null!;

    private Buffer _vertexBuffer;
    private DeviceMemory _vertexBufferMemory;
    private uint _vertexCount;

    private bool _disposed;

    public VulkanModel(Vk vk, VulkanDevice device, Vertex[] vertices)
    {
        _vk = vk;
        _vulkanDevice = device;

        _vertexCount = (uint)vertices.Length;
        CreateVertexBuffers(vertices);
    }

    //
    // IDisposable interface
    //

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual unsafe void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) { }

            _vk.DestroyBuffer(_vulkanDevice.Device, _vertexBuffer, null);
            _vk.FreeMemory(_vulkanDevice.Device, _vertexBufferMemory, null);

            _disposed = true;
        }
    }

    //
    // Constructor methods
    //

    private unsafe void CreateVertexBuffers(Vertex[] vertices)
    {
        BufferCreateInfo bufferInfo = new()
        {
            SType = StructureType.BufferCreateInfo,
            Size = (ulong)(sizeof(Vertex) * vertices.Length),
            Usage = BufferUsageFlags.VertexBufferBit,
            SharingMode = SharingMode.Exclusive,
        };

        fixed (Buffer* pVertexBuffer = &_vertexBuffer)
        {
            if (_vk.CreateBuffer(_vulkanDevice.Device, bufferInfo, null, pVertexBuffer) != Result.Success)
            {
                throw new Exception("failed to create vertex buffer!");
            }
        }

        MemoryRequirements memRequirements = new();
        _vk.GetBufferMemoryRequirements(_vulkanDevice.Device, _vertexBuffer, out memRequirements);

        MemoryAllocateInfo allocateInfo = new()
        {
            SType = StructureType.MemoryAllocateInfo,
            AllocationSize = memRequirements.Size,
            MemoryTypeIndex = _vulkanDevice.FindMemoryType(memRequirements.MemoryTypeBits, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit),
        };

        fixed (DeviceMemory* pVertexBufferMemory = &_vertexBufferMemory)
        {
            if (_vk.AllocateMemory(_vulkanDevice.Device, allocateInfo, null, pVertexBufferMemory) != Result.Success)
            {
                throw new Exception("failed to allocate vertex buffer memory!");
            }
        }

        _ = _vk.BindBufferMemory(_vulkanDevice.Device, _vertexBuffer, _vertexBufferMemory, 0);

        void* pData;
        _ = _vk.MapMemory(_vulkanDevice.Device, _vertexBufferMemory, 0, bufferInfo.Size, 0, &pData);
        vertices.AsSpan().CopyTo(new Span<Vertex>(pData, vertices.Length));
        _vk.UnmapMemory(_vulkanDevice.Device, _vertexBufferMemory);
    }

    //
    // Other methods
    //

    public unsafe void Bind(CommandBuffer commandBuffer)
    {
        var vertexBuffers = new Buffer[] { _vertexBuffer };
        var offsets = new ulong[] { 0 };

        fixed (ulong* pOffsets = offsets)
        fixed (Buffer* pVertexBuffers = vertexBuffers)
        {
            _vk.CmdBindVertexBuffers(commandBuffer, 0, 1, pVertexBuffers, pOffsets);
        }
    }

    public void Draw(CommandBuffer commandBuffer) => _vk.CmdDraw(commandBuffer, _vertexCount, 1, 0, 0);
}
