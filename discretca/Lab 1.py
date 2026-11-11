''' Task 1 '''
def Union_of_sets(a: set, b: set) -> set:
    result = b.copy()
    for i in a:
        result.add(i)
    return result

''' Task 2 '''
def Crossing_of_sets(a: set, b: set) -> set:
    return set(i for i in a if i in b)

''' Task 3 '''
def Symmetric_difference_of_sets(a: set, b: set) -> set:
    result = set()
    cross = Crossing_of_sets(a,b)
    for i in a:
        if i not in cross:
            result.add(i)

    for i in b:
        if i not in cross:
            result.add(i)

    return result

''' Task 4-5 '''
def Set_difference(a: set, b: set) -> set:
    return {i for i in a if i not in b}

