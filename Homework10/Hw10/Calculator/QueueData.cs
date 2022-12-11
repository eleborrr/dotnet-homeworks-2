using System.Linq.Expressions;

namespace Hw10.Calculator;


public struct QueueData
    {
        public Expression? Number = null;
        public string? Operation = null;
        public bool IsNumber = true;

        public QueueData(double number)
        {
            Number = Expression.Constant(number);
        }

        public override string ToString()
        {
            if(IsNumber)
                return Number.ToString();
            return Operation;
        }

        public QueueData(string operation)
        {
            Operation = operation;
            IsNumber = false;
        }
    }
