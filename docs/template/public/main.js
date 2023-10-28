export default {
  defaultTheme: 'dark',
  iconLinks: [
    {
      icon: 'github',
      href: 'https://github.com/HamletTanyavong/Mathematics.NET',
      title: 'GitHub'
    },
    {
      icon: 'heart',
      href: 'https://github.com/sponsors/HamletTanyavong',
      title: 'Sponsor'
    }
  ]
}

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
