let account = null;
const register = document.querySelector('#register');
const loginForm = document.querySelector('#loginForm');
const recovery = document.querySelector('#recovery');
const status = document.querySelector('#status');
function goodbye()
{
    status.textContent = 'До свидания';
    loginForm.hidden = true;
    recovery.hidden = true;
}
register.addEventListener('submit', event =>
{
    event.preventDefault(); const data = new FormData(register); account = Object.fromEntries([...data].map(([key, value]) => [key, value.trim()])); if (Object.values(account).some(value => !value))
    {
        status.textContent = 'Все поля должны быть заполнены.'; return;
    }
    document.querySelector('#summary').textContent = `Ваш логин: ${account.login}, пароль: ${account.password}, контрольный вопрос: ${account.question}. Сейчас будет проведен тестовый вход на сайт.`; register.hidden = true; loginForm.hidden = false; status.textContent = '';
}
);
document.querySelector('#decline').addEventListener('click', () =>
{
    register.hidden = true; goodbye();
}
);
loginForm.addEventListener('submit', event =>
{
    event.preventDefault(); const data = new FormData(loginForm); if (data.get('login').trim() !== account.login) return goodbye(); if (data.get('password').trim() === account.password) return location.assign('welcome.html'); document.querySelector('#question').textContent = account.question; recovery.hidden = false; loginForm.hidden = true;
}
);
recovery.addEventListener('submit', event =>
{
    event.preventDefault(); const answer = new FormData(recovery).get('answer').trim().toLocaleLowerCase('ru'); if (answer === account.answer.toLocaleLowerCase('ru')) location.assign('welcome.html'); else goodbye();
}
);
