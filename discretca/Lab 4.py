class TuringMachine:
    def __init__(self, tape: list, head: int = 0, state: str = 'q0'):
        self.tape = tape    
        self.head = head    
        self.state = state  

    def print_tape(self):
        tape_str = "".join(self.tape)
        print(f"Лента: {tape_str}")
        print(f"       {' ' * self.head}^ (Головка в состоянии {self.state})")
        print("-" * 30)

    ''' Task 1 '''
    def run_addition(self):
        print("Запуск МТ для сложения x + y...")
        self.print_tape()
        
        while self.state != 'halt':
            symbol = self.tape[self.head]
            
            if self.state == 'q0':
                if symbol == '|':
                    self.head += 1
                elif symbol == '*':
                    self.tape[self.head] = '|'
                    self.state = 'q1'
                    self.head += 1
                elif symbol == '_':
                    self.state = 'halt'
            
            elif self.state == 'q1':
                if symbol == '|':
                    self.head += 1
                elif symbol == '_': 
                    self.state = 'halt'
            
            self.print_tape()
        
        result = self.tape.count('|')
        print(f"Результат (сумма): {result}\n")

    ''' Task 2 '''
    def run_subtraction(self):
        print("Запуск МТ для вычитания x - y...")
        self.print_tape()
        
        while self.state != 'halt':
            symbol = self.tape[self.head]
            
            if self.state == 'q0':
                if symbol == '|':
                    self.head += 1
                elif symbol == '*':
                    self.tape[self.head] = '_'
                    self.state = 'q1'
                    self.head -= 1
            
            elif self.state == 'q1':
                if symbol == '|':
                    self.tape[self.head] = '_'
                    self.state = 'q2'
                    self.head += 2
                elif symbol == '_':
                    self.head -= 1
            
            elif self.state == 'q2':
                if symbol == '|':
                    self.tape[self.head] = '_'
                    self.state = 'q1'
                    self.head -= 1
                elif symbol == '_':
                    self.head += 1
                else:
                    self.state = 'halt'

            self.print_tape()
        
        result = self.tape.count('|')
        print(f"Результат (разность): {result}\n")

    ''' Task 3 '''
    def run_division_by_2(self):
        print("Запуск МТ для деления x / 2...")
        self.print_tape()
        count = 0
        
        while True:
            if self.tape.count('|') < 2:
                break
            
            while self.tape[self.head] != '|':
                self.head += 1
            self.tape[self.head] = '_'
            
            self.head += 1
            while self.tape[self.head] != '|':
                self.head += 1
            self.tape[self.head] = '_'
            
            count += 1
            self.head = 0
            self.print_tape()
            
        self.state = 'halt'
        print(f"Результат деления: {count} (остаток: {self.tape.count('|')})\n")