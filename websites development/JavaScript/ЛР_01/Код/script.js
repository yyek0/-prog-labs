const openedAt = new Date();
const twoDigits = value => String(value).padStart(2, '0');
document.querySelector('#date').textContent = `сегодня ${openedAt.getDate()}/${openedAt.getMonth() + 1}/${openedAt.getFullYear()}`;
document.querySelector('#time').textContent = `время ${twoDigits(openedAt.getHours())}:${twoDigits(openedAt.getMinutes())}:${twoDigits(openedAt.getSeconds())}`;
