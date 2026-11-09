function questionWord(count)
{
    if (count === 1)
    {
        return 'вопрос';
    }

    if (count >= 2 && count <= 4)
    {
        return 'вопроса';
    }

    return 'вопросов';
}

const answers = [5, 5, 42, 5, 2];
const form = document.querySelector('#quiz');
const result = document.querySelector('#result');

form.addEventListener('submit', function (event)
{
    event.preventDefault();

    const data = new FormData(form);
    let score = 0;

    for (let index = 0; index < answers.length; index++)
    {
        if (Number(data.get(`q${index + 1}`)) === answers[index])
        {
            score++;
        }
    }

    result.textContent = `${data.get('last').trim()} ${data.get('first').trim()} правильно ответил на ${score} ${questionWord(score)} из 5.`;
});

form.addEventListener('reset', function ()
{
    result.textContent = '';
});
