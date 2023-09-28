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
    public Expression()
    {
        this.coefficient = 0;
        this.constant = 0;
        this.variable = 'x';
        this.exponent = 1;
    }
    public Expression(float constant)
    {
        this.coefficient = 0;
        this.constant = constant;
        this.variable = 'x';
        this.exponent = 1;
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
        else if (a.coefficient == 0 && b.coefficient != 0)
        {
            return new Expression(b.variable, b.coefficient, b.exponent, a.constant + b.constant);
        }
        else if (a.coefficient != 0 && b.coefficient == 0)
        {
            return new Expression(a.variable, a.coefficient, a.exponent, a.constant + b.constant);
        }
        else if (a.coefficient == 0 && b.coefficient == 0)
        {
            return new Expression(a.constant + b.constant);
        }
        else
        {
            throw new InvalidOperationException("Variables or exponents do not match");
        }
    }

    public static Expression operator *(Expression a, Expression b)
    {
        float newCoefficient;
        float newExponent;
        float newConstant = a.constant * b.constant;

        if (a.coefficient != 0 && b.coefficient != 0)
        {
            newCoefficient = a.coefficient * b.coefficient;
            newExponent = a.exponent + b.exponent;
            return new Expression(a.variable, newCoefficient, newExponent, newConstant);
        }
        else if (a.coefficient == 0 && b.coefficient != 0)
        {
            return new Expression(b.variable, b.coefficient * a.constant, b.exponent, newConstant);
        }
        else if (a.coefficient != 0 && b.coefficient == 0)
        {
            return new Expression(a.variable, a.coefficient * b.constant, a.exponent, newConstant);
        }
        else
        {
            return new Expression(newConstant);
        }
    }



    public override string ToString()
    {
        string returnString = "";
        if (coefficient != 0)
        {
            if (coefficient != 1)
            {
                returnString += $"{coefficient}";
            }
            returnString += $"{variable}";
        }
        if (exponent != 1)
        {
            returnString += $"^{exponent}";
        }
        if (constant != 0)
        {
            if (returnString.Length != 0 && constant > 0)
            {
                returnString += "+";
            }
            returnString += $"{constant}";
        }
        if (returnString.Length == 0)
        {
            return "0";
        }
        return returnString;
    }
}