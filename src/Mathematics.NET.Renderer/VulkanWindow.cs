// <copyright file="VulkanWindow.cs" company="Mathematics.NET">
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

using Silk.NET.Core.Contexts;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Mathematics.NET.Renderer;

public class VulkanWindow : IDisposable
{
    private string _title;
    private int _width;
    private int _height;
    private IWindow _window = null!;
    private bool _frameBufferResized;

    private bool _disposed;

    public VulkanWindow(string title, int width, int height)
    {
        _title = title;
        _width = width;
        _height = height;

        InitializeWindow();
    }

    public Extent2D Extent => new((uint)_width, (uint)_height);

    public IVkSurface VkSurface => _window.VkSurface ?? throw new ApplicationException("VkSurface is null");

    public IWindow Window => _window;

    public bool WasWindowResized => _frameBufferResized;

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
                _window.Dispose();
            }

            _disposed = true;
        }
    }

    //
    // Constructor methods
    //

    private void InitializeWindow()
    {
        var options = WindowOptions.DefaultVulkan;
        options.Title = _title;
        options.Size = new Vector2D<int>(_width, _height);

        _window = Silk.NET.Windowing.Window.Create(options);
        _window.Resize += Resize;

        _window.Initialize();

        if (_window.VkSurface is null)
        {
            throw new PlatformNotSupportedException("Vulkan is not supported on this platform");
        }
    }

    //
    // Other methods
    //

    public void ResetWindowResizedFlag() => _frameBufferResized = false;

    private void Resize(Vector2D<int> size)
    {
        _frameBufferResized = true;
    }

    public void Run() => _window.Run();
}
