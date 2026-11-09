const form = document.querySelector('#club');
form.addEventListener('submit', event =>
{
    event.preventDefault(); document.querySelector('#result').textContent = 'Анкета заполнена правильно. Данные остались в браузере.';
}
);
form.addEventListener('reset', () =>
{
    document.querySelector('#result').textContent = '';
}
);
