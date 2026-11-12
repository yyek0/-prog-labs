class PostMachine:
    def __init__(self, tape: list, head: int = 0):
        self.tape = tape
        self.head = head
        self.commands = []
        self.pc = 1

    def print_tape(self):
        tape_str = "".join(self.tape)
        print(f"Лента: {tape_str}")
        print(f"       {' ' * self.head}^ (Команда №{self.pc})")
        print("-" * 30)

    def set_commands(self, commands: list):
        self.commands = commands

    def run(self):
        print("Запуск машины Поста...")
        self.print_tape()

        while 0 < self.pc <= len(self.commands):
            cmd = self.commands[self.pc - 1]
            action = cmd[0]
            next_pc = cmd[1]

            if action == '0':
                break
            if action == 'V':
                self.tape[self.head] = '|'
                self.pc = next_pc
            elif action == 'X':
                self.tape[self.head] = '_'
                self.pc = next_pc
            elif action == 'R':
                self.head += 1
                if self.head >= len(self.tape):
                    self.tape.append('_')
                self.pc = next_pc
            elif action == 'L':
                if self.head > 0:
                    self.head -= 1
                self.pc = next_pc
            elif action == '?':
                target = cmd[2]
                if self.tape[self.head] == '|':
                    self.pc = next_pc
                else:
                    self.pc = target

            self.print_tape()

        print(f"Машина остановлена. Результат на ленте: {''.join(self.tape)}\n")

    ''' Task 1 '''
    def run_addition(self):
        print("Задание а) F(x,y) = x + y")
        cmds = [
            ('R', 2),
            ('?', 3, 1),
            ('V', 4),
            ('R', 5),
            ('?', 4, 6),
            ('0', 0)
        ]
        self.set_commands(cmds)
        self.run()

    ''' Task 2 '''
    def run_addition_4(self):
        print("Задание б) F(x) = x + 4")
        cmds = [
            ('R', 2),
            ('?', 1, 3),
            ('V', 4),
            ('R', 5),
            ('V', 6),
            ('R', 7),
            ('V', 8),
            ('R', 9),
            ('V', 10),
            ('0', 0)
        ]
        self.set_commands(cmds)
        self.run()

    ''' Task 3 '''
    def run_subtraction_4(self):
        print("Задание в) F(x) = x - 4")
        cmds = [
            ('R', 2),
            ('?', 1, 3),
            ('L', 4),
            ('X', 5),
            ('L', 6),
            ('X', 7),
            ('L', 8),
            ('X', 9),
            ('L', 10),
            ('X', 11),
            ('0', 0)
        ]
        self.set_commands(cmds)
        self.run()