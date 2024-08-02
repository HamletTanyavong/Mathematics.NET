<script>
    document.addEventListener("DOMContentLoaded", function () {
    const interactiveCard = document.getElementById("interactive-card");

    const overlay = document.getElementById("overlay");

    const circleOne = document.getElementById("circle-one");
    const circleOneRadius = circleOne.offsetWidth / 2;

    const circleTwo = document.getElementById("circle-two");
    const circleTwoRadius = circleTwo.offsetWidth / 2;

    const circleThree = document.getElementById("circle-three");
    const circleThreeRadius = circleThree.offsetWidth / 2;

    interactiveCard.addEventListener("mousemove", function (event) {
        var boundingRectangle = interactiveCard.getBoundingClientRect();
        var centerX = interactiveCard.clientWidth / 2;
        var centerY = interactiveCard.clientHeight / 2;
        var x = event.clientX - boundingRectangle.left - centerX;
        var y = event.clientY - boundingRectangle.top - centerY;

        var magnitude = Math.hypot(x, y);
        var maxMagnitude = Math.hypot(centerX, centerY);

        overlay.style.backgroundColor = `rgba(255, 255, 255, ${0.1 * Math.cos(magnitude / maxMagnitude * Math.PI / 2)})`;

        circleOne.style.left = `${x / 2 - circleOneRadius + centerX}px`;
        circleOne.style.top = `${y / 2 - circleOneRadius + centerY}px`;

        circleTwo.style.left = `${-x - circleTwoRadius + centerX}px`;
        circleTwo.style.top = `${-y - circleTwoRadius + centerY}px`;

        circleThree.style.left = `${-4 * x - circleThreeRadius + centerX}px`;
        circleThree.style.top = `${-4 * y - circleThreeRadius + centerY}px`;
    });
});
</script>

<div id="interactive-card">
    <div id="overlay"></div>
    <div class="flare" id="circle-one"></div>
    <div class="flare" id="circle-two"></div>
    <div class="flare" id="circle-three"></div>
    <img src="https://raw.githubusercontent.com/HamletTanyavong/Mathematics.NET/gh-pages/images/logo/mathematics.net.svg" width="128" height="128" style="margin: 16px" alt="Mathematics.NET Logo">
    <h1>Mathematics.NET</h1>
    <p>Mathematics.NET is a C# class library that provides tools for solving mathematical problems.</p>
</div>

[![GitHub](https://img.shields.io/github/license/HamletTanyavong/Mathematics.NET?style=flat-square&logo=github&labelColor=87cefa&color=ffd700)](https://github.com/HamletTanyavong/Mathematics.NET)
[![GitHub Repo Stars](https://img.shields.io/github/stars/HamletTanyavong/Mathematics.NET?color=87cefa&style=flat-square&logo=github)](https://github.com/HamletTanyavong/Mathematics.NET/stargazers)
[![NuGet](https://img.shields.io/nuget/v/Physics.NET.Mathematics?style=flat-square&logo=nuget)](https://www.nuget.org/packages/Physics.NET.Mathematics)
[![NuGet](https://img.shields.io/nuget/dt/Physics.NET.Mathematics?style=flat-square&logo=nuget)](https://www.nuget.org/packages/Physics.NET.Mathematics)

## About

Mathematics.NET provides custom types for complex, real, and rational numbers as well as other mathematical objects such as vectors, matrices, and tensors. Mathematics.NET also supports first and second-order, forward and reverse-mode automatic differentiation.
