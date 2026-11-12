import re


class TuringMachine:
    def __init__(self, tape: list, head: int = 0, state: str = 'q0'):
        self.tape = list(tape) if tape else ['_']
        self.head = head
        self.state = state

    def print_tape(self):
        print(f"Лента: {''.join(self.tape)}")
        print(f"       {' ' * self.head}^ (Головка в состоянии {self.state})")
        print("-" * 30)

    def _read(self):
        if self.head < 0:
            raise ValueError("Головка вышла за левую границу ленты")
        while self.head >= len(self.tape):
            self.tape.append('_')
        return self.tape[self.head]

    def _prepare_binary(self):
        if not re.fullmatch(r'\|*\*\|*_*', ''.join(self.tape).replace(' ', '')):
            raise ValueError("Ожидалась лента вида '|||*||_' (числа разделены '*')")
        self.head, self.state = 0, 'q0'

    def _prepare_unary(self):
        if not re.fullmatch(r'\|*_*', ''.join(self.tape).replace(' ', '')):
            raise ValueError("Ожидалась лента вида '|||||_'")
        self.head, self.state = 0, 'q0'

    # Задание а: убираем один символ с правого края после замены * на |.
    def run_addition(self):
        self._prepare_binary()
        print("Запуск МТ для сложения x + y...")
        self.print_tape()
        while self.state != 'halt':
            symbol = self._read()
            if self.state == 'q0':
                if symbol == '|':
                    self.head += 1
                else:  # '*'
                    self.tape[self.head] = '|'
                    self.head += 1
                    self.state = 'q1'
            elif self.state == 'q1':
                if symbol == '|':
                    self.head += 1
                else:  # конец второго числа
                    self.head -= 1
                    self.state = 'q2'
            elif self.state == 'q2':
                self.tape[self.head] = '_'
                self.state = 'halt'
            self.print_tape()
        result = self.tape.count('|')
        print(f"Результат (сумма): {result}\n")
        return result

    # Задание б: для каждой палочки y удаляем крайнюю палочку x.
    def run_subtraction(self):
        self._prepare_binary()
        sep = self.tape.index('*')
        x = sum(ch == '|' for ch in self.tape[:sep])
        y = sum(ch == '|' for ch in self.tape[sep + 1:])
        if x < y:
            raise ValueError("В унарной записи здесь требуется x >= y")

        print("Запуск МТ для вычитания x - y...")
        self.print_tape()
        while self.state != 'halt':
            symbol = self._read()
            if self.state == 'q0':
                if symbol == '|':
                    self.head += 1
                else:  # '*'
                    self.head += 1
                    self.state = 'q1'
            elif self.state == 'q1':  # ищем ещё не зачёркнутую палочку y
                if symbol == '|':
                    self.tape[self.head] = 'x'
                    self.head -= 1
                    self.state = 'q2'
                elif symbol == 'x':
                    self.head += 1
                else:  # '_' справа от второго числа
                    self.head -= 1
                    self.state = 'q5'
            elif self.state == 'q2':  # назад до разделителя
                if symbol == '*':
                    self.head -= 1
                    self.state = 'q3'
                else:
                    self.head -= 1
            elif self.state == 'q3':  # стираем одну палочку x справа налево
                if symbol == '|':
                    self.tape[self.head] = '_'
                    self.head += 1
                    self.state = 'q4'
                else:
                    self.head -= 1
            elif self.state == 'q4':  # возвращаемся к числу y
                if symbol == '*':
                    self.head += 1
                    self.state = 'q1'
                else:
                    self.head += 1
            elif self.state == 'q5':  # уборка пометок и разделителя
                if symbol == 'x':
                    self.tape[self.head] = '_'
                    self.head -= 1
                elif symbol == '*':
                    self.tape[self.head] = '_'
                    self.state = 'halt'
                else:
                    self.head -= 1
            self.print_tape()

        result = self.tape.count('|')
        print(f"Результат (разность): {result}\n")
        return result

    # Задание в: удаляем пары палочек; выдаём частное и остаток.
    def run_division_by_2(self):
        self._prepare_unary()
        print("Запуск МТ для деления x / 2...")
        self.print_tape()
        count = 0
        while self.tape.count('|') >= 2:
            for _ in range(2):
                self.head = self.tape.index('|')
                self.tape[self.head] = '_'
            count += 1
            self.head = 0
            self.print_tape()
        remainder = self.tape.count('|')
        # Представляем полученное частное на ленте в унарной записи.
        self.tape = ['|'] * count + ['_']
        self.head, self.state = 0, 'halt'
        self.print_tape()
        print(f"Результат деления: {count} (остаток: {remainder})\n")
        return count, remainder
