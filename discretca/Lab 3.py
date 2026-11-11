def Factorial(n):
    if n < 0:
        raise ValueError("Factorial exists for numbers >= 0")
    if n <= 1: 
        return 1
    return Factorial(n-1) * n

def factorial_product(a : list) -> int:
    res = 1
    for i in a: res *= Factorial(i)
    return res

''' Task 1 '''
def Permutations(n : int) -> int:
    return Factorial(n)

''' Task 2 '''
# l - list of counts of elements
def Permutations_with_repetition(l : list[int]) -> int:
    if not(l):
        raise ValueError("no param")
    return Factorial(sum(l)) // factorial_product(l)

''' Task 3 '''
def Arrangements(n : int, m : int) -> int:
    return Factorial(n) // Factorial(n-m)

''' Task 4 '''
def Arrangements_with_repetition(n : int, m : int) -> int:
    return n**m

''' Task 5 '''
def Combinations(n : int, m : int) -> int:
    return Factorial(n) // (Factorial(m) * Factorial(n-m))

''' Task 6 '''
def Combinations_with_repetition(n : int, m : int) -> int:
    return Combinations(n+m-1, m)


