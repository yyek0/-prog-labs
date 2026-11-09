const frames = Array.from(
{
    length: 7
}
, (_, i) => `frames/frame${i + 1}.svg`);
frames.forEach(src =>
{
    const image = new Image(); image.src = src;
}
);
let current = 0;
let timer = null;
function showFrame()
{
    const label = `Кадр ${current + 1} из 7`;
    document.querySelector('#frame').src = frames[current];
    document.querySelector('#frame').alt = label;
    document.querySelector('#counter').textContent = label;
}
function stop()
{
    clearInterval(timer);
    timer = null;
}
document.querySelector('#play').addEventListener('click', () =>
{
    if (timer !== null) return; timer = setInterval(() =>
    {
        current = (current + 1) % frames.length; showFrame();
    }
    , 250);
}
);
document.querySelector('#stop').addEventListener('click', stop);
document.querySelector('#start').addEventListener('click', () =>
{
    stop(); current = 0; showFrame();
}
);
window.addEventListener('pagehide', stop);
