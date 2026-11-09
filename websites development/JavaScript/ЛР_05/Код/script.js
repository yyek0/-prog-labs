const quiz = document.querySelector('#quiz');
function grade(data)
{
    let score = 0;
    if (data.get('q1') === 'a') score++;
    const selected = data.getAll('q2').sort();
    if (selected.join(', ') === 'css, html, js') score++;
    if (data.get('q3') === 'no') score++;
    if ((data.get('q4') || '').trim().toLowerCase() === 'color') score++;
    return score;
}
quiz.addEventListener('submit', event =>
{
    event.preventDefault(); document.querySelector('#score').value = `Верных ответов: ${grade(new FormData(quiz))} из 4`;
}
);
quiz.addEventListener('reset', () =>
{
    document.querySelector('#score').value = 'Тест еще не проверен';
}
);
