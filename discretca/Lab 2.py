''' Task 1 (a) '''
def Fibonacci(n: int) -> int:
    if n < 0:
        raise ValueError("Fibonacci nums starts from 0")
    if n == 0: 
        return 0
    if n == 1: 
        return 1
    return Fibonacci(n-1) + Fibonacci(n-2)

''' Task 1 (b) '''
def FibonacciSum(n : int) -> int:
    if n < 0:
        raise ValueError("Fibonacci nums starts from 0")
    return Fibonacci(n+2) - 1

''' Task 2 '''
PI = 3.1415926535898
def Sin1(x: float) -> float:
    while not(0 <= x <= 2*PI):
        if x > 2*PI:
            x -= 2*PI
        else:
            x += 2*PI

    sign = 1
    if x > PI: 
        sign = -1
        x = 2 * PI - x

    if x > PI/2: 
        x = PI-x

    if x < 0.000001:
        return sign * x

    half_sin = Sin1(x/2)
    full_sin = 2 * half_sin * (1 - half_sin**2)**0.5
    return sign * full_sin

# --------

def Factorial(n : int) -> int:
    if n < 0:
        raise ValueError("Factorial exists for numbers >= 0")
    if n <= 1: 
        return 1
    return n * Factorial(n - 1)

def Sin2(x : float) -> float:
    return sum((-1)**n/Factorial(2*n+1) * x**(2*n+1) for n in range(0, 100))

''' Task 3 '''
def Abs(x : float) -> float:
    if x < 0:
        return -1 * x
    return x

def Sqrt(s : float) -> float:
    if s < 0:
        raise ValueError("Невозможно вычислить квадратный корень из отрицательного числа")
    if s == 0:
        return 0.0

    x = s / 2.0 if s > 1 else 1.0
    while True:
        next_x = 0.5 (x + s/x)
        if Abs(next - x) <= 1e-10:
            return next_x
        x = next_x

