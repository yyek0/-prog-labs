const form = document.querySelector('#account');
const status = document.querySelector('#status');
const signIn = document.querySelector('#sign-in');
let account;

function value(name)
{
    return form.elements[name].value.trim();
}

function show(message)
{
    status.textContent = message;
}

document.querySelector('#register').addEventListener('click', function ()
{
    const login = value('login');
    const password = value('password');
    const question = value('question');
    const answer = value('answer');

    if (!login || !password || !question || !answer)
    {
        show('Заполните все поля регистрации.');
        return;
    }

    account = { login, password, question, answer };
    signIn.hidden = false;
    form.elements.enteredLogin.focus();
    show(`Регистрация завершена. Контрольный вопрос: ${question}`);
});

document.querySelector('#enter').addEventListener('click', function ()
{
    if (!account)
    {
        show('Сначала зарегистрируйтесь.');
        return;
    }

    const login = value('enteredLogin');
    const password = value('enteredPassword');
    const recovery = value('recovery');

    if (login !== account.login)
    {
        show('Логин введён неверно.');
        return;
    }

    if (password === account.password || recovery.toLocaleLowerCase('ru') === account.answer.toLocaleLowerCase('ru'))
    {
        location.assign('welcome.html');
        return;
    }

    show('Пароль или ответ на контрольный вопрос введён неверно.');
});
