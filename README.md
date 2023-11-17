# MatrixCalculator
> Calculator that processes matrix operations

## Expressions:

- coefficient: float (optional)
- variable: char 
- exponent: float (optional)
- constant: float

When prompted for a number by a program, it prompts you for an expression. This expression needs to include at least a variable or a constant but can include any of the parts above as well.

Example input: `5` Expression made: `0x^1+5`

Example input: `x` Expression made: `1x^1+0`

## Matrices
A matrix is a list of rows, which contain Expressions.


### `new` command
- param: name

Starts the process of creating a matrix with the given name

ex: `new A`

The user can put in `:i` to create an identity matrix for a given size,

ex: `:i 5` will create a 5x5 identity matrix.

### `det` command
- param: matrix

If the matrix name exists, it will print the determinant of the matrix as an Expression.

## Operations

Perform an operation by putting an operation symbol between two matrices
Ex: `A + B`
- param: firstMatrix
- param: operation
- param secondMatrix

List of operators:
- `+`
- `-`
- `*`
