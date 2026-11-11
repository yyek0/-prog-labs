def caesar_cipher(text: str, shift: int, mode: str) -> str:
    result = ""
    
    for char in text:
        if char.isupper():
            result += chr((ord(char) - ord('A') + (shift if mode == 'encrypt' else -shift)) % 26 + ord('A'))
        elif char.islower():
            result += chr((ord(char) - ord('a') + (shift if mode == 'encrypt' else -shift)) % 26 + ord('a'))
        else:
            result += char
            
    return result

def encrypt(text: str, shift: int) -> str:
    return caesar_cipher(text, shift, 'encrypt')

def decrypt(text: str, shift: int) -> str:
    return caesar_cipher(text, shift, 'decrypt')