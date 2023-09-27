class Expression
{
    float constant;
    float coefficient;
    char variable;
    float exponent;
    //  coefficient(variable) + constant

    public Expression(char variable, float coefficient = 1, float exponent = 1, float constant = 0)
    {
        this.coefficient = coefficient;
        this.constant = constant;
        this.variable = variable;
        this.exponent = exponent;
    }

    public static Expression operator +(Expression a, float b)
    {
        return new Expression(a.variable, a.coefficient, a.exponent, (a.constant + b));
    }
    public static Expression operator *(Expression a, float b)
    {
        return new Expression(a.variable, (a.coefficient * b), a.exponent, (a.constant * b));
    }
    public static Expression operator +(Expression a, Expression b)
    {
        if (a.variable == b.variable && a.exponent == b.exponent)
        {
            return new Expression(a.variable, a.coefficient + b.coefficient, a.exponent, a.constant + b.constant);
        }
        throw new InvalidOperationException("Variables or exponents do not match");
    }
    public static Expression operator *(Expression a, Expression b)
    {
        if (a.variable == b.variable)
        {
            return new Expression(a.variable, a.coefficient * b.coefficient, a.exponent + b.exponent, a.constant * b.constant);
        }
        throw new InvalidOperationException("Variables do not match");
    }
    
    public override string ToString()
    {
        return $"{coefficient}{variable}^{exponent} + {constant}";
    }
}