class MarkovAlgorithm:
    def __init__(self, string: str, rules: list):
        self.string = string
        self.rules = rules
        self.step = 1

    def print_step(self, rule_left, rule_right, is_final):
        marker = " (ФИНАЛЬНОЕ)" if is_final else ""
        print(f"Шаг {self.step}: Правило '{rule_left} -> {rule_right}'{marker}")
        print(f"Строка: {self.string}")
        print("-" * 30)
        self.step += 1

    def run(self):
        print(f"Начальная строка: {self.string}")
        
        while True:
            applied = False
            for left, right, is_final in self.rules:
                if left in self.string:
                    self.string = self.string.replace(left, right, 1)
                    self.print_step(left, right, is_final)
                    
                    if is_final:
                        self.string = self.string.replace('.', '')
                        print(f"Конец работы. Итог: {self.string}\n")
                        return self.string
                    
                    applied = True
                    break
            
            if not applied:
                print("Нет применимых правил. Работа завершена.\n")
                return self.string

    ''' Task 1 '''
    def run_addition_5(self):
        print("Запуск НАМ для сложения x + 5...")
        rules_add_5 = [
           ("*", "|||||", True)
        ]
        self.rules = rules_add_5
        self.run()

    ''' Task 2 '''
    def run_subtraction_5(self):
        print("Запуск НАМ для вычитания x - 5...")
        rules_sub_5 = [
            ("*", "", False),
            ("|||||", "", True)
        ]
        self.rules = rules_sub_5
        self.run()